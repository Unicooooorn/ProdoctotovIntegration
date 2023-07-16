using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Mapping;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Application.Response;
using ProdoctorovIntegration.Application.Services;

namespace ProdoctorovIntegration.Infrastructure.Services;

public class ScheduleService : IScheduleService
{
    private readonly IClientService _clientService;
    private readonly HospitalDbContext _dbContext;
    private readonly OrganizationNameOptions _organizationNameOptions;

    public ScheduleService(IClientService clientService, HospitalDbContext dbContext, IOptions<OrganizationNameOptions> organizationNameOptions)
    {
        _clientService = clientService;
        _dbContext = dbContext;
        _organizationNameOptions = organizationNameOptions.Value;
    }

    public async Task<GetScheduleResponse> GetScheduleAsync(CancellationToken cancellationToken = default)
    {
        var events = await _dbContext.Event.Where(
                e => e.IsForProdoctorov && e.StartDate > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
        return events.Count > 0 ? events.MapToResponse(_organizationNameOptions.Name) : new GetScheduleResponse();
    }

    public async Task<IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>> GetOccupiedDoctorScheduleSlotAsync(CancellationToken cancellationToken = default)
    {
        var events = await _dbContext.Event.Where(x => x.IsForProdoctorov).ToListAsync(cancellationToken);

        return events.MapOccupiedSlotsToResponse().ToArray();
    }

    public async Task<CheckAppointmentByClaimResponse> CheckAppointmentByClaimAsync(string claimId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(claimId, out var guid))
            return new CheckAppointmentByClaimResponse
            {
                StatusCode = 204,
                ClaimStatus = "cancelled"
            };

        var cell = await _dbContext.Event.FirstOrDefaultAsync(x => x.ClaimId == guid, cancellationToken);

        return new CheckAppointmentByClaimResponse
        {
            StatusCode = 204,
            ClaimStatus = cell?.Client != null
                ? "successfully"
                : "cancelled",
            DateStart = cell?.StartDate,
            DateEnd = cell?.StartDate.AddMinutes(cell.Duration)
        };
    }

    public async Task<RefreshAppointmentResponse> RefreshAppointmentAsync(string claimIdString, WorkerDto? worker, AppointmentDto? appointment, ClientDto? client, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(claimIdString, out var claimId))
            return new RefreshAppointmentResponse
            {
                StatusCode = 404
            };

        if (worker is not null || appointment is not null || client is not null)
        {
            return await RefreshInOtherAppointmentAsync(claimId, worker, appointment, client, cancellationToken);
        }

        return await _dbContext.Event.AnyAsync(x => x.ClaimId == claimId, cancellationToken)
            ? new RefreshAppointmentResponse
            {
                StatusCode = 204
            }
            : new RefreshAppointmentResponse
            {
                StatusCode = 404
            };
    }


    public async Task<RecordClientResponse> RecordClientAsync(WorkerDto worker, AppointmentDto appointment, ClientDto client, string appointmentSource, CancellationToken cancellationToken = default)
    {
        var clientId = await _clientService.FindClientAsync(client, cancellationToken);

        if (!Guid.TryParse(worker.Id, out var workerId))
            return new RecordClientResponse
            {
                StatusCode = 416,
                Detail = "Slot doesn't exist"
            };

        var adjacentAppointmentExists = await _dbContext.Event.AnyAsync(
            x => x.Client != null && x.Client.Id == clientId &&
                 x.StartDate == appointment.DateStart &&
                 x.Worker.Id != workerId, cancellationToken);

        if (adjacentAppointmentExists)
            return new RecordClientResponse
            {
                StatusCode = 409,
                Detail = "The client has an appointment with another doctor at this time"
            };

        var duration = (appointment.DateEnd - appointment.DateStart).Minutes;
        var isExistsCell = await _dbContext.Event.AnyAsync(
            x => x.StartDate == appointment.DateStart && x.Duration == duration &&
                 x.Worker.Id == workerId, cancellationToken);
        if (!isExistsCell)
            return new RecordClientResponse
            {
                StatusCode = 416,
                Detail = "Slot doesn't exist"
            };

        var cell = await _dbContext.Event.FirstOrDefaultAsync(x => x.StartDate == appointment.DateStart
                                                                         && x.Duration == duration
                                                                         && x.Worker.Id == workerId
                                                                         && x.Client == null,
            cancellationToken);

        if (cell is null)
            return new RecordClientResponse
            {
                StatusCode = 423,
                Detail = "Slot is busy"
            };

        var eventClient = await _dbContext.Client.FirstOrDefaultAsync(x => x.Id == clientId, cancellationToken);

        cell.Client = eventClient;
        cell.ClaimId = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RecordClientResponse
        {
            StatusCode = 204,
            ClaimId = cell.ClaimId?.ToString()!
        };
    }

    public async Task<CancelAppointmentResponse> CancelAppointmentAsync(string claimIdString, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(claimIdString, out _))
            return new CancelAppointmentResponse
            {
                StatusCode = 204
            };

        var claimId = Guid.Parse(claimIdString);
        var appointment = await _dbContext.Event.FirstOrDefaultAsync(x => x.ClaimId == claimId, cancellationToken);

        if (appointment == null)
            return new CancelAppointmentResponse
            {
                StatusCode = 204
            };

        appointment.Client = null;
        appointment.ClientData = string.Empty;
        appointment.Note = string.Empty;
        appointment.ClaimId = null;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CancelAppointmentResponse
        {
            StatusCode = 204
        };
    }

    private async Task<RefreshAppointmentResponse> RefreshInOtherAppointmentAsync(Guid claimId, WorkerDto? worker, AppointmentDto? appointment, ClientDto? client, CancellationToken cancellationToken = default)
    {
        var currentAppointment =
            await _dbContext.Event.FirstOrDefaultAsync(x => x.ClaimId == claimId, cancellationToken);
        if (currentAppointment is null)
            return new RefreshAppointmentResponse
            {
                StatusCode = 404
            };

        if (!Guid.TryParse(worker?.Id, out var workerId) && worker is not null)
            return new RefreshAppointmentResponse
            {
                StatusCode = 404
            };

        var newWorker =
            await _dbContext.Worker.FirstOrDefaultAsync(x => x.Id == workerId,
                cancellationToken);
        if (newWorker is not null)
            currentAppointment.Worker = newWorker;

        if (client != null)
        {

            var clientId = await _clientService.FindClientAsync(client, cancellationToken);
            var newClient = await _dbContext.Client.FirstOrDefaultAsync(x => x.Id == clientId, cancellationToken);
            currentAppointment.Client = newClient;
        }

        var newAppointment = await _dbContext.Event.FirstOrDefaultAsync(x =>
            appointment != null && newWorker != null && x.Worker.Id == newWorker.Id &&
            x.StartDate == appointment.DateStart
            && x.Duration == (appointment.DateEnd - appointment.DateStart).Minutes, cancellationToken);

        if (newAppointment is not null)
        {
            newAppointment.Client = currentAppointment.Client;
            newAppointment.ClaimId = claimId;
            currentAppointment.Client = null;
            currentAppointment.ClaimId = null;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return new RefreshAppointmentResponse
        {
            StatusCode = 204
        };
    }
}
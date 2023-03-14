using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Requests.FindClient;
using ProdoctorovIntegration.Domain.Client;
using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Command.RecordClient;

public class RecordClientCommand : IRequest<RecordClientResponse>
{
    [JsonPropertyName("doctor")] 
    public WorkerDto Worker { get; set; } = new();
    [JsonPropertyName("appointment")]
    public AppointmentDto Appointment { get; set; } = new();
    [JsonPropertyName("client")] 
    public ClientDto Client { get; set; } = new();
    [JsonPropertyName("appointment_source")]
    public string AppointmentSource { get; set; } = string.Empty;
}

public class RecordClientCommandHandler : IRequestHandler<RecordClientCommand, RecordClientResponse>
{
    private readonly HospitalDbContext _dbContext;
    private readonly IMediator _mediator;

    public RecordClientCommandHandler(HospitalDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<RecordClientResponse> Handle(RecordClientCommand command, CancellationToken cancellationToken)
    {
        var request = new FindOrCreateClientRequest
        {
            Client = command.Client
        };
        var clientId = await _mediator.Send(request, cancellationToken);

        var adjacentAppointmentExists = await _dbContext.Event.AnyAsync(
            x => x.Client != null && x.Client.Id == clientId &&
                 x.StartDate == command.Appointment.DateStart &&
                 x.Worker.Id != command.Worker.Id, cancellationToken);

        if (adjacentAppointmentExists)
            return new RecordClientResponse
            {
                StatusCode = 409,
                Detail = "The client has an appointment with another doctor at this time"
            };

        var duration = (command.Appointment.DateEnd - command.Appointment.DateStart).Minutes;
        var isExistsCell = await _dbContext.Event.AnyAsync(
            x => x.StartDate == command.Appointment.DateStart && x.Duration == duration &&
                 x.Worker.Id == command.Worker.Id, cancellationToken);
        if (!isExistsCell)
            return new RecordClientResponse
            {
                StatusCode = 416,
                Detail = "Slot doesn't exist"
            };

        var cell = await _dbContext.Event.FirstOrDefaultAsync(x => x.StartDate == command.Appointment.DateStart
                                                                         && x.Duration == duration
                                                                         && x.Worker.Id == command.Worker.Id
                                                                         && x.Client == null,
            cancellationToken);

        if (cell is null)
            return new RecordClientResponse
            {
                StatusCode = 423,
                Detail = "Slot is busy"
            };

        var client = await _dbContext.Client.FirstOrDefaultAsync(x => x.Id == clientId, cancellationToken);

        cell.Client = client;
        cell.ClaimId = Guid.NewGuid();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RecordClientResponse
        {
            StatusCode = 204,
            ClaimId = cell.ClaimId?.ToString()!
        };
    }
}
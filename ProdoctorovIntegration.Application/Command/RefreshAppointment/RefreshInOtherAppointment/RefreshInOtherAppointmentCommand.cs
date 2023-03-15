using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Requests.FindClient;

namespace ProdoctorovIntegration.Application.Command.RefreshAppointment.RefreshInOtherAppointment;

public class RefreshInOtherAppointmentCommand : IRequest<RefreshAppointmentResponse>
{
    public Guid ClaimId { get; set; }
    public WorkerDto? Worker { get; set; }
    public AppointmentDto? Appointment { get; set; }
    public ClientDto? Client { get; set; }
}

public class
    RefreshInOtherAppointmentCommandHandler : IRequestHandler<RefreshInOtherAppointmentCommand,
        RefreshAppointmentResponse>
{
    private readonly HospitalDbContext _dbContext;
    private readonly IMediator _mediator;

    public RefreshInOtherAppointmentCommandHandler(HospitalDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<RefreshAppointmentResponse> Handle(RefreshInOtherAppointmentCommand request, CancellationToken cancellationToken)
    {
        var currentAppointment =
            await _dbContext.Event.FirstOrDefaultAsync(x => x.ClaimId == request.ClaimId, cancellationToken);
        if (currentAppointment is null)
            return new RefreshAppointmentResponse
            {
                StatusCode = 404
            };

        var worker =
            await _dbContext.Worker.FirstOrDefaultAsync(x => request.Worker != null && x.Id == request.Worker.Id,
                cancellationToken);
        if(worker is not null)
            currentAppointment.Worker = worker;

        if (request.Client != null)
        {
            var findOrCreateClientRequest = new FindOrCreateClientRequest
            {
                Client = request.Client
            };

            var clientId = await _mediator.Send(findOrCreateClientRequest, cancellationToken);
            var client = await _dbContext.Client.FirstOrDefaultAsync(x => x.Id == clientId, cancellationToken);
            currentAppointment.Client = client;
        }

        var newAppointment = await _dbContext.Event.FirstOrDefaultAsync(x =>
            request.Appointment != null && worker != null && x.Worker.Id == worker.Id &&
            x.StartDate == request.Appointment.DateStart
            && x.Duration == (request.Appointment.DateEnd - request.Appointment.DateStart).Minutes, cancellationToken);

        if (newAppointment is not null)
        {
            newAppointment.Client = currentAppointment.Client;
            newAppointment.ClaimId = request.ClaimId;
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
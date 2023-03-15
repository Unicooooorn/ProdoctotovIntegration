using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Command.RefreshAppointment.RefreshInOtherAppointment;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;

namespace ProdoctorovIntegration.Application.Command.RefreshAppointment;

public class RefreshAppointmentCommand : IRequest<RefreshAppointmentResponse>
{
    [JsonPropertyName("claim_id")]
    public string ClaimId { get; set; } = string.Empty;
    public WorkerDto? Worker { get; set; }
    public AppointmentDto? Appointment { get; set; }
    public ClientDto? Client { get; set; }
}

public class RefreshAppointmentCommandHandler : IRequestHandler<RefreshAppointmentCommand, RefreshAppointmentResponse>
{
    private readonly HospitalDbContext _dbContext;
    private readonly IMediator _mediator;

    public RefreshAppointmentCommandHandler(HospitalDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<RefreshAppointmentResponse> Handle(RefreshAppointmentCommand command, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(command.ClaimId, out var claimId))
            return new RefreshAppointmentResponse
            {
                StatusCode = 404
            };

        if (command.Worker is not null || command.Appointment is not null || command.Client is not null)
        {
            var refreshInOtherAppointmentCommand = new RefreshInOtherAppointmentCommand
            {
                ClaimId = claimId,
                Appointment = command.Appointment,
                Worker = command.Worker,
                Client = command.Client
            };
            return await _mediator.Send(refreshInOtherAppointmentCommand, cancellationToken);
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
}
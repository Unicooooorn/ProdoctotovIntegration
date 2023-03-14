using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.DbContext;

namespace ProdoctorovIntegration.Application.Command.CancelAppointment;

public class CancelAppointmentCommand : IRequest<CancelAppointmentResponse>
{
    public CancelAppointmentCommand(string claimId)
    {
        ClaimId = claimId;
    }
    public string ClaimId { get; set; }
}

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, CancelAppointmentResponse>
{
    private readonly HospitalDbContext _dbContext;

    public CancelAppointmentCommandHandler(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CancelAppointmentResponse> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.ClaimId, out _))
            return new CancelAppointmentResponse
            {
                StatusCode = 204
            };

        var claimId = Guid.Parse(request.ClaimId);
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
}
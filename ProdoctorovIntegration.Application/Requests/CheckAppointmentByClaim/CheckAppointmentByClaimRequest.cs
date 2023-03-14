using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.DbContext;
using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Requests.CheckAppointmentByClaim;

public class CheckAppointmentByClaimRequest : IRequest<CheckAppointmentByClaimResponse>
{
    [JsonPropertyName("claim_id")]
    public string ClaimId { get; set; } = string.Empty;
}

public class
    CheckAppointmentByClaimRequestHandler : IRequestHandler<CheckAppointmentByClaimRequest,
        CheckAppointmentByClaimResponse>
{
    private readonly HospitalDbContext _dbContext;

    public CheckAppointmentByClaimRequestHandler(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CheckAppointmentByClaimResponse> Handle(CheckAppointmentByClaimRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.ClaimId, out var guid))
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
}
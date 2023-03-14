namespace ProdoctorovIntegration.Application.Requests.CheckAppointmentByClaim;

public class CheckAppointmentByClaimResponse
{
    public int StatusCode { get; set; }
    public string ClaimStatus { get; set; } = string.Empty;
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
}
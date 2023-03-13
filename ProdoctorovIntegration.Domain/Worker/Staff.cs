namespace ProdoctorovIntegration.Domain.Worker;

public class Staff
{
    public Guid Id { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Speciality { get; set; } = string.Empty;
}
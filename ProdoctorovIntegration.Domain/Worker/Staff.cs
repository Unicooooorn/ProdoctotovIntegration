namespace ProdoctorovIntegration.Domain.Worker;

public class Staff
{
    public Department Department { get; set; } = new();
    public Speciality Speciality { get; set; } = new();
}
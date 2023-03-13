namespace ProdoctorovIntegration.Domain.Worker;

public class Worker
{
    public Guid Id { get; set; } = Guid.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string PatrName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Staff Staff { get; set; } = new();
}
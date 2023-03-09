namespace ProdoctorovIntegration.Domain.Worker;

public class Worker
{
    public string FirstName { get; set; } = string.Empty;
    public string PatrName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public Staff Staff { get; set; } = new();
}
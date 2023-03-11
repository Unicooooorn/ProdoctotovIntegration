namespace ProdoctorovIntegration.Domain.Client;

public class Client
{
    public long Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string PatrName { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; } 
}
namespace ProdoctorovIntegration.Domain.Client;

public class ClientContact
{
    public Guid Id { get; set; }
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    public long ContactOnlyDigits { get; set; }
}
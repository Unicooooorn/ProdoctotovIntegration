namespace ProdoctorovIntegration.Domain.Client;

public class ClientContact
{
    public Client Client { get; set; } = new();
    public long ContactInfoTypeId { get; set; }
    public long ContactOnlyDigits { get; set; }
}
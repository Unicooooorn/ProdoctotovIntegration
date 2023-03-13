namespace ProdoctorovIntegration.Domain.Client;

public class ClientContact
{
    public Guid Id { get; set; }
    public Client Client { get; set; } = new();
    public ContactTypeInfo ContactInfoType { get; set; } = new();
    public long ContactOnlyDigits { get; set; }
}
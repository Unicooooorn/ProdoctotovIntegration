using Bogus;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;

public sealed class ClientContactFaker : Faker<ClientContact>
{
    public ClientContactFaker(Client? client = null, ContactTypeInfo? contactTypeInfo = null, long? phoneNumber = null)
    {
        CustomInstantiator(f =>
            new ClientContact
            {
                Id = Guid.NewGuid(),
                Client = client ?? new ClientFaker(),
                ContactInfoType = contactTypeInfo ?? new ContactTypeInfoFaker(),
                ContactOnlyDigits = phoneNumber ?? f.Random.Long(9000000000, 9999999999)
            });

    }
}
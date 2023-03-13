using Bogus;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;

public sealed class ContactTypeInfoFaker : Faker<ContactTypeInfo>
{
    public ContactTypeInfoFaker()
    {
        CustomInstantiator(
            f => new ContactTypeInfo
            {
                Id = Guid.NewGuid(),
                Code = f.Random.Int(0, 3),
                Name = f.Random.String2(10)
            });
    }
}
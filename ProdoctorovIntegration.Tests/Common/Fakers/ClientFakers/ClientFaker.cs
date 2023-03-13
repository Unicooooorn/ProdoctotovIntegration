using Bogus;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;

public sealed class ClientFaker : Faker<Client>
{
    public ClientFaker()
    {
        CustomInstantiator(f =>
            new Client
            {
                Id = Guid.NewGuid(),
                FirstName = f.Name.FirstName(),
                PatrName = f.Random.String2(10),
                LastName = f.Name.LastName(),
                BirthDay = f.Date.Between(DateTime.UtcNow.AddYears(-50), DateTime.UtcNow.AddYears(-10))
            });
    }
}
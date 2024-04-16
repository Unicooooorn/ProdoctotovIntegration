using Bogus;
using ProdoctorovIntegration.Application.Models.Common;

namespace ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;

public sealed class ClientDtoFaker : Faker<ClientDto>
{
    public ClientDtoFaker()
    {
        CustomInstantiator(f => new ClientDto
        {
            FirstName = f.Name.FirstName(),
            SecondName = f.Random.String2(10),
            LastName = f.Name.LastName(),
            Birthday = f.Date.Between(DateTime.UtcNow.AddYears(-30), DateTime.UtcNow.AddYears(-10)),
            MobilePhone = f.Phone.Random.Long(1000000000, 9999999999).ToString()
        });
    }
}
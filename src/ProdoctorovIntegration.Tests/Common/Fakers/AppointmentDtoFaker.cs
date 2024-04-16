using Bogus;
using ProdoctorovIntegration.Application.Models.Common;

namespace ProdoctorovIntegration.Tests.Common.Fakers;

public sealed class AppointmentDtoFaker : Faker<AppointmentDto>
{
    public AppointmentDtoFaker()
    {
        CustomInstantiator(f => new AppointmentDto
        {
            Comment = f.Random.String2(10),
            DateStart = DateTime.UtcNow.AddMinutes(-20),
            DateEnd = DateTime.UtcNow.AddMinutes(-10)
        });
    }
}
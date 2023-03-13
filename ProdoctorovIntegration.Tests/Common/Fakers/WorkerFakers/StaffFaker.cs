using Bogus;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;

public sealed class StaffFaker : Faker<Staff>
{
    public StaffFaker()
    {
        CustomInstantiator(f =>
            new Staff
            {
                Id = Guid.NewGuid(),
                Department = f.Company.CompanyName(),
                Speciality = f.Random.String2(10)
            });
    }
}
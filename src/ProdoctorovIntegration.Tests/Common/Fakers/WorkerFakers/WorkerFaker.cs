using Bogus;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;

public sealed class WorkerFaker : Faker<Worker>
{
    public WorkerFaker()
    {
        CustomInstantiator(f =>
            new Worker
            {
                Id = Guid.NewGuid(),
                FirstName = f.Person.FirstName,
                PatrName = f.Person.Random.String2(10),
                LastName = f.Person.LastName
            });
    }
}
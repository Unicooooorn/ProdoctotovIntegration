using Bogus;
using ProdoctorovIntegration.Application.Models.Common;

namespace ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;

public sealed class WorkerDtoFaker : Faker<WorkerDto>
{
    public WorkerDtoFaker()
    {
        CustomInstantiator(f => new WorkerDto
        {
            Id = Guid.NewGuid().ToString(),
            LpuId = Guid.NewGuid().ToString(),
            Specialty = new SpecialtyDto
            {
                Id = f.Random.Int(0, 10),
                Name = f.Name.JobArea()
            }
        });
    }
}
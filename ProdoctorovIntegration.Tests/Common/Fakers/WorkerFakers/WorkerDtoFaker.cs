using Bogus;
using ProdoctorovIntegration.Application.Common;

namespace ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;

public sealed class WorkerDtoFaker : Faker<WorkerDto>
{
    public WorkerDtoFaker()
    {
        CustomInstantiator(f => new WorkerDto
        {
            Id = Guid.NewGuid(),
            LpuId = Guid.NewGuid(),
            Speciality = new SpecialityDto
            {
                Id = f.Random.Int(0, 10),
                Name = f.Name.JobArea()
            }
        });
    }
}
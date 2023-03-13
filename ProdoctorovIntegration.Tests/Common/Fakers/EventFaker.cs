using Bogus;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Domain.Worker;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;

namespace ProdoctorovIntegration.Tests.Common.Fakers;

public sealed class EventFaker : Faker<Event>
{
    public EventFaker(Client? client = null, bool? isForProdoctorov = null, Worker? worker = null)
    {
        CustomInstantiator(f =>
            new Event
            {
                Id = Guid.NewGuid(),
                Client = client,
                IsForProdoctorov = isForProdoctorov ?? true,
                RoomId = f.Random.Int(0, 10),
                Worker = worker ?? new WorkerFaker(),
                StartDate = f.Date.Between(DateTime.UtcNow.AddMinutes(-60), DateTime.UtcNow),
                Duration = 10
            });
    }
}
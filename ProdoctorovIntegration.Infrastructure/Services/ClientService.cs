using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Services;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Domain.Exception;

namespace ProdoctorovIntegration.Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly HospitalDbContext _dbContext;

    public ClientService(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> FindClientAsync(ClientDto client, CancellationToken cancellationToken = default)
    {
        var clients = await _dbContext.Client.Where(x => x.FirstName.Equals(
                                                             client.FirstName)
                                                         && x.PatrName.Equals(client.SecondName)
                                                         && x.LastName.Equals(client.LastName))
            .ToListAsync(cancellationToken);

        return clients.Count switch
        {
            0 => await CreateClientAsync(client, cancellationToken),
            1 => clients.First().Id,
            > 1 => await GetClientByPhoneAsync(client, clients, cancellationToken),
            _ => throw new ClientNotFoundOrCreatedException("Don't find or create client")
        };
    }

    private async Task<Guid> GetClientByPhoneAsync(ClientDto client, IEnumerable<Client> clients, CancellationToken cancellationToken = default)
    {
        var clientsGuid = clients.Select(x => x.Id);
        if (!long.TryParse(client.MobilePhone, out var digits))
            throw new ArgumentException("Phone number incorrect");
        var clientContact = await _dbContext.ClientContact
            .Where(x => clientsGuid.Contains(x.Client != null ? x.Client.Id : Guid.Empty) && x.ContactOnlyDigits.Equals(digits))
            .ToListAsync(cancellationToken);

        return clientContact.Count == 1
            ? clientContact.First().Client!.Id
            : await CreateClientAsync(client, cancellationToken);
    }

    private async Task<Guid> CreateClientAsync(ClientDto client, CancellationToken cancellationToken = default)
    {
        var newClient = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = client.FirstName,
            PatrName = client.SecondName,
            LastName = client.LastName,
        };

        await _dbContext.AddAsync(newClient, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newClient.Id;
    }
}
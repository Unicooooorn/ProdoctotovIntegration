using ProdoctorovIntegration.Application.Models.Common;

namespace ProdoctorovIntegration.Application.Interfaces;

public interface IClientService
{
    Task<Guid> FindClientAsync(ClientDto client, CancellationToken cancellationToken = default);
}
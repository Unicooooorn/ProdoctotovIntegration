using ProdoctorovIntegration.Application.Common;

namespace ProdoctorovIntegration.Application.Services;

public interface IClientService
{
    Task<Guid> FindClientAsync(ClientDto client, CancellationToken cancellationToken = default);
}
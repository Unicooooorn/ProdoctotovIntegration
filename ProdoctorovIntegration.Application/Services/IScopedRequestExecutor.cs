using MediatR;

namespace ProdoctorovIntegration.Application.Services;

public interface IScopedRequestExecutor
{
    Task<TResponse> Execute<TResponse>(IRequest<TResponse> request);
}
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProdoctorovIntegration.Application.Services;

namespace ProdoctorovIntegration.Infrastructure.Services;

public class ScopedRequestExecutor : IScopedRequestExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public ScopedRequestExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Execute<TResponse>(IRequest<TResponse> request)
    {
        using var innerScope = _serviceProvider.CreateScope();
        var mediator = innerScope.ServiceProvider.GetService<IMediator>()!;

        return await mediator.Send(request);
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Command.CreateClient;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Requests.GetClientByPhoneOrCreate;
using ProdoctorovIntegration.Domain.Exception;

namespace ProdoctorovIntegration.Application.Requests.FindClient;

public class FindOrCreateClientRequest : IRequest<Guid>
{
    public ClientDto Client { get; set; } = new();
}

public class FindOrCreateClientRequestHandler : IRequestHandler<FindOrCreateClientRequest, Guid>
{
    private readonly HospitalDbContext _dbContext;
    private readonly IMediator _mediator;

    public FindOrCreateClientRequestHandler(HospitalDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(FindOrCreateClientRequest request, CancellationToken cancellationToken)
    {
        var clients = await _dbContext.Client.Where(x => x.FirstName.Equals(
            request.Client.FirstName)
            && x.PatrName.Equals(request.Client.SecondName)
            && x.LastName.Equals(request.Client.LastName))
            .ToListAsync(cancellationToken);

        return clients.Count switch
        {
            0 => await _mediator.Send(new CreateClientCommand(request.Client), cancellationToken),
            1 => clients.First().Id,
            > 1 => await _mediator.Send(new GetClientByPhoneOrCreateRequest(request.Client, clients), cancellationToken),
            _ => throw new ClientNotFoundOrCreatedException("Don't find or create client")
        };
    }
}
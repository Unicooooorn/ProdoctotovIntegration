using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Command.CreateClient;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Application.Requests.GetClientByPhoneOrCreate;

public class GetClientByPhoneOrCreateRequest : IRequest<Guid>
{
    public GetClientByPhoneOrCreateRequest(ClientDto client, List<Client> clients)
    {
        Client = client;
        Clients = clients;
    }
    public ClientDto Client { get; set; } 
    public IList<Client> Clients { get; set; }
}

public class GetClientByPhoneOrCreateRequestHandler : IRequestHandler<GetClientByPhoneOrCreateRequest, Guid>
{
    private readonly HospitalDbContext _dbContext;
    private readonly IMediator _mediator;
    public GetClientByPhoneOrCreateRequestHandler(HospitalDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(GetClientByPhoneOrCreateRequest request, CancellationToken cancellationToken)
    {
        var clientsGuid = request.Clients.Select(x => x.Id);
        var clientContact = await _dbContext.ClientContact
            .Where(x => clientsGuid.Contains(x.Client != null ? x.Client.Id : Guid.Empty) && x.ContactOnlyDigits.Equals(request.Client.MobilePhone))
            .ToListAsync(cancellationToken);

        return clientContact.Count == 1
            ? clientContact.First().Client!.Id
            : await _mediator.Send(new CreateClientCommand(request.Client), cancellationToken);
    }
}
using MediatR;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Application.Command.CreateClient;

public class CreateClientCommand : IRequest<Guid>
{
    public CreateClientCommand(ClientDto client)
    {
        Client = client;
    }
    public ClientDto Client { get; set; }
}

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
{
    private readonly HospitalDbContext _dbContext;

    public CreateClientCommandHandler(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var newClient = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = request.Client.FirstName,
            PatrName = request.Client.SecondName,
            LastName = request.Client.LastName,
        };

        await _dbContext.AddAsync(newClient, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newClient.Id;
    }
}
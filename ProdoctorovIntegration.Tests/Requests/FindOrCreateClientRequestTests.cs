using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using ProdoctorovIntegration.Application.Command.CreateClient;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.Requests.FindClient;
using ProdoctorovIntegration.Application.Requests.GetClientByPhoneOrCreate;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Requests;

public class FindOrCreateClientCommandTests : BaseHospitalTestWithDb
{
    private readonly Mock<IMediator> _mediator;
    public FindOrCreateClientCommandTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _mediator = new Mock<IMediator>();
    }

    private FindOrCreateClientRequestHandler Sut()
    {
        return new FindOrCreateClientRequestHandler(HospitalContext, _mediator.Object);
    }

    [Fact]
    public async Task ReturnGuid_WhenClientsNotFound()
    {
        //Arrange
        var request = CreateRequestForClientsNotFound();
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should<Guid>();
            result.Should().NotBe(Guid.Empty);
        }
    }

    [Fact]
    public async Task ReturnGuid_WhenClientFound()
    {
        //Arrange
        var request = await CreateRequestForClientFound();
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        result.Should().Be(request.Client.Id);
    }

    [Fact]
    public async Task ReturnGuid_WhenClientFoundByPhone()
    {
        //Arrange
        var request = await CreateRequestForClientByPhone();
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        result.Should().NotBe(Guid.Empty);
    }

    private FindOrCreateClientRequest CreateRequestForClientsNotFound()
    {
        _mediator.Setup(x => x.Send(It.IsAny<CreateClientCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid);
        var client = new ClientDtoFaker().Generate();
        return new FindOrCreateClientRequest {Client = client};
    }

    private async Task<FindOrCreateClientRequest> CreateRequestForClientFound()
    {
        var clients = new ClientFaker().Generate(2);
        await HospitalContext.AddRangeAsync(clients);
        await HospitalContext.SaveChangesAsync();

        var clientDto = new ClientDto
        {
            Id = clients[0].Id,
            FirstName = clients[0].FirstName,
            LastName = clients[0].LastName,
            SecondName = clients[0].PatrName
        };

        return new FindOrCreateClientRequest {Client = clientDto};
    }

    private async Task<FindOrCreateClientRequest> CreateRequestForClientByPhone()
    {
        _mediator.Setup(x => x.Send(It.IsAny<GetClientByPhoneOrCreateRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid);

        var client = new ClientDtoFaker().Generate();

        var clients = new ClientFaker()
            .RuleFor(x => x.FirstName, client.FirstName)
            .RuleFor(x => x.PatrName, client.SecondName)
            .RuleFor(x => x.LastName, client.LastName)
            .Generate(2);
        await HospitalContext.AddRangeAsync(clients);
        await HospitalContext.SaveChangesAsync();
        
        return new FindOrCreateClientRequest {Client = client};
    }
}
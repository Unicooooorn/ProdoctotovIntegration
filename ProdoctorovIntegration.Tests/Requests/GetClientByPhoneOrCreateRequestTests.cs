using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using ProdoctorovIntegration.Application.Command.CreateClient;
using ProdoctorovIntegration.Application.Requests.GetClientByPhoneOrCreate;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Requests;

public class GetClientByPhoneOrCreateRequestTests : BaseHospitalTestWithDb
{
    private readonly Mock<IMediator> _mediator;
    public GetClientByPhoneOrCreateRequestTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _mediator = new Mock<IMediator>();
    }

    private GetClientByPhoneOrCreateRequestHandler Sut()
    {
        return new GetClientByPhoneOrCreateRequestHandler(HospitalContext, _mediator.Object);
    }


    [Fact]
    public async Task ReturnClientGuid_WhenFoundByPhone()
    {
        //Arrange
        var request = await CreateRequestForFound();
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should<Guid>();
            result.Should().Be(request.Clients[0].Id);
        }
    }

    [Fact]
    public async Task ReturnNewClientGuid_WhenClientNotFound()
    {
        //Arrange
        InitializeSetups();
        var request = await CreateRequestForNotFound();
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should<Guid>();
            result.Should().NotBe(Guid.Empty);
            result.Should().NotBe(request.Clients[0].Id);
        }
    }

    private async Task<GetClientByPhoneOrCreateRequest> CreateRequestForFound()
    {
        var client = new ClientFaker().Generate();
        var clientDto = new ClientDtoFaker().Generate();
        var clientContact = new ClientContactFaker(client, clientDto.MobilePhone).Generate();

        await HospitalContext.AddAsync(client);
        await HospitalContext.AddAsync(clientContact);
        await HospitalContext.SaveChangesAsync();

        return new GetClientByPhoneOrCreateRequest(clientDto, new List<Client>{client});
    }

    private async Task<GetClientByPhoneOrCreateRequest> CreateRequestForNotFound()
    {
        var client = new ClientFaker().Generate();
        var clientDto = new ClientDtoFaker().Generate();
        var clientContact = new ClientContactFaker().Generate();

        await HospitalContext.AddAsync(client);
        await HospitalContext.AddAsync(clientContact);
        await HospitalContext.SaveChangesAsync();

        return new GetClientByPhoneOrCreateRequest(clientDto, new List<Client> {client});
    }

    private void InitializeSetups()
    {
        _mediator.Setup(x => x.Send(It.IsAny<CreateClientCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid);
    }
}
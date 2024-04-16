using FluentAssertions;
using FluentAssertions.Execution;
using ProdoctorovIntegration.Application.Models.Common;
using ProdoctorovIntegration.Infrastructure.Services;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Services;

public class ClientServiceTests : BaseHospitalTestWithDb
{
    public ClientServiceTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private ClientService Sut()
    {
        return new ClientService(HospitalContext);
    }

    [Fact]
    public async Task ReturnGuid_WhenClientsNotFound()
    {
        //Arrange
        var client = new ClientDtoFaker().Generate();
        //Act
        var result = await Sut().FindClientAsync(client, CancellationToken.None);
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
        //Act
        var result = await Sut().FindClientAsync(clientDto, CancellationToken.None);
        //Assert
        result.Should().Be(clientDto.Id);
    }

    [Fact]
    public async Task ReturnGuid_WhenClientFoundByPhone()
    {
        //Arrange
        var client = new ClientDtoFaker().Generate();

        var clients = new ClientFaker()
            .RuleFor(x => x.FirstName, client.FirstName)
            .RuleFor(x => x.PatrName, client.SecondName)
            .RuleFor(x => x.LastName, client.LastName)
            .Generate(2);
        await HospitalContext.AddRangeAsync(clients);
        await HospitalContext.SaveChangesAsync();
        //Act
        var result = await Sut().FindClientAsync(client, CancellationToken.None);
        //Assert
        result.Should().NotBe(Guid.Empty);
    }
}
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Command.CreateClient;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Commands;

public class CreateClientCommandTests : BaseHospitalTestWithDb
{
    public CreateClientCommandTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private CreateClientCommandHandler Sut()
    {
        return new CreateClientCommandHandler(HospitalContext);
    }

    [Fact]
    public async Task ReturnGuid_WhenClientCreated()
    {
        //Arrange
        var client = new ClientDtoFaker().Generate();
        //Act
        var result = await Sut().Handle(new CreateClientCommand(client), CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should<Guid>();
            await HospitalContext.Client.AnyAsync();
        }
    }
}
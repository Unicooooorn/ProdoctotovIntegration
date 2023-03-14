using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.Command.CancelAppointment;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Commands;

public class CancelAppointmentCommandTests : BaseHospitalTestWithDb
{
    public CancelAppointmentCommandTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private CancelAppointmentCommandHandler Sut()
    {
        return new CancelAppointmentCommandHandler(HospitalContext);
    }

    [Fact]
    public async Task ReturnSuccess_WhenAppointmentCancelled()
    {
        //Arrange
        var cell = new EventFaker()
            .RuleFor(x => x.Client, new ClientFaker().Generate())
            .RuleFor(x => x.ClaimId, Guid.NewGuid())
            .Generate();

        await HospitalContext.AddAsync(cell);
        await HospitalContext.SaveChangesAsync();
        var command = new CancelAppointmentCommand(cell.ClaimId.ToString()!);
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<CancelAppointmentResponse>();
            result.StatusCode.Should().Be(204);
        }
    }
}
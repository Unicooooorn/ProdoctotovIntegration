using FluentAssertions;
using FluentAssertions.Execution;
using ProdoctorovIntegration.Application.Requests.CheckAppointmentByClaim;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Requests;

public class CheckAppointmentByClaimRequestTests : BaseHospitalTestWithDb
{
    private const string Successfully = "successfully";
    private const string Cancelled = "cancelled";

    public CheckAppointmentByClaimRequestTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private CheckAppointmentByClaimRequestHandler Sut()
    {
        return new CheckAppointmentByClaimRequestHandler(HospitalContext);
    }

    [Fact]
    public async Task ReturnCancelled_WhenClientIsNull()
    {
        //Arrange
        var cell = new EventFaker()
            .RuleFor(x => x.ClaimId, Guid.NewGuid())
            .Generate();
        await HospitalContext.AddAsync(cell);
        await HospitalContext.SaveChangesAsync();
        var request = new CheckAppointmentByClaimRequest {ClaimId = cell.ClaimId.ToString()!};
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(204);
            result.ClaimStatus.Should().Be(Cancelled);
        }
    }

    [Fact]
    public async Task ReturnSuccessfully_WhenClientNotNull()
    {
        //Arrange
        var client = new ClientFaker().Generate();
        var cell = new EventFaker()
            .RuleFor(x => x.ClaimId, Guid.NewGuid())
            .RuleFor(x => x.Client, client)
            .Generate();
        await HospitalContext.AddAsync(client);
        await HospitalContext.AddAsync(cell);
        await HospitalContext.SaveChangesAsync();
        var request = new CheckAppointmentByClaimRequest {ClaimId = cell.ClaimId.ToString()!};
        //Act
        var result = await Sut().Handle(request, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(204);
            result.ClaimStatus.Should().Be(Successfully);
        }
    }
}
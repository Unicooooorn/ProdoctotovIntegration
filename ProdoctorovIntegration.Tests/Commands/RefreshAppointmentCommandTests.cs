using FluentAssertions;
using MediatR;
using Moq;
using ProdoctorovIntegration.Application.Command.RefreshAppointment;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Commands;

public class RefreshAppointmentCommandTests : BaseHospitalTestWithDb
{
    private readonly Mock<IMediator> _mediator;
    public RefreshAppointmentCommandTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _mediator = new Mock<IMediator>();
    }

    private RefreshAppointmentCommandHandler Sut()
    {
        return new RefreshAppointmentCommandHandler(HospitalContext, _mediator.Object);
    }

    [Fact]
    public async Task ReturnSuccessCode_WhenAppointmentFound()
    {
        //Arrange
        var claimId = Guid.NewGuid();
        var appointment = new EventFaker()
            .RuleFor(x => x.ClaimId, claimId)
            .Generate();
        await HospitalContext.AddAsync(appointment);
        await HospitalContext.SaveChangesAsync();
        var command = new RefreshAppointmentCommand {ClaimId = claimId.ToString()};
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        result.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task ReturnErrorCode_WhenAppointmentNotFound()
    {
        //Arrange
        var command = new RefreshAppointmentCommand {ClaimId = Guid.NewGuid().ToString()};
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        result.StatusCode.Should().Be(404);
    }
}
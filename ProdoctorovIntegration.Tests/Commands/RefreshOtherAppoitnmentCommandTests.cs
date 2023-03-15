using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProdoctorovIntegration.Application.Command.RefreshAppointment.RefreshInOtherAppointment;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.Requests.FindClient;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Commands;

public class RefreshOtherAppoitnmentCommandTests : BaseHospitalTestWithDb
{
    private readonly Mock<IMediator> _mediator;

    public RefreshOtherAppoitnmentCommandTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _mediator = new Mock<IMediator>();
    }

    private RefreshInOtherAppointmentCommandHandler Sut()
    {
        return new RefreshInOtherAppointmentCommandHandler(HospitalContext, _mediator.Object);
    }

    [Fact]
    public async Task ReturnErrorStatusCode_WhenAppointmentNotFound()
    {
        //Arrange
        var command = new RefreshInOtherAppointmentCommand {ClaimId = Guid.NewGuid()};
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ReturnSuccessCode_WhenAppointmentClientRefresh()
    {
        //Arrange
        var claimId = Guid.NewGuid();
        var client = new ClientFaker().Generate();
        var otherClient = new ClientFaker().Generate();
        var appointment = new EventFaker(client)
            .RuleFor(x => x.ClaimId, claimId)
            .Generate();
        await HospitalContext.AddRangeAsync(client, otherClient, appointment);
        await HospitalContext.SaveChangesAsync();

        _mediator.Setup(x => x.Send(It.IsAny<FindOrCreateClientRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(otherClient.Id);

        var clientDto = new ClientDtoFaker()
            .RuleFor(x => x.Id, otherClient.Id)
            .Generate();
        var command = new RefreshInOtherAppointmentCommand {ClaimId = claimId, Client = clientDto};
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(204);
            var newAppointment = await HospitalContext.Event.FirstAsync(x => x.Id == appointment.Id);
            newAppointment.Should().NotBeNull();
            newAppointment.Client.Should().NotBeNull();
            newAppointment.Client!.Id.Should().Be(otherClient.Id);
        }
    }

    [Fact]
    public async Task ReturnSuccessCode_WhenAppointmentWorkerRefresh()
    {
        var claimId = Guid.NewGuid();
        var worker = new WorkerFaker().Generate();
        var otherWorker = new WorkerFaker().Generate();
        var appointment = new EventFaker()
            .RuleFor(x => x.ClaimId, claimId)
            .RuleFor(x => x.Worker, worker)
            .Generate();
        await HospitalContext.AddRangeAsync(worker, otherWorker, appointment);
        await HospitalContext.SaveChangesAsync();
        var workerDto = new WorkerDtoFaker()
            .RuleFor(x => x.Id, otherWorker.Id)
            .Generate();
        var command = new RefreshInOtherAppointmentCommand {ClaimId = claimId, Worker = workerDto};
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(204);
            var newAppointment = await HospitalContext.Event.FirstAsync(x => x.Id == appointment.Id);
            newAppointment.Should().NotBeNull();
            newAppointment.Worker.Should().NotBeNull();
            newAppointment.Worker!.Id.Should().Be(otherWorker.Id);
        }
    }

    [Fact]
    public async Task ReturnSuccessCode_WhenAppointmentRefresh()
    {
        var claimId = Guid.NewGuid();
        var appointment = new EventFaker()
            .RuleFor(x => x.Client, () => new ClientFaker().Generate())
            .RuleFor(x => x.ClaimId, claimId)
            .Generate();
        var clientId = appointment.Client?.Id;
        var worker = new WorkerFaker().Generate();
        var otherAppointment = new EventFaker()
            .RuleFor(x => x.Worker, worker).Generate();
        await HospitalContext.AddRangeAsync(appointment, otherAppointment);
        await HospitalContext.SaveChangesAsync();
        var appointmentDto = new AppointmentDtoFaker()
            .RuleFor(x => x.DateStart, otherAppointment.StartDate)
            .RuleFor(x => x.DateEnd, otherAppointment.StartDate.AddMinutes(otherAppointment.Duration))
            .Generate();
        var workerDto = new WorkerDtoFaker()
            .RuleFor(x => x.Id, worker.Id)
            .Generate();
        var command = new RefreshInOtherAppointmentCommand
            {ClaimId = claimId, Worker = workerDto, Appointment = appointmentDto};
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(204);
            var oldAppointment = await HospitalContext.Event.FirstAsync(x => x.Id == appointment.Id);
            oldAppointment.Client.Should().BeNull();
            oldAppointment.ClaimId.Should().BeNull();
            var newAppointment = await HospitalContext.Event.FirstAsync(x => x.Id == otherAppointment.Id);
            newAppointment.Client!.Id.Should().Be((Guid) clientId!);
            newAppointment.ClaimId.Should().Be(claimId);
        }
    }
}
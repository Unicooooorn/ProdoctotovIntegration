using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using ProdoctorovIntegration.Application.Command.RecordClient;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.Requests.FindClient;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Commands;

public class RecordClientTests : BaseHospitalTestWithDb
{
    private const string AppointmentSource = "AppointmentSource";

    private readonly Mock<IMediator> _mediator;

    public RecordClientTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _mediator = new Mock<IMediator>();
    }

    private RecordClientCommandHandler Sut()
    {
        return new RecordClientCommandHandler(HospitalContext, _mediator.Object);
    }

    [Fact]
    public async Task ReturnErrorCode_WhenClientHasAppointmentWithAnotherWorkerAtThisTime()
    {
        //Arrange
        var worker = new WorkerFaker().Generate();
        var workerDto = new WorkerDtoFaker().Generate();
        var client = new ClientFaker().Generate();
        var clientDto = new ClientDtoFaker()
            .RuleFor(x => x.Id, client.Id)
            .Generate();
        var cell = new EventFaker(client, true, worker).Generate();
        var appointment = new AppointmentDtoFaker()
            .RuleFor(x => x.DateStart, cell.StartDate)
            .RuleFor(x => x.DateEnd, cell.StartDate.AddMinutes(cell.Duration))
            .Generate();

        await HospitalContext.AddAsync(cell);
        await HospitalContext.SaveChangesAsync();

        InitializeSetup(client.Id);

        var command = new RecordClientCommand
        {
            Appointment = appointment,
            Client = clientDto,
            Worker = workerDto,
            AppointmentSource = AppointmentSource
        };

        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<RecordClientResponse>();
            result.StatusCode.Should().Be(409);
        }
    }

    [Fact]
    public async Task ReturnErrorCode_WhenSlotNotExist()
    {
        //Arrange
        var client = new ClientFaker().Generate();
        InitializeSetup(client.Id);

        var command = new RecordClientCommand
        {
            Appointment = new AppointmentDtoFaker().Generate(),
            Worker = new WorkerDtoFaker().Generate(),
            Client = new ClientDtoFaker()
                .RuleFor(x => x.Id, client.Id)
                .Generate(),
            AppointmentSource = AppointmentSource
        };
        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<RecordClientResponse>();
            result.StatusCode.Should().Be(416);
        }
    }

    [Fact]
    public async Task ReturnErrorCode_WhenSlotIsBusy()
    {
        //Arrange
        var worker = new WorkerFaker().Generate();
        var client = new ClientFaker().Generate();
        var clientDto = new ClientDtoFaker()
            .RuleFor(x => x.Id, client.Id)
            .Generate();
        var cell = new EventFaker(worker:worker).Generate();
        cell.Client = new ClientFaker().Generate();

        var appointment = new AppointmentDtoFaker()
            .RuleFor(x => x.DateStart, cell.StartDate)
            .RuleFor(x => x.DateEnd, cell.StartDate.AddMinutes(cell.Duration))
            .Generate();

        await HospitalContext.AddAsync(cell);
        await HospitalContext.SaveChangesAsync();

        InitializeSetup(client.Id);

        var workerDto = new WorkerDtoFaker()
            .RuleFor(x => x.Id, worker.Id.ToString)
            .Generate();

        var command = new RecordClientCommand
        {
            Appointment = appointment,
            Client = clientDto,
            Worker = workerDto,
            AppointmentSource = AppointmentSource
        };

        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<RecordClientResponse>();
            result.StatusCode.Should().Be(423);
        }
    }

    [Fact]
    public async Task ReturnClaimId_WhenRecordSuccess()
    {
        //Arrange
        var client = new ClientFaker().Generate();
        
        var worker = new WorkerFaker().Generate();
        var cell = new EventFaker(worker: worker).Generate();

        await HospitalContext.AddAsync(client);
        await HospitalContext.AddAsync(cell);
        await HospitalContext.SaveChangesAsync();

        InitializeSetup(client.Id);
        var clientDto = new ClientDtoFaker()
            .RuleFor(x => x.Id, client.Id).Generate();
        var appointment = new AppointmentDtoFaker()
            .RuleFor(x => x.DateStart, cell.StartDate)
            .RuleFor(x => x.DateEnd, cell.StartDate.AddMinutes(cell.Duration))
            .Generate();
        var workerDto = new WorkerDtoFaker()
            .RuleFor(x => x.Id, worker.Id.ToString)
            .Generate();

        var command = new RecordClientCommand
        {
            Appointment = appointment,
            Client = clientDto,
            Worker = workerDto
        };

        //Act
        var result = await Sut().Handle(command, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<RecordClientResponse>();
            result.StatusCode.Should().Be(204);
            result.ClaimId.Should().NotBe(string.Empty);
            result.ClaimId.Should().NotBe(Guid.Empty.ToString());
        }
    }

    private void InitializeSetup(Guid guid)
    {
        _mediator.Setup(x => x.Send(It.IsAny<FindOrCreateClientRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(guid);
    }
}
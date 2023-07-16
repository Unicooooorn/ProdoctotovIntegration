using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Options;
using Moq;
using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Application.Response;
using ProdoctorovIntegration.Application.Services;
using ProdoctorovIntegration.Infrastructure.Services;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.ClientFakers;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Services;

public class ScheduleServiceTests : BaseHospitalTestWithDb
{
    private const string Successfully = "successfully";
    private const string Cancelled = "cancelled";
    private const string OrganizationName = "OrgName";
    private const string AppointmentSource = "AppointmentSource";
    private readonly Mock<IClientService> _clientServiceMock;
    private readonly OrganizationNameOptions _organizationNameOptions;

    public ScheduleServiceTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _organizationNameOptions = new OrganizationNameOptions
        {
            Name = OrganizationName
        };

        _clientServiceMock = new Mock<IClientService>();
    }

    private ScheduleService Sut()
    {
        var organizationNameOptionsMock = new Mock<IOptions<OrganizationNameOptions>>();
        organizationNameOptionsMock.Setup(x => x.Value)
            .Returns(_organizationNameOptions);

        return new ScheduleService(_clientServiceMock.Object, HospitalContext, organizationNameOptionsMock.Object);
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenEventsNotFound()
    {
        //Act
        var result = await Sut().GetScheduleAsync(CancellationToken.None);
        //Assert
        result.Schedule.Data.Department.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnData_WhenEventsFound()
    {
        //Arrange
        var worker = new WorkerFaker().Generate();
        var events = new EventFaker(worker: worker).Generate();
        await HospitalContext.AddAsync(worker);
        await HospitalContext.AddAsync(events);
        await HospitalContext.SaveChangesAsync();
        //Act
        var result = await Sut().GetScheduleAsync(CancellationToken.None);
        //Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenSlotsNotFound()
    {
        //Act
        var result = await Sut().GetOccupiedDoctorScheduleSlotAsync(CancellationToken.None);
        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnCollection_WhenSlotsFound()
    {
        //Arrange
        var worker = new WorkerFaker().Generate();
        var events = new EventFaker(worker: worker).Generate();
        await HospitalContext.AddAsync(worker);
        await HospitalContext.AddAsync(events);
        await HospitalContext.SaveChangesAsync();
        //Act
        var result = await Sut().GetOccupiedDoctorScheduleSlotAsync(CancellationToken.None);
        //Assert
        result.Should().NotBeEmpty();
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
        //Act
        var result = await Sut().CheckAppointmentByClaimAsync(cell.ClaimId.ToString()!, CancellationToken.None);
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
        //Act
        var result = await Sut().CheckAppointmentByClaimAsync(cell.ClaimId.ToString()!, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(204);
            result.ClaimStatus.Should().Be(Successfully);
        }
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
        //Act
        var result = await Sut().RefreshAppointmentAsync(claimId.ToString(),null, null, null, CancellationToken.None);
        //Assert
        result.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task ReturnErrorCode_WhenAppointmentNotFound()
    {
        //Arrange
        //Act
        var result = await Sut().RefreshAppointmentAsync(Guid.NewGuid().ToString(), null, null, null, CancellationToken.None);
        //Assert
        result.StatusCode.Should().Be(404);
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

        //Act
        var result = await Sut().RecordClientAsync(workerDto, appointment, clientDto, AppointmentSource, CancellationToken.None);
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

        var workerDto = new WorkerDtoFaker().Generate();
        var appointment = new AppointmentDtoFaker().Generate();
        var clientDto = new ClientDtoFaker().Generate();

        //Act
        var result = await Sut().RecordClientAsync(workerDto, appointment, clientDto, AppointmentSource, CancellationToken.None);
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
        var cell = new EventFaker(worker: worker).Generate();
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

        //Act
        var result = await Sut().RecordClientAsync(workerDto, appointment, clientDto, AppointmentSource, CancellationToken.None);
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

        //Act
        var result = await Sut().RecordClientAsync(workerDto, appointment, clientDto, AppointmentSource, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<RecordClientResponse>();
            result.StatusCode.Should().Be(204);
            result.ClaimId.Should().NotBe(string.Empty);
            result.ClaimId.Should().NotBe(Guid.Empty.ToString());
        }
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

        //Act
        var result = await Sut().CancelAppointmentAsync(cell.ClaimId.ToString()!, CancellationToken.None);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<CancelAppointmentResponse>();
            result.StatusCode.Should().Be(204);
        }
    }

    private void InitializeSetup(Guid clientId)
    {
        _clientServiceMock.Setup(x => x.FindClientAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientId);
    }
}
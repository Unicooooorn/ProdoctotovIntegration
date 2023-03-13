using FluentAssertions;
using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;
using ProdoctorovIntegration.Tests.Common.Fakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Requests;

public class GetOccupiedDoctorSlotsRequestTests : BaseHospitalTestWithDb
{
    public GetOccupiedDoctorSlotsRequestTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private GetOccupiedDoctorScheduleSlotRequestHandler Sut()
    {
        return new GetOccupiedDoctorScheduleSlotRequestHandler(HospitalContext);
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenSlotsNotFound()
    {
        //Act
        var result = await Sut().Handle(new GetOccupiedDoctorScheduleSlotRequest(), CancellationToken.None);
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
        var result = await Sut().Handle(new GetOccupiedDoctorScheduleSlotRequest(), CancellationToken.None);
        //Assert
        result.Should().NotBeEmpty();
    }
}
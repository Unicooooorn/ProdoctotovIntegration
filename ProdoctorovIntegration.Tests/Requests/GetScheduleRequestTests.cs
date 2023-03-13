using FluentAssertions;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Requests;

public class GetScheduleRequestTests : BaseHospitalTestWithDb
{
    public GetScheduleRequestTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private GetScheduleRequestHandler Sut()
    {
        return new GetScheduleRequestHandler(HospitalContext);
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenEventsNotFound()
    {
        //Act
        var result = await Sut().Handle(new GetScheduleRequest(), CancellationToken.None);
        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnCollection_WhenEventsFound()
    {
        //Arrange
        var worker = new WorkerFaker().Generate();
        var events = new EventFaker(worker:worker).Generate();
        await HospitalContext.AddAsync(worker);
        await HospitalContext.AddAsync(events);
        await HospitalContext.SaveChangesAsync();
        //Act
        var result = await Sut().Handle(new GetScheduleRequest(), CancellationToken.None);
        //Assert
        result.Should().NotBeEmpty();
    }
}
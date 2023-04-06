using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using ProdoctorovIntegration.Tests.Common.Fakers.WorkerFakers;
using Xunit;

namespace ProdoctorovIntegration.Tests.Requests;

public class GetScheduleRequestTests : BaseHospitalTestWithDb
{
    private const string OrganizationName = "OrgName";
    private readonly OrganizationNameOptions _organizationNameOptions;
    public GetScheduleRequestTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _organizationNameOptions = new OrganizationNameOptions
        {
            Name = OrganizationName
        };
    }

    private GetScheduleRequestHandler Sut()
    {
        var organizationNameOptions = new Mock<IOptions<OrganizationNameOptions>>();
        organizationNameOptions.Setup(x => x.Value)
            .Returns(_organizationNameOptions);
        return new GetScheduleRequestHandler(HospitalContext, organizationNameOptions.Object);
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
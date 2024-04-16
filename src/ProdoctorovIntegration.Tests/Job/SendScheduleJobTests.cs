using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ProdoctorovIntegration.Application.Interfaces;
using ProdoctorovIntegration.Application.Mapping;
using ProdoctorovIntegration.Application.Response;
using ProdoctorovIntegration.Infrastructure.Jobs;
using ProdoctorovIntegration.Tests.Common.Fakers;
using Quartz;
using Xunit;

namespace ProdoctorovIntegration.Tests.Job;

public class SendScheduleJobTests
{
    private const string OrganizationName = "OrgName";

    private readonly Mock<ISendScheduleService> _sendScheduleService;
    private readonly Mock<IScheduleService> _scheduleServiceMock;

    public SendScheduleJobTests()
    {
        _sendScheduleService = new Mock<ISendScheduleService>();
        _scheduleServiceMock = new Mock<IScheduleService>();
    }

    private SendScheduleJob Sut()
    {
        return new SendScheduleJob(_sendScheduleService.Object, new NullLogger<SendScheduleJob>(),
            _scheduleServiceMock.Object);
    }

    [Fact]
    public async Task SendAllEvents()
    {
        //Arrange
        var cell = new EventFaker().Generate();
        var cells = new[] { cell };
        _scheduleServiceMock.Setup(x => x.GetScheduleAsync(CancellationToken.None))
            .ReturnsAsync(cells.MapToResponse(OrganizationName));
        //Act
        await Sut().Execute(Mock.Of<IJobExecutionContext>());
        //Assert
        _sendScheduleService.Verify(
            x => x.SendScheduleAsync(It.IsAny<GetScheduleResponse>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
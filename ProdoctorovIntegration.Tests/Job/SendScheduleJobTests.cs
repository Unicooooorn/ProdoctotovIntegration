using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ProdoctorovIntegration.Application.Mapping;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Application.Services;
using ProdoctorovIntegration.Infrastructure.Jobs;
using ProdoctorovIntegration.Tests.Common.Fakers;
using Quartz;
using Xunit;

namespace ProdoctorovIntegration.Tests.Job;

public class SendScheduleJobTests
{
    private const string OrganizationName = "OrgName";

    private readonly Mock<ISendScheduleService> _sendScheduleService;
    private readonly Mock<IScopedRequestExecutor> _scopedRequestExecutor;

    public SendScheduleJobTests()
    {
        _sendScheduleService = new Mock<ISendScheduleService>();
        _scopedRequestExecutor = new Mock<IScopedRequestExecutor>();
    }

    private SendScheduleJob Sut()
    {
        return new SendScheduleJob(_sendScheduleService.Object, new NullLogger<SendScheduleJob>(),
            _scopedRequestExecutor.Object);
    }

    [Fact]
    public async Task SendAllEvents()
    {
        //Arrange
        var cell = new EventFaker().Generate();
        var cells = new[] { cell };
        _scopedRequestExecutor.Setup(x => x.Execute(It.IsAny<GetScheduleRequest>()))
            .ReturnsAsync(cells.MapToResponse(OrganizationName).ToList().AsReadOnly);
        //Act
        await Sut().Execute(Mock.Of<IJobExecutionContext>());
        //Assert
        _sendScheduleService.Verify(
            x => x.SendScheduleAsync(It.IsAny<IReadOnlyCollection<GetScheduleResponse>>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
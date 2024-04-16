using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using ProdoctorovIntegration.Application.Mapping;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Infrastructure.Services;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using System.Net;
using Xunit;

namespace ProdoctorovIntegration.Tests.Services;

public class SendScheduleServiceTests : BaseHospitalTestWithDb
{
    private const string OrganizationName = "OrgName";
    private const string OccupiedScheduleUrl = "url1";
    private const string SendScheduleUrl = "url2";
    private const string Token = "token";
    private readonly Mock<IHttpClientFactory> _httpClientFactory;
    private readonly ConnectionOptions _connectionOptions;
    private readonly AuthenticationOptions _authenticationOptions;

    public SendScheduleServiceTests(HospitalDatabaseFixture databaseFixture) : base(databaseFixture)
    {
        _httpClientFactory = SetupHttpClientFactory();

        _connectionOptions = new ConnectionOptions
        {
            OccupiedSchedule = OccupiedScheduleUrl,
            SendSchedule = SendScheduleUrl
        };

        _authenticationOptions = new AuthenticationOptions
        {
            Token = Token
        };
    }

    private SendScheduleService Sut()
    {
        var connectionOptions = new Mock<IOptions<ConnectionOptions>>();
        connectionOptions.Setup(x => x.Value)
            .Returns(_connectionOptions);
        var authenticationOptions = new Mock<IOptions<AuthenticationOptions>>();
        authenticationOptions.Setup(x => x.Value)
            .Returns(_authenticationOptions);

        return new SendScheduleService(_httpClientFactory.Object, connectionOptions.Object,
            authenticationOptions.Object, new NullLogger<SendScheduleService>());
    }

    [Fact]
    public Task SendSchedule()
    {
        //Arrange
        var cell = new EventFaker().Generate();
        var cells = new[] { cell };
        var body = cells.MapToResponse(OrganizationName);
        //Act
        var task = Sut().SendScheduleAsync(body, CancellationToken.None);
        //Assert
        Assert.True(task.IsCompletedSuccessfully);
        return Task.CompletedTask;
    }

    [Fact]
    public Task SendOccupiedSchedule()
    {
        //Arrange
        var cell = new EventFaker().Generate();
        var cells = new []{ cell };
        var body = cells.MapOccupiedSlotsToResponse();
        //Act
        var task = Sut().SendOccupiedSlotsAsync(body, CancellationToken.None);
        //Assert
        Assert.True(task.IsCompletedSuccessfully);
        return Task.CompletedTask;
    }

    private static Mock<IHttpClientFactory> SetupHttpClientFactory()
    {
        var httpClientResponse = new Mock<HttpMessageHandler>();
        httpClientResponse
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient(httpClientResponse.Object));

        return httpClientFactory;
    }
}
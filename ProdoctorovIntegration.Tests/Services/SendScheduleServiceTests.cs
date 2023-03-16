using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Infrastructure.Services;
using ProdoctorovIntegration.Tests.Common;
using ProdoctorovIntegration.Tests.Common.Fakers;
using System.Net;
using ProdoctorovIntegration.Application.Mapping;
using ProdoctorovIntegration.Domain;
using Xunit;

namespace ProdoctorovIntegration.Tests.Services;

public class SendScheduleServiceTests : BaseHospitalTestWithDb
{
    private const string OccupiedScheduleUrl = "url1";
    private const string SendScheduleUrl = "url2";
    private const string Token = "token";
    private readonly Mock<IHttpClientFactory> _httpClientFactory;
    private readonly ConnectionOptions _connectionOptions;
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly Mock<IMediator> _mediator;

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

        _mediator = new Mock<IMediator>();
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
    public async Task SendSchedule()
    {
        //Arrange
        var cell = new EventFaker().Generate();
        var cells = new[] { cell };
        _mediator.Setup(x => x.Send(It.IsAny<GetScheduleRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cells.MapToResponse().ToList().AsReadOnly);
        var body = await _mediator.Object.Send(new GetScheduleRequest());
        //Act
        var result = await Sut().SendScheduleAsync(body, CancellationToken.None);
        //Assert
        result.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task SendOccupiedSchedule()
    {
        //Arrange
        var cell = new EventFaker().Generate();
        var cells = new []{ cell };
        _mediator.Setup(x => x.Send(It.IsAny<GetOccupiedDoctorScheduleSlotRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cells.MapOccupiedSlotsToResponse().ToList().AsReadOnly);
        var body = await _mediator.Object.Send(new GetOccupiedDoctorScheduleSlotRequest());
        //Act
        var result = await Sut().SendOccupiedSlotsAsync(body, CancellationToken.None);
        //Assert
        result.Should().Be(Unit.Value);
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
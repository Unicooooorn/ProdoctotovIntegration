using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Application.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Infrastructure.Services;

public class SendScheduleService : ISendScheduleService
{
    private const string ServiceUrl = "https://api.prodoctorov.ru/v2";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ConnectionOptions _connectionOptions;
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly ILogger<SendScheduleService> _logger;

    public SendScheduleService(IHttpClientFactory httpClientFactory, IOptions<ConnectionOptions> connectionOptionsMonitor, IOptions<AuthenticationOptions> authenticationOptionsMonitor, ILogger<SendScheduleService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _connectionOptions = connectionOptionsMonitor.Value;
        _authenticationOptions = authenticationOptionsMonitor.Value;
    }

    public async Task SendScheduleAsync(IReadOnlyCollection<GetScheduleResponse> events, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _authenticationOptions.Token);

        var jsonSerializeOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        try
        {
            foreach (var cell in events)
            {
                var content = JsonContent.Create(
                    cell,
                    typeof(GetScheduleResponse),
                    new MediaTypeHeaderValue("application/json"),
                    jsonSerializeOptions);

                var uri = $"{ServiceUrl}/{_connectionOptions.SendSchedule}";

                _logger.LogInformation("{Method} request to: {Request}", HttpMethod.Post.Method, uri);

                var response = await client.PostAsync(uri, content, cancellationToken);

                response.EnsureSuccessStatusCode();
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "{Method} failed with {ExMessage}", HttpMethod.Post.Method, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Operation failed");
        }
    }

    public async Task SendOccupiedSlotsAsync(IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse> events, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", _authenticationOptions.Token);

        var jsonSerializeOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        try
        {
            foreach (var cell in events)
            {
                var content = JsonContent.Create(
                    cell,
                    typeof(GetOccupiedDoctorScheduleSlotResponse),
                    new MediaTypeHeaderValue("application/json"),
                    jsonSerializeOptions);

                var uri = $"{ServiceUrl}/{_connectionOptions.OccupiedSchedule}";

                _logger.LogInformation("{Method} request to: {Request}", HttpMethod.Post.Method, uri);

                var response = await client.PostAsync(uri, content, cancellationToken);

                response.EnsureSuccessStatusCode();
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "{Method} failed with {ExMessage}", HttpMethod.Post.Method, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Operation failed");
        }
    }
}
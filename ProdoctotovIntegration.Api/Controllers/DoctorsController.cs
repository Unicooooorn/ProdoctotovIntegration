using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Application.Requests.Schedule;

namespace ProdoctorovIntegration.Api.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.AuthenticationScheme)]
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send_schedule")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetScheduleResponse>), StatusCodes.Status200OK)]
    public async Task<IReadOnlyCollection<GetScheduleResponse>> GetScheduleAsync([FromQuery] GetScheduleRequest request)
    {
        return await _mediator.Send(request);
    }
}
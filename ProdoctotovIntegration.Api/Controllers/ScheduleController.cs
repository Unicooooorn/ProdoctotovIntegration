using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdoctorovIntegration.Application.Command.CancelAppointment;
using ProdoctorovIntegration.Application.Command.RecordClient;
using ProdoctorovIntegration.Application.Options.Authentication;

namespace ProdoctorovIntegration.Api.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.AuthenticationScheme)]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("record_client")]
    [ProducesResponseType(typeof(RecordClientResponse), StatusCodes.Status200OK)]
    public async Task<RecordClientResponse> RecordClient([FromQuery] RecordClientCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost]
    public async Task<CancelAppointmentResponse> CancelAppointment([FromQuery] CancelAppointmentCommand command)
    {
        return await _mediator.Send(command);
    }
}
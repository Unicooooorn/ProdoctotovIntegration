using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdoctorovIntegration.Application.Interfaces;
using ProdoctorovIntegration.Application.Models.Common;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Application.Response;

namespace ProdoctorovIntegration.Api.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.AuthenticationScheme)]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpPost("record_client")]
    [ProducesResponseType(typeof(RecordClientResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecordClientAsync([FromQuery] WorkerDto worker, [FromQuery] AppointmentDto appointment, [FromQuery] ClientDto client, [FromQuery] string appointmentSource)
    {
        var response = await _scheduleService.RecordClientAsync(worker, appointment, client, appointmentSource);
        return Ok(response);
    }

    [HttpPost("cancel_appointment")]
    [ProducesResponseType(typeof(CancelAppointmentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelAppointmentAsync([FromQuery] string claimId)
    {
        var response = await _scheduleService.CancelAppointmentAsync(claimId);
        return Ok(response);
    }

    [HttpPost("check_appointment")]
    [ProducesResponseType(typeof(CheckAppointmentByClaimResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckAppointmentAsync([FromQuery] string claimId)
    {
        var response = await _scheduleService.CheckAppointmentByClaimAsync(claimId);
        return Ok(response);
    }

    [HttpPost("refresh_appointment")]
    [ProducesResponseType(typeof(RefreshAppointmentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshAppointmentAsync([FromQuery] string claimId, WorkerDto worker, AppointmentDto appointment, ClientDto client)
    {
        var response = await _scheduleService.RefreshAppointmentAsync(claimId, worker, appointment, client);
        return Ok(response);
    }
}
﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdoctorovIntegration.Application.Command.CancelAppointment;
using ProdoctorovIntegration.Application.Command.RecordClient;
using ProdoctorovIntegration.Application.Command.RefreshAppointment;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Application.Requests.CheckAppointmentByClaim;

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
    public async Task<RecordClientResponse> RecordClientAsync([FromQuery] RecordClientCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("cancel_appointment")]
    [ProducesResponseType(typeof(CancelAppointmentResponse), StatusCodes.Status200OK)]
    public async Task<CancelAppointmentResponse> CancelAppointmentAsync([FromQuery] CancelAppointmentCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("check_appointment")]
    [ProducesResponseType(typeof(CheckAppointmentByClaimResponse), StatusCodes.Status200OK)]
    public async Task<CheckAppointmentByClaimResponse> CheckAppointmentAsync(
        [FromQuery] CheckAppointmentByClaimRequest request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("refresh_appointment")]
    [ProducesResponseType(typeof(RefreshAppointmentResponse), StatusCodes.Status200OK)]
    public async Task<RefreshAppointmentResponse> RefreshAppointmentAsync([FromQuery] RefreshAppointmentCommand command)
    {
        return await _mediator.Send(command);
    }
}
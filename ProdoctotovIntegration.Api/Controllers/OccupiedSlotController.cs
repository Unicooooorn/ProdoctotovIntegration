using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;

namespace ProdoctorovIntegration.Api.Controllers;


[ApiController]
[Route("v{version:apiVersion}/{action}")]
public class OccupiedSlotController
{
    private readonly IMediator _mediator;

    public OccupiedSlotController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("occupied_doctor_schedule_slot")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>), StatusCodes.Status200OK)]
    public async Task<IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>> GetOccupiedDoctorScheduleSlotsAsync(
        [FromQuery] GetOccupiedDoctorScheduleSlotRequest request)
    {
        return await _mediator.Send(request);
    }
}
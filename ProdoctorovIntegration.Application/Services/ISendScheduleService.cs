using MediatR;
using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;
using ProdoctorovIntegration.Application.Requests.Schedule;

namespace ProdoctorovIntegration.Application.Services;

public interface ISendScheduleService
{
    Task<Unit> SendScheduleAsync(IReadOnlyCollection<GetScheduleResponse> events, CancellationToken cancellationToken);
    Task<Unit> OccupiedSlotsAsync(IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse> events, CancellationToken cancellationToken);
}
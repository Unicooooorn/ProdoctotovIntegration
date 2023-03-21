using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;
using ProdoctorovIntegration.Application.Requests.Schedule;

namespace ProdoctorovIntegration.Application.Services;

public interface ISendScheduleService
{
    Task SendScheduleAsync(IReadOnlyCollection<GetScheduleResponse> events, CancellationToken cancellationToken);
    Task SendOccupiedSlotsAsync(IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse> events, CancellationToken cancellationToken);
}
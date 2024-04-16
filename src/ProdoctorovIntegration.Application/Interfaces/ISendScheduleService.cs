using ProdoctorovIntegration.Application.Response;

namespace ProdoctorovIntegration.Application.Interfaces;

public interface ISendScheduleService
{
    Task SendScheduleAsync(GetScheduleResponse events, CancellationToken cancellationToken = default);
    Task SendOccupiedSlotsAsync(IEnumerable<GetOccupiedDoctorScheduleSlotResponse> events, CancellationToken cancellationToken = default);
}
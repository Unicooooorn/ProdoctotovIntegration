using ProdoctorovIntegration.Application.Common;
using ProdoctorovIntegration.Application.Response;

namespace ProdoctorovIntegration.Application.Services;

public interface IScheduleService
{
    Task<GetScheduleResponse> GetScheduleAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>> GetOccupiedDoctorScheduleSlotAsync(CancellationToken cancellationToken = default);
    Task<CheckAppointmentByClaimResponse> CheckAppointmentByClaimAsync(string claimId, CancellationToken cancellationToken = default);
    Task<RefreshAppointmentResponse> RefreshAppointmentAsync(string claimIdString, WorkerDto? worker, AppointmentDto? appointment, ClientDto? client, CancellationToken cancellationToken = default);
    Task<RecordClientResponse> RecordClientAsync(WorkerDto worker, AppointmentDto appointment, ClientDto client, string appointmentSource, CancellationToken  cancellationToken = default);
    Task<CancelAppointmentResponse> CancelAppointmentAsync(string claimIdString, CancellationToken cancellationToken = default);
}
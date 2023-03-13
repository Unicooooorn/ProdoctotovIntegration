using ProdoctorovIntegration.Application.Requests.Schedule;

namespace ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;

public class GetOccupiedDoctorScheduleSlotResponse
{
    public string FilialId { get; set; } = string.Empty;
    public string DoctorId { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public Cell[] Cells { get; set; } = Array.Empty<Cell>();
}
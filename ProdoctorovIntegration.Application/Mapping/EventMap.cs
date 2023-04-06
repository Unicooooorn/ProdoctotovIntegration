using System.Text;
using ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Application.Mapping;

public static class EventMap
{
    private const char Space = ' ';
    public static IEnumerable<GetScheduleResponse> MapToResponse(this IEnumerable<Event> events, string? organizationName)
    {
        return events.GroupBy(x => x.Worker.Id)
            .ToDictionary(x => x.Key, x => x.ToList())
            .Select(x => new GetScheduleResponse
            {
                Schedule = new Schedule
                {
                    DepartmentName = new Dictionary<string, object>
                    {
                        {x.Value.FirstOrDefault()?.Worker.Staff.Id.ToString() ?? string.Empty, organizationName ?? string.Empty}
                    },
                    Data = new DoctorScheduleData
                    {
                        Department = new Dictionary<string, object>
                        {
                            {x.Value.FirstOrDefault()?.Worker.Staff.Id.ToString() ?? string.Empty, new Department
                            {
                                DoctorInfo = new Dictionary<string, object>
                                {
                                    {x.Key.ToString(), new DoctorInfo
                                    {
                                        Specialty = x.Value.FirstOrDefault()?.Worker.Staff.Speciality ?? string.Empty,
                                        FullName = GetWorkerFullName(x.Value.FirstOrDefault()!.Worker),
                                        Cells = x.Value.Select(c => new Cell
                                            {
                                                Date = c.StartDate.ToString("yyyy-MM-dd"),
                                                TimeStart = c.StartDate.ToString("t"),
                                                TimeEnd = c.StartDate.AddMinutes(c.Duration).ToString("t"),
                                                IsFree = c.Client == null
                                            })
                                            .ToArray()
                                    }}
                                }
                            }}
                        }
                    }
                }
            });
    }

    public static IEnumerable<GetOccupiedDoctorScheduleSlotResponse> MapOccupiedSlotsToResponse(
        this IEnumerable<Event> events)
    {
        return events.GroupBy(x => x.Worker.Id)
            .ToDictionary(x => x.Key, x => x.ToList())
            .Select(x => new GetOccupiedDoctorScheduleSlotResponse()
            {
                FilialId = x.Value.FirstOrDefault()?.Worker.Staff.Id.ToString() ?? string.Empty,
                DoctorId = x.Key.ToString(),
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Cells = x.Value.Select(y => new Cell
                    {
                        TimeStart = y.StartDate.ToString("t"),
                        TimeEnd = y.StartDate.AddMinutes(y.Duration).ToString("t"),
                        IsFree = y.Client == null
                    })
                    .ToArray()
            });
    }

    private static string GetWorkerFullName(Worker worker)
    {
        var sb = new StringBuilder();

        sb.Append(worker.LastName);
        sb.Append(Space);
        sb.Append(worker.FirstName);
        sb.Append(Space);
        sb.Append(worker.PatrName);

        return sb.ToString();
    }
}
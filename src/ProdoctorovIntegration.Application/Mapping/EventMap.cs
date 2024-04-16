using ProdoctorovIntegration.Application.Response;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Worker;
using System.Text;

namespace ProdoctorovIntegration.Application.Mapping;

public static class EventMap
{
    private const char Space = ' ';
    public static GetScheduleResponse MapToResponse(this IList<Event> events, string? organizationName)
    {
        return new GetScheduleResponse
        {
            Schedule = new Schedule
            {
                DepartmentName = new Dictionary<string, object>
                {
                    {
                        events.FirstOrDefault()?.Worker.Staff.Id.ToString() ?? string.Empty,
                        organizationName ?? string.Empty
                    }
                },
                Data = new DoctorScheduleData
                {
                    Department = new Dictionary<string, object>
                    {
                        {
                            events.FirstOrDefault()?.Worker.Staff.Id.ToString() ?? string.Empty,
                            events.GroupBy(x => x.Worker.Id)
                                .ToDictionary(x => x.Key, x => x.ToList())
                                .ToDictionary(x => x.Key, x => new DoctorInfo
                                {
                                    Specialty = x.Value.FirstOrDefault()?.Worker?.Staff?.Speciality ?? string.Empty,
                                    FullName = GetWorkerFullName(x.Value.FirstOrDefault()?.Worker!),
                                    Cells = x.Value.DistinctBy(q => q.StartDate).Select(q => new Cell
                                        {
                                            Date = q.StartDate.ToString("yyyy-MM-dd"),
                                            TimeStart = q.StartDate.ToString("t"),
                                            TimeEnd = q.StartDate.AddMinutes(q.Duration).ToString("t"),
                                            IsFree = q.Client is null
                                        })
                                        .ToArray()
                                })
                        }
                    }
                }
            }
        };
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
                        IsFree = y.ClientId is null
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
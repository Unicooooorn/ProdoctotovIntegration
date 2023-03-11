using System.Text;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Application.Mapping;

public static class EventMap
{
    private const char Space = ' ';
    public static IEnumerable<GetScheduleResponse> MapToResponse(this IEnumerable<Event> events)
    {
        return events.GroupBy(x => x.Worker.Id)
            .ToDictionary(x => x.Key, x => x.ToList())
            .Select(x => new GetScheduleResponse
            {
                Data = new DoctorScheduleData
                {
                    Department = new Department
                    {
                        DoctorInfo = new DoctorInfo
                        {
                            Speciality = x.Value.FirstOrDefault()?.Worker.Staff.Speciality ?? string.Empty,
                            FullName = GetWorkerFullName(x.Value.FirstOrDefault()!.Worker),
                            Cells = x.Value.Select(c => new Cell
                            {
                                Date = c.StartDate.ToString("yyyy-MM-dd"),
                                TimeStart = c.StartDate.ToString("t"),
                                TimeEnd = c.StartDate.AddMinutes(c.Duration).ToString("t"),
                                IsFree = c.Client == null
                            })
                                .ToArray()
                        }
                    }
                }
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
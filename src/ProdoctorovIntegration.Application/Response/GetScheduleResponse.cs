using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Response;

public class GetScheduleResponse
{
    public Schedule Schedule { get; set; } = new();
}

public class Schedule
{
    [JsonExtensionData]
    public Dictionary<string, object> DepartmentName { get; set; } = new();
    public DoctorScheduleData Data { get; set; } = new();
}

public class DoctorScheduleData
{
    [JsonExtensionData]
    public Dictionary<string, object> Department { get; set; } = new();
}

public class Department
{
    [JsonExtensionData]
    public Dictionary<string, object> DoctorInfo { get; set; } = new();
}

public class DoctorInfo
{
    public string FullName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public Cell[] Cells { get; set; } = Array.Empty<Cell>();
}

public class Cell
{
    public string Date { get; set; } = string.Empty;
    public string TimeStart { get; set; } = string.Empty;
    public string TimeEnd { get; set; } = string.Empty;
    public bool IsFree { get; set; }
}
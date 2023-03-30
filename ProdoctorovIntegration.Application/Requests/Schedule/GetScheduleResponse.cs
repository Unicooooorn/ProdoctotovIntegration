namespace ProdoctorovIntegration.Application.Requests.Schedule;

public class GetScheduleResponse
{

    public Schedule Schedule { get; set; } = new();
}

public class Schedule
{
    public string DepartmentName { get; set; } = string.Empty;
    public DoctorScheduleData Data { get; set; } = new();
}

public class DoctorScheduleData
{
    public Department Department { get; set; } = new();
}

public class Department
{
    public DoctorInfo DoctorInfo { get; set; } = new();
}

public class DoctorInfo
{
    public string FullName { get; set; } = string.Empty;
    public string Speciality { get; set; } = string.Empty;
    public Cell[] Cells { get; set; } = Array.Empty<Cell>();
}

public class Cell
{
    public string Date { get; set; } = string.Empty;
    public string TimeStart { get; set; } = string.Empty;
    public string TimeEnd { get; set; } = string.Empty;
    public bool IsFree { get; set; }
}
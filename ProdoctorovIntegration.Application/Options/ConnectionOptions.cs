namespace ProdoctorovIntegration.Application.Options;

public class ConnectionOptions
{
    public const string Position = "ConnectionOptions";

    public string SendSchedule { get; set; } = string.Empty;
    public string OccupiedSchedule { get; set; } = string.Empty;
}
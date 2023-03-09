namespace ProdoctorovIntegration.Domain;

public class Event
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public long Duration { get; set; }
    public Worker.Worker Worker { get; set; } = new();
    public long RoomId { get; set; }
    public Client.Client Client { get; set; } = new();
    public string ClientData { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public bool IsForProdoctorov { get; set; }
    public Guid? ClaimId { get; set; }
    public Worker.Worker InsertUserId { get; set; } = new();
    public Worker.Worker UpdateUserId { get; set; } = new();
}
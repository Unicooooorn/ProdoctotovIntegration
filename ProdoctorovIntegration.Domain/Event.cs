﻿namespace ProdoctorovIntegration.Domain;

public class Event
{
    public DateTime StartDate { get; set; }
    public long Duration { get; set; }
    public Worker.Worker Worker { get; set; } = new();
    public long RoomId { get; set; }
    public Client.Client Client { get; set; } = new();
    public string ClientData { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public bool IsForProdoctorov { get; set; }
    public Guid? ClaimId { get; set; }
    public long InsertUserId { get; set; }
    public long UpdateUserId { get; set; }
}
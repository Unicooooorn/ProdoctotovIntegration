﻿namespace ProdoctorovIntegration.Application.Response;

public class RecordClientResponse
{
    public long StatusCode { get; set; }
    public string ClaimId { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}
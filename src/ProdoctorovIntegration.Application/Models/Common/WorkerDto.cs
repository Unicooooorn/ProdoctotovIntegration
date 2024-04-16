using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Models.Common;

public class WorkerDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("lpu_id")]
    public string LpuId { get; set; } = string.Empty;
    [JsonPropertyName("specialty")]
    public SpecialtyDto Specialty { get; set; } = new();
}
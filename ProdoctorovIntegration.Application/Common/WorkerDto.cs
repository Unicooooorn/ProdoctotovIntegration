using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Common;

public class WorkerDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("lpu_id")]
    public Guid LpuId { get; set; }
    [JsonPropertyName("speciality")]
    public SpecialityDto Speciality { get; set; } = new();
}
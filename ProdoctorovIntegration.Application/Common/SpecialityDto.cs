using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Common;

public class SpecialityDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
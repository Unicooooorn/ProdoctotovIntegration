using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Models.Common;

public class SpecialtyDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
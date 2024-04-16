using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Models.Common;

public class ClientDto
{
    public Guid Id { get; set; }
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("second_name")]
    public string SecondName { get; set; } = string.Empty;
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;
    [JsonPropertyName("mobile_phone")]
    public string? MobilePhone { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }
}
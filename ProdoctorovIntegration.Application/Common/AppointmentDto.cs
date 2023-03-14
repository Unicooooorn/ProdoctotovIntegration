using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProdoctorovIntegration.Application.Common;

public class AppointmentDto
{
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [JsonPropertyName("dt_start")]
    public DateTime DateStart { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [JsonPropertyName("dt_end")]
    public DateTime DateEnd { get; set; }
    [JsonPropertyName("is_online")]
    public bool IsOnline { get; set; }
    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;
    [JsonPropertyName("is_club")]
    public bool? IsClub { get; set; }
}
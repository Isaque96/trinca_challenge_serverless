using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Churras
{
    public Churras() { }
    
    public Churras(string reason, string status, DateTime date, bool isTrincasPaying)
    {
        Id = Guid.NewGuid().ToString();
        Reason = reason;
        Status = status;
        Date = date;
        IsTrincasPaying = isTrincasPaying;
    }

    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("reason")]
    public string Reason { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("isTrincasPaying")]
    public bool IsTrincasPaying { get; set; }
}
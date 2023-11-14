using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Churras
{
    public Churras() { }

    public Churras(string reason, string status, DateTime date, bool isTrincasPaying)
    {
        Id = Guid.NewGuid();
        Reason = reason;
        Status = status;
        Date = date;
        IsTrincasPaying = isTrincasPaying;
    }

    [Key]
    [JsonPropertyName("id")]
    [Column("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("reason")]
    [Column("reason")]
    public string Reason { get; set; }
    
    [JsonPropertyName("status")]
    [Column("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("date")]
    [Column("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("isTrincasPaying")]
    [Column("isTrincasPaying")]
    public bool IsTrincasPaying { get; set; }
}
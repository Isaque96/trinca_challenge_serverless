using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Agenda
{
    public Agenda() { }

    public Agenda(string taskTitle, DateTime date, bool isBbq = false)
    {
        Id = Guid.NewGuid();
        TaskTitle = taskTitle;
        Date = date;
        IsBbq = isBbq;
    }

    [Key]
    [JsonPropertyName("id")]
    [Column("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("taskTitle")]
    [Column("taskTitle")]
    public string TaskTitle { get; set; }
    
    [JsonPropertyName("date")]
    [Column("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("isBbq")]
    [Column("isBbq")]
    public bool IsBbq { get; set; }
}
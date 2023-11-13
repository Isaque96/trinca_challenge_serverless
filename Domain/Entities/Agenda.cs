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

    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("taskTitle")]
    public string TaskTitle { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("isBbq")]
    public bool IsBbq { get; set; }
    
    [JsonPropertyName("churrasId")]
    public Guid ChurrasId { get; set; }
}
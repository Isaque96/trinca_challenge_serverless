using Newtonsoft.Json;

namespace Domain.Entities;

public class Agenda
{
    public Agenda() { }

    public Agenda(string taskTitle, DateTime date)
    {
        Id = Guid.NewGuid().ToString();
        TaskTitle = taskTitle;
        Date = date;
    }

    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("taskTitle")]
    public string TaskTitle { get; set; }
    
    [JsonProperty("date")]
    public DateTime Date { get; set; }
}
using Newtonsoft.Json;

namespace Domain.Entities;

public class Agenda
{
    public Agenda() { }

    public Agenda(string agendaTitle, DateTime date, bool done, string? id = null, string? churrasId = null!)
    {
        Id = id ?? Guid.NewGuid().ToString();
        AgendaTitle = agendaTitle;
        Date = date;
        Done = done;
        ChurrasId = churrasId;
    }

    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("agendaTitle")]
    public string AgendaTitle { get; set; }
    
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    
    [JsonProperty("done")]
    public bool Done { get; set; }
    
    [JsonProperty("churrasId")]
    public string? ChurrasId { get; set; }
}
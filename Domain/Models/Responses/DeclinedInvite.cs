using Newtonsoft.Json;

namespace Domain.Models.Responses;

public class DeclinedInvite
{
    public DeclinedInvite() { }

    public DeclinedInvite(string name, string message, string agendaId)
    {
        Name = name;
        Message = message;
        AgendaId = agendaId;
    }

    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("message")]
    public string Message { get; set; }
    
    [JsonProperty("agendaId")]
    public string AgendaId { get; set; }
}
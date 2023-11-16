using Newtonsoft.Json;

namespace Domain.Models.Responses;

public class AcceptedInvite
{
    public AcceptedInvite() { }

    public AcceptedInvite(string name, string message, string reason, string agendaId)
    {
        Name = name;
        Message = message;
        Reason = reason;
        AgendaId = agendaId;
    }

    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("message")]
    public string Message { get; set; }
    
    [JsonProperty("reason")]
    public string Reason { get; set; }
    
    [JsonProperty("agendaId")]
    public string AgendaId { get; set; }
}
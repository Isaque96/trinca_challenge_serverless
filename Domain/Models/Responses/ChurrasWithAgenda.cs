using Domain.Entities;
using Newtonsoft.Json;

namespace Domain.Models.Responses;

public class ChurrasWithAgenda
{
    public ChurrasWithAgenda() { }

    public ChurrasWithAgenda(Churras churras, Agenda agenda)
    {
        Churras = churras;
        Agenda = agenda;
    }

    [JsonProperty("churras")]
    public Churras Churras { get; set; }
    
    [JsonProperty("agenda")]
    public Agenda Agenda { get; set; }
}
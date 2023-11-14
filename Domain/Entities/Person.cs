using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Person
{
    public Person()
    {
        Agendas = new Collection<Agenda>();
    }

    public Person(string name, bool isModerator, ICollection<Agenda> agendas)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Agendas = agendas;
        IsModerator = isModerator;
    }

    public Person(string name, bool isModerator)
        : this(name, isModerator, new Collection<Agenda>()) { }

    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("isModerator")]
    public bool IsModerator { get; set; }
    
    [JsonProperty("agendas")]
    public ICollection<Agenda> Agendas { get; set; }
}
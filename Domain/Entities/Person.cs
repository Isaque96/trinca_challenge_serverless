using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Person
{
    public Person() { }

    public Person(string name, bool isModerator, IEnumerable<Agenda> agendas)
    {
        Id = Guid.NewGuid();
        Name = name;
        Agendas = agendas;
        IsModerator = isModerator;
    }

    public Person(string name, bool isModerator)
        : this(name, isModerator, Enumerable.Empty<Agenda>()) { }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("isModerator")]
    public bool IsModerator { get; set; }
    
    [JsonPropertyName("agendas")]
    public IEnumerable<Agenda> Agendas { get; set; }
}
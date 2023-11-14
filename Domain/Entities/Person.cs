using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [Key]
    [JsonPropertyName("id")]
    [Column("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    [Column("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("isModerator")]
    [Column("isModerator")]
    public bool IsModerator { get; set; }
    
    [JsonPropertyName("agendas")]
    public IEnumerable<Agenda> Agendas { get; set; }
}
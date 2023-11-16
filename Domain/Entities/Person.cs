using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Person
{
    public Person()
    {
        AgendasIds = new Collection<string>();
    }

    public Person(string name, bool isModerator, ICollection<string> agendasIds, string? id = null)
    {
        Id = id ?? Guid.NewGuid().ToString();
        Name = name;
        AgendasIds = agendasIds;
        IsModerator = isModerator;
    }

    public Person(string name, bool isModerator, string? id = null)
        : this(name, isModerator, new Collection<string>(), id) { }

    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("isModerator")]
    public bool IsModerator { get; set; }
    
    [JsonProperty("agendas")]
    public ICollection<string> AgendasIds { get; set; }
}
using Newtonsoft.Json;

namespace Domain.Models.Requests;

public class VeganBbq
{
    public VeganBbq() { }

    public VeganBbq(bool isVegan)
    {
        IsVegan = isVegan;
    }

    [JsonProperty("isVeg")]
    public bool IsVegan { get; set; }
}
using Domain.Entities;
using Newtonsoft.Json;

namespace Domain.Models.Requests;

public class ChurrasInfo
{
    public ChurrasInfo() { }
    
    public ChurrasInfo(DateTime date, string reason, bool isTrincaPaying)
    {
        Date = date;
        Reason = reason;
        IsTrincasPaying = isTrincaPaying;
    }

    [JsonProperty("date")]
    public DateTime Date { get; set; }
    
    [JsonProperty("reason")]
    public string Reason { get; set; }
    
    [JsonProperty("isTrincasPaying")]
    public bool IsTrincasPaying { get; set; }

    public Churras CreateEntity()
        => new(Reason, ChurrasStatus.New.ToString(), Date, IsTrincasPaying);
}
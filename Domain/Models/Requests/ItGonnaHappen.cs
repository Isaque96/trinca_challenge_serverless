using Newtonsoft.Json;

namespace Domain.Models.Requests;

public class ItGonnaHappen
{
    public ItGonnaHappen() { }

    public ItGonnaHappen(bool gonnaHappen, bool trincaWillPay)
    {
        GonnaHappen = gonnaHappen;
        TrincaWillPay = trincaWillPay;
    }

    [JsonProperty("GonnaHappen")]
    public bool GonnaHappen { get; set; }
    
    [JsonProperty("TrincaWillPay")]
    public bool TrincaWillPay { get; set; }
}
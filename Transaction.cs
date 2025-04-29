using Newtonsoft.Json;

namespace SupportBank;
public class Transaction
{
    [JsonProperty("Date")]
    public required string Date { get; set; }
    [JsonProperty("FromAccount")]
    public required string From { get; set; }
    [JsonProperty("ToAccount")]
    public required string To { get; set; }
    [JsonProperty("Narrative")]
    public required string Narrative { get; set; }
    [JsonProperty("Amount")]
    public double Amount { get; set; }
}
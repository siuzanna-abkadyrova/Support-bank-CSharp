namespace SupportBank;
public class Transaction
{
    public required string Date { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public required string Narrative { get; set; }
    public double Amount { get; set; }
}
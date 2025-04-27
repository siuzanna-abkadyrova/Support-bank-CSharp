using System.Text.Json;

namespace SupportBank;

public class Printer
{
    public static void PrintAllPeople(Dictionary<string, List<Transaction>> peopleTransactions)
    {
        foreach (var kvp in peopleTransactions)
        {
            string name = kvp.Key;
            double owes = kvp.Value.Where(transaction => transaction.From == name)
                .Sum(transaction => transaction.Amount);
            double isOwed = kvp.Value.Where(transaction => transaction.To == name).Sum(transaction => transaction.Amount);
            
            Console.WriteLine($"{name} owes £{owes:F2} and is owed £{isOwed:F2}");
        }
    }

    public static void PrintPersonTransactions(List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            Console.WriteLine($"{transaction.Date} from: {transaction.From} to: {transaction.To} narrative: {transaction.Narrative} amount: {transaction.Amount}");
        }
    }

    public static void PrintInvalidDataWarning(List<string> invalidRecords)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Warning: One or more rows in the CSV file contained invalid data and could not be processed. These rows have been excluded from the transaction processing:");
        Console.WriteLine(string.Join(Environment.NewLine, invalidRecords));
        Console.ResetColor();
    }
}
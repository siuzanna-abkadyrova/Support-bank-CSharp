using System.Text.RegularExpressions;
namespace SupportBank;
public static class App
{
    private static Dictionary<string, List<Transaction>> TransactionsByPerson = new Dictionary<string, List<Transaction>>();
    public static void Run()
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.WriteLine("Please enter your command");
            var command = Console.ReadLine() ?? "";
            if (command.Contains("List All", StringComparison.CurrentCultureIgnoreCase))
            {
                Printer.PrintAllPeople(TransactionsByPerson);
            } else if (command.Contains("List", StringComparison.CurrentCultureIgnoreCase))
            {
                List<Transaction>? transactions = App.GetPersonTransactions(command, TransactionsByPerson);

                if (transactions != null)
                {
                    Printer.PrintPersonTransactions(transactions);
                    continue;
                }

                Console.WriteLine("Account does not exist");
            } else if (command.Contains("Import File", StringComparison.CurrentCultureIgnoreCase))
            {
                string? fileName = GetFilenameFromCommand(command);

                if (fileName != null)
                {
                    TransactionsByPerson = GetTransactionsByPerson(fileName);
                    Console.WriteLine("File imported");
                    continue;
                }
                
                Console.WriteLine("File does not exist");
            }
            else if (command.Contains("Exit", StringComparison.CurrentCultureIgnoreCase))
            {
                keepRunning = false;
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }
    private static List<Transaction>? GetPersonTransactions(string input, Dictionary<string, List<Transaction>> transactionsByPerson)
    {
        string pattern = @"List (.+)";
        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            string name = match.Groups[1].Value;
            if (!transactionsByPerson.ContainsKey(name))
            {
                return null;
            }
            return transactionsByPerson[name];
        }
        else
        {                
            return null;
        }
    }
    
    private static string? GetFilenameFromCommand(string input)
    {
        string prefix = "Import File ";
        if (input.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return input.Substring(prefix.Length).Trim();
        }
        return null;
    }

    private static Dictionary<string, List<Transaction>> GetTransactionsByPerson(string fileName)
    {
        List<Transaction> transactions = FileParser.Parse(fileName) ?? new List<Transaction>();
        var transactionsByPerson = new Dictionary<string, List<Transaction>>();

        foreach (var transaction in transactions)
        {
            if (!transactionsByPerson.ContainsKey(transaction.From))
            {
                transactionsByPerson[transaction.From] = new List<Transaction>();
            }
            transactionsByPerson[transaction.From].Add(transaction);
    
            if (!transactionsByPerson.ContainsKey(transaction.To))
            {
                transactionsByPerson[transaction.To] = new List<Transaction>();
            }
            transactionsByPerson[transaction.To].Add(transaction);
        }

        return transactionsByPerson;
    }
}
using System.Text.RegularExpressions;
namespace SupportBank;
public class ConsoleInput
{
    public static void Run(Dictionary<string, List<Transaction>> peopleTransactions)
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.WriteLine("Please enter your command");
            var command = Console.ReadLine() ?? "";
            if (command.Contains("List All"))
            {
                Printer.PrintAllPeople(peopleTransactions);
            } else if (command.Contains("List", StringComparison.CurrentCultureIgnoreCase))
            {
                List<Transaction>? transactions = ConsoleInput.GetPersonAccountFromInput(command, peopleTransactions);

                if (transactions != null)
                {
                    Printer.PrintPersonTransactions(transactions);
                    continue;
                }
                
                Console.WriteLine("Account does not exist");
            }
            else if (command.Contains("Exit"))
            {
                keepRunning = false;
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }
    private static List<Transaction>? GetPersonAccountFromInput(string input, Dictionary<string, List<Transaction>> peopleTransactions)
    {
        string pattern = @"List (.+)";
        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            string name = match.Groups[1].Value;
            if (!peopleTransactions.ContainsKey(name))
            {
                return null;
            }
            return peopleTransactions[name];
        }
        else
        {                
            return null;
        }
    }
}
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SupportBank;
using NLog;
using NLog.Config;
using NLog.Targets;

ILogger logger = LogManager.GetCurrentClassLogger();

var config = new LoggingConfiguration();
var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
config.AddTarget("File Logger", target);
config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
LogManager.Configuration = config;

logger.Info("Application started");

string normalFile = "Transactions2014.csv";
string dodgyFile = "DodgyTransactions2015.csv";
using var reader = new StreamReader(dodgyFile); 
using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

try
{
    var validRecords = new List<Transaction>();
    var invalidRecords = new List<string>();  
    
    while (csv.Read())
    {
        try
        {
            var transaction = csv.GetRecord<Transaction>();
            validRecords.Add(transaction);
        }
        catch (Exception ex)
        {
            logger.Error($"Error processing row: {ex.Message}");
            var rawRow = csv.Parser.Record;  
            if (rawRow != null)
            {
                invalidRecords.Add(string.Join(",", rawRow)); 
            }
        }
    }

    if (invalidRecords.Count > 0)
    {
        Printer.PrintInvalidDataWarning(invalidRecords);
    }

    var peopleTransactions = new Dictionary<string, List<Transaction>>();

    foreach (var transaction in validRecords)
    {
        if (!peopleTransactions.ContainsKey(transaction.From))
        {
            peopleTransactions[transaction.From] = new List<Transaction>();
        }
        peopleTransactions[transaction.From].Add(transaction);
    
        if (!peopleTransactions.ContainsKey(transaction.To))
        {
            peopleTransactions[transaction.To] = new List<Transaction>();
        }
        peopleTransactions[transaction.To].Add(transaction);
    }

    ConsoleInput.Run(peopleTransactions);
}
catch (CsvHelperException ex)
{
    logger.Error($"Error reading CSV file: {ex.Message}");
}

logger.Info("Application stopped");
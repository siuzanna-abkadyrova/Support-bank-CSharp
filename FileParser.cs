using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using NLog;

namespace SupportBank;

public class FileParser
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public static List<Transaction>? Parse(string filename)
    {
        string fileExtension = Path.GetExtension(filename).ToLower();

        if (fileExtension == ".json")
        {
            return ParseJSON(filename);
        }
        else if (fileExtension == ".csv")
        {
            return ParseCSV(filename);
        }
        else
        {
            Console.WriteLine("Unsupported file type.");
            return null;
        }
    }
    private static List<Transaction> ParseJSON(string filename)
    {
        string jsonContent = File.ReadAllText(filename);
        List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonContent) ?? new List<Transaction>();
        
        return transactions;
    }
    
    private static List<Transaction> ParseCSV(string filename)
    {
        using var reader = new StreamReader(filename); 
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        
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
                Logger.Error($"Error processing row: {ex.Message}");
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
        
        return validRecords;
    }
}
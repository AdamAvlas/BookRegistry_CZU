using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookRegistry
{
    public static class ErrorLogger
    {
        public static void LogError(Exception exception)
        {
            Error testError = new(exception.Message, exception.Source, DateTime.Now);

            string output = JsonSerializer.Serialize(testError);

            string fileName = $"log.{DateTime.Today.ToString("dd_MM_yyyy")}.json";
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, fileName);

            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }

            int noFiles = Directory.GetFiles(logDirectoryPath).Length;
            if (noFiles >= 3 && !File.Exists(logFilePath))
            {
                Dictionary<string, DateTime> keyValuePairs = new Dictionary<string, DateTime>();

                foreach (string file in Directory.GetFiles(logDirectoryPath))
                {
                    keyValuePairs.Add(file, File.GetLastWriteTime(file));
                }
                keyValuePairs = (Dictionary<string, DateTime>)keyValuePairs.OrderBy(keyValue => keyValue.Value);

            }

            Console.WriteLine($"Logging error to: {logFilePath}");

            using (StreamWriter streamWriter = new(logFilePath, true))
            {
                streamWriter.WriteLine(output);
            }
        }
    }

    public class Error
    {
        public Error(string message, string source, DateTime timeStamp) 
        {
            Message = message;
            Source = source;
            Timestamp = timeStamp;
        }
        public string Message { get; set; }
        public String Source { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

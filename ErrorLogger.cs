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
            Error testError = new(exception.Message, exception.StackTrace, exception.Source, DateTime.Now);

            string output = JsonSerializer.Serialize(testError);

            string fileName = $"log.{DateTime.Today.ToString("dd_MM_yyyy")}.json";
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, fileName);

            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }


            Console.WriteLine($"Logging error to: {logFilePath}");

            using (StreamWriter streamWriter = new(logFilePath, true))
            {
                streamWriter.WriteLine(output);
            }

            int noFiles = Directory.GetFiles(logDirectoryPath).Length;
            const int maxKeptLogFiles = 3;
            if (noFiles > maxKeptLogFiles)
            {
                Dictionary<string, DateTime> filesDict = [];

                foreach (string file in Directory.GetFiles(logDirectoryPath))
                {
                    filesDict.Add(file, File.GetLastWriteTime(file));
                }
                var filesDictSorted = filesDict.OrderBy(pair => pair.Value);

                for (int i = 0; i < (noFiles-maxKeptLogFiles); i++)
                {
                    File.Delete(filesDictSorted.ElementAt(i).Key);
                }
            }
        }
    }

    public class Error(string message, string stackTrace, string source, DateTime timeStamp)
    {
        public string Message { get; set; } = message;
        public string StackTrace { get; set; } = stackTrace;
        public String Source { get; set; } = source;
        public DateTime Timestamp { get; set; } = timeStamp;
    }
}

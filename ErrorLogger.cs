﻿using System.Text.Json;

namespace BookRegistry;

public static class ErrorLogger//class for logging more complex errors to an external file
{
    public static void LogError(Exception exception)
    {
        string stackTrace = "Error stack trace could not be found/parsed correctly";
        string errorSource = "Error source could not be found/parsed correctly";
        if (exception.StackTrace is not null)
        {
            stackTrace = exception.StackTrace.ToString();
        }
        if (exception.Source is not null)
        {
            errorSource = exception.Source.ToString();
        }
        Error error = new(exception.Message, stackTrace, errorSource, DateTime.Now);

        string output = JsonSerializer.Serialize(error);

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
        if (noFiles > maxKeptLogFiles)//deletes older log-files so that only 3 newest are kept
        {
            Dictionary<string, DateTime> filesDict = [];

            foreach (string file in Directory.GetFiles(logDirectoryPath))
            {
                filesDict.Add(file, File.GetLastWriteTime(file));
            }
            var filesDictSorted = filesDict.OrderBy(pair => pair.Value);

            for (int i = 0; i < (noFiles - maxKeptLogFiles); i++)
            {
                File.Delete(filesDictSorted.ElementAt(i).Key);
            }
        }
    }
}

public class Error(string message, string stackTrace, string source, DateTime timeStamp)//a simple object representing a single error to be serialized
{
    public string Message { get; set; } = message;
    public string StackTrace { get; set; } = stackTrace;
    public String Source { get; set; } = source;
    public DateTime Timestamp { get; set; } = timeStamp;
}

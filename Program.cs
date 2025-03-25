using System.Configuration;
using BookRegistry.Classes;

namespace BookRegistry
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseHandler databaseHandler = new();
            databaseHandler.Initialize();

            ConsoleFunctions.MainMenu(databaseHandler);
        }
    }
}

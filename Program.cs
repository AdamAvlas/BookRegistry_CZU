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

            bool endProgram = false;
            while (!endProgram)
            {
                Console.Clear();
                Console.WriteLine("(1) View All\n(2) Create New\n(3) Edit existing\n(4) Exit program");
                Console.Write("Your choice: ");
                string consoleCommand = Console.ReadLine();

                if (consoleCommand.Length == 0)
                {
                    Console.WriteLine("Vole nemůžeš zadat NIC");
                    Console.ReadLine();
                    continue;
                }

                //Console.WriteLine($"napsals: {consoleCommand} typu[{consoleCommand.Length}]");

                if (!Int32.TryParse(consoleCommand, out int consoleCommandInt))
                {
                    Console.WriteLine("Neplatná volba vole!");
                    Console.ReadLine();
                    continue;
                }

                switch (consoleCommandInt)
                {
                    case 1:
                        //Console.WriteLine("View all");
                        ConsoleFunctions.ViewAll(databaseHandler);
                        break;

                    case 2:
                        Console.WriteLine("You have chosen option 1....bitch");
                        break;

                    case 4:
                        Console.WriteLine("Exiting...");
                        endProgram = true;
                        break;

                    default:
                        Console.WriteLine("Neplatná/neexistující volba!");
                        break;

                }
            }
        }
    }
}

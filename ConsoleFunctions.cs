using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookRegistry.Classes;

namespace BookRegistry
{
    public static class ConsoleFunctions
    {
        public static int UserInputCheck(string userInput)
        {
            if (userInput.Length == 0)
            {
                Console.WriteLine("Nemůžeš zadat NIC!");
                Console.ReadLine();
                return 0;
            }

            if (!Int32.TryParse(userInput, out int userInputInteger))
            {
                Console.WriteLine("Neplatná volba vole!");
                Console.ReadLine();
                return 0;
            }

            return userInputInteger;
        }
        public static void MainMenu(DatabaseHandler databaseHandler)
        {
            bool endProgram = false;
            while (!endProgram)
            {
                Console.Clear();
                Console.WriteLine("(1) View All\n(2) Create New\n(3) Edit existing\n(4) Exit program");
                Console.Write("Your choice: ");
                string consoleCommand = Console.ReadLine();
                int consoleCommandInt = UserInputCheck(consoleCommand);

                if (consoleCommandInt == 0)
                {
                    continue;
                }

                switch (consoleCommandInt)
                {
                    case 1:
                        //Console.WriteLine("View all");
                        ConsoleFunctions.ViewAll(databaseHandler);
                        break;

                    case 2:
                        Console.WriteLine("You have chosen option 1");
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
        public static void ViewAll(DatabaseHandler databaseHandler)
        {
            Console.Clear();
            Console.WriteLine("Výpis všech dostupných knih: ");
            foreach (Book book in databaseHandler.Books)
            {
                Console.WriteLine($"[{book.Id}]-[{book.Title}]-[{book.Author.GetFullName()}]-[{book.Category.Name}]");
            }

            bool endBlock = false;
            while (!endBlock)
            {
                Console.WriteLine("\n(1) Edit book\n(2) Remove book\n(3) Return to main menu");
                Console.Write("Your choice: ");
                string consoleCommand = Console.ReadLine();
                int consoleCommandInt = UserInputCheck(consoleCommand);
                
                if (consoleCommandInt == 0)
                {
                    continue;
                }

                switch (consoleCommandInt)
                {
                    case 1:
                        Console.WriteLine("You have chosen option 1");
                        break;

                    case 2:
                        Console.WriteLine("You have chosen option 2");
                        break;

                    case 3:
                        Console.WriteLine("Exiting...");
                        endBlock = true;
                        break;

                    default:
                        Console.WriteLine("Neplatná/neexistující volba!");
                        break;

                }
            }
        }

        public static void CreateNewBook(DatabaseHandler databaseHandler)
        {
            Console.WriteLine("Creating new book:");
            Console.Write("Enter title: ");
            string bookTitle = Console.ReadLine();
            Console.WriteLine("\nChoose author/create new: ");

            int i = 1;
            foreach (Author author in databaseHandler.Authors)
            {
                Console.WriteLine($"({i}) {author.GetFullName()} [{author.Id}]");
            }

            Console.WriteLine($"({i + 1}) Create new author");
            Console.Write("Your choice: ");
        }
    }
}

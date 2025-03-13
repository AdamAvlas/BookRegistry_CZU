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
        public static void ViewAll(DatabaseHandler databaseHandler)
        {
            Console.Clear();
            Console.WriteLine("Výpis všech dostupných knih: ");
            foreach (Book book in databaseHandler.Books)
            {
                Console.WriteLine($"[{book.Id}]-[{book.Title}]-[{book.Author.GetFullName()}]-[{book.Category.Name}]");
            }
            Console.WriteLine("\n Možnosti:\n(1) Návrat do menu\n(2) Upravit\n(3) Smazat");
            Console.Write("Your choice: ");
            Console.ReadLine();

            bool endBlock = false;
            while (!endBlock)
            {
                Console.Clear();
                Console.WriteLine("(1) Return to menu\n(2) Edit book\n(3) Remove book");
                Console.Write("Your choice: ");
                string consoleCommand = Console.ReadLine();

                if (consoleCommand.Length == 0)
                {
                    Console.WriteLine("Vole nemůžeš zadat NIC");
                    Console.ReadLine();
                    continue;
                }

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

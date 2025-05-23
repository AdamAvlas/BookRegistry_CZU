﻿using BookRegistry.Classes;
using System.Text.RegularExpressions;

namespace BookRegistry;

public static class ConsoleFunctions//the main function handling communication with user, static, does not need to be initialized
{
    private static string UserInputCheck(string userInput)
    {
        const int maxLength = 40;
        if (userInput == null)
        {
            Console.WriteLine($"Your title was empty!");
            return "";
        }
        string regexPattern = @"[^a-zA-Z0-9!""()?.,$#@%:ěščřžýáíéúůďťňĚŠČŘŽÝÁÍÉŮÚĎŤŇ ]";
        userInput = userInput.Trim();
        userInput = Regex.Replace(userInput, regexPattern, String.Empty);
        if (userInput.Length > 0 && userInput.Length < maxLength)
        {
            return userInput;
        }
        Console.WriteLine($"Your title was either too long, or to short. Must be at most {maxLength} characters long (yours was {userInput.Length} long)");
        return "";
    }//method for checking the user input

    private static int MenuInputCheck(string userInput)
    {
        if (userInput == null || userInput.Length == 0)
        {
            Console.WriteLine("Cannot enter NOTHING!");
            Console.ReadLine();
            return -1;
        }

        if (!Int32.TryParse(userInput, out int userInputInteger))
        {
            Console.WriteLine("Invalid input!");
            Console.ReadLine();
            return -1;
        }

        return userInputInteger;
    }//method for checking the user input, when chosing an option in a menu

    public static void MainMenu(DatabaseHandler databaseHandler)
    {
        bool endProgram = false;
        while (!endProgram)
        {
            Console.Clear();
            Console.WriteLine("(1) View All\n(2) Create New\n(3) Edit existing\n(4) Remove existing\n(5) Exit program");
            Console.Write("Your choice: ");
            string consoleCommand = Console.ReadLine()!;
            int consoleCommandInt = MenuInputCheck(consoleCommand);

            if (consoleCommandInt == 0)
            {
                continue;
            }

            switch (consoleCommandInt)
            {
                case 1:
                    ViewAll(databaseHandler);
                    break;

                case 2:
                    CreateNewBook(databaseHandler);
                    break;

                case 3:
                    EditBook(databaseHandler);
                    break;

                case 4:
                    RemoveBook(databaseHandler);
                    break;

                case 5:
                    Console.WriteLine("Exiting...");
                    endProgram = true;
                    break;

                default:
                    Console.WriteLine("Neplatná/neexistující volba!");
                    break;
            }
        }
    }//the main/default method called directly after the program is started, and to which it returns after most succesful operatinos

    private static void ViewAll(DatabaseHandler databaseHandler)
    {
        Console.Clear();
        Console.WriteLine($"Výpis všech dostupných knih: ");
        foreach (Book book in databaseHandler.Books)
        {
            Console.WriteLine($"[{book.Id}]-[{book.Title}]-[{book.Author.GetFullName()}]-[{book.Category.Name}]");
        }
        Console.WriteLine("----------------------------------------");

        Console.WriteLine("To return to the main menu, press any key");
        Console.ReadLine();
    }//READ method, displays all existing books

    private static void CreateNewBook(DatabaseHandler databaseHandler)//CREATE method, for creating new books
    {
        Console.Clear();
        Console.WriteLine("Creating new book:");
        string newBookTitle;
        while (true)
        {
            Console.Write("Enter title: ");
            newBookTitle = UserInputCheck(Console.ReadLine()!);
            if (newBookTitle.Length > 0)
            {
                break;
            }
        }
        Console.WriteLine($"Chosen title: {newBookTitle}");

        //either choosing an author, or creating another one
        Author? newBookAuthor = null;
        bool endAuthorBlock = true;
        while (endAuthorBlock)
        {
            Console.WriteLine("\nChoose author/create new: ");

            int i = 0;
            foreach (Author author in databaseHandler.Authors)
            {
                Console.WriteLine($"({i + 1}) {author.GetFullName()} [{author.Id}]");
                i++;
            }
            Console.WriteLine($"\n({0}) Create new");

            int authorIDInteger;
            while (true)
            {
                Console.Write("Your choice: ");
                string authorID = Console.ReadLine()!;
                authorIDInteger = MenuInputCheck(authorID);

                if (authorIDInteger > 0 && authorIDInteger <= i)
                {
                    newBookAuthor = databaseHandler.Authors[authorIDInteger - 1];
                    endAuthorBlock = false;
                    break;
                }
                else if (authorIDInteger == 0)
                {
                    CreateNewAuthor(databaseHandler);
                    break;
                }
            }
        }
        Console.WriteLine($"Author chosen: {newBookAuthor.GetFullName()}");

        Category? newBookCategory = null;
        bool endCategoryBlock = true;
        while (endCategoryBlock)
        {
            Console.WriteLine("\nChoose category/create new: ");

            int j = 0;
            foreach (Category category in databaseHandler.Categories)
            {
                Console.WriteLine($"({j + 1}) {category.Name} [{category.Id}]");
                j++;
            }
            Console.WriteLine($"\n({0}) Create new");

            int categoryIDInteger;
            while (true)
            {
                Console.Write("Your choice: ");
                string categoryID = Console.ReadLine()!;
                categoryIDInteger = MenuInputCheck(categoryID);

                if (categoryIDInteger > 0 && categoryIDInteger <= j)
                {
                    newBookCategory = databaseHandler.Categories[categoryIDInteger - 1];
                    endCategoryBlock = false;
                    break;
                }
                else if (categoryIDInteger == 0)
                {
                    CreateNewCategory(databaseHandler);
                    break;
                }
            }

        }
        Console.WriteLine($"Category chosen: {newBookCategory.Name}");

        while (true)
        {
            Console.WriteLine("Confirm? [Y/N]: ");
            string choice = Console.ReadLine()!;
            if (choice == "Y" || choice == "y")
            {
                Console.WriteLine("Creating new book...");
                Book newBook = new(0, newBookTitle, newBookCategory, newBookAuthor);
                databaseHandler.InsertNewBook(newBook);
                Console.ReadLine();
                break;
            }
            if (choice == "N" || choice == "n")
            {
                Console.WriteLine("Canceling the operation...");
                Console.ReadLine();
                break;
            }
        }
    }

    private static void CreateNewAuthor(DatabaseHandler databaseHandler)//CREATE method, for creating a new author(only ever called from CreateNewBook and UpdateBook, not directly from MM)
    {
        Console.WriteLine("Creating new author:");
        string newAuthorName;
        while (true)
        {
            Console.Write("Enter first name: ");
            newAuthorName = UserInputCheck(Console.ReadLine()!);
            if (newAuthorName.Length > 0)
            {
                break;
            }
        }
        string newAuthorLastName;
        while (true)
        {
            Console.Write("Enter last name: ");
            newAuthorLastName = UserInputCheck(Console.ReadLine()!);
            if (newAuthorName.Length > 0)
            {
                break;
            }
        }
        string newAuthorBirthdateString;
        DateOnly newAuthorBirthdate = new(1, 1, 1);
        while (true)
        {
            Console.Write("Enter birth date (format: dd.mm.yyyy)]: ");
            newAuthorBirthdateString = Console.ReadLine()!;
            if (newAuthorBirthdateString is not null && newAuthorBirthdateString.Length > 0)
            {
                try
                {
                    newAuthorBirthdate = DateOnly.ParseExact(newAuthorBirthdateString, ["dd-mm-yyyy", "dd.mm.yyyy", "dd mm yyyy"]);//just to be safe, 3 different possible formats
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Incorrect date format! Please make sure you enter the date in the correct format (dd.mm.yyyy)!: {ex.Message}");
                }
            }
        }
        Author newAuthor = new(0, newAuthorName, newAuthorLastName, newAuthorBirthdate);
        databaseHandler.InsertNewAuthor(newAuthor);
    }

    private static void CreateNewCategory(DatabaseHandler databaseHandler)//CREATE method, for creating a new category(only ever called from CreateNewBook and UpdateBook, not directly from MM)
    {
        Console.WriteLine("Creating new category:");
        string newCategoryName;
        while (true)
        {
            Console.Write("Enter category name/title: ");
            newCategoryName = UserInputCheck(Console.ReadLine()!);
            if (newCategoryName.Length > 0)
            {
                break;
            }
        }

        Category newCategory = new(0, newCategoryName);
        databaseHandler.InsertNewCategory(newCategory);
    }

    private static void RemoveBook(DatabaseHandler databaseHandler)//DELETE method, for removing existing books, parses whole object to DBHandler
    {
        Console.Clear();
        Console.WriteLine("Removing a book:");

        foreach (Book book in databaseHandler.Books)
        {
            Console.WriteLine($"[{book.Id}]-[{book.Title}]-[{book.Author.GetFullName()}]-[{book.Category.Name}]");
        }

        int bookToRemoveID = 0;
        Book? bookToRemove;
        while (true)
        {
            Console.Write("Choose book to remove: ");
            string bookToRemoveStr = Console.ReadLine()!;
            bookToRemoveID = MenuInputCheck(bookToRemoveStr);

            if (bookToRemoveID > 0)
            {
                bookToRemove = databaseHandler.Books.Find(b => b.Id == bookToRemoveID);
                if (bookToRemove == null)
                {
                    Console.WriteLine($"No book with ID[{bookToRemoveID}] exists, please try again");
                    continue;
                }

                Console.WriteLine($"You have chosen to remove book [{bookToRemove.Title}]");
                break;
            }
        }
        while (true)
        {
            Console.WriteLine("Confirm removal? [Y/N]: ");
            string choice = Console.ReadLine()!;
            if (choice == "Y" || choice == "y")
            {
                Console.WriteLine("Removing book....");
                databaseHandler.DeleteBook(bookToRemove);
                Console.ReadLine();
                break;
            }
            if (choice == "N" || choice == "n")
            {
                Console.WriteLine("Canceling the operation...");
                Console.ReadLine();
                break;
            }
        }
    }

    private static void EditBook(DatabaseHandler databaseHandler)
    {
        Console.Clear();
        Console.WriteLine("Updating...");
        foreach (Book book in databaseHandler.Books)
        {
            Console.WriteLine($"[{book.Id}]-[{book.Title}]-[{book.Author.GetFullName()}]-[{book.Category.Name}]");
        }

        int bookToUpdateID = 0;
        Book bookToUpdate;

        while (true)
        {
            Console.Write("Choose book to update: ");
            string bookToRemoveStr = Console.ReadLine()!;
            bookToUpdateID = MenuInputCheck(bookToRemoveStr);

            if (bookToUpdateID > 0)
            {
                bookToUpdate = databaseHandler.Books.Find(b => b.Id == bookToUpdateID);
                if (bookToUpdate == null)
                {
                    Console.WriteLine($"No book with ID[{bookToUpdateID}] exists, please try again");
                    continue;
                }

                Console.WriteLine($"You have chosen to update book [{bookToUpdate.Title}]");
                break;
            }
        }

        string newBookTitle;
        while (true)
        {
            Console.Write("Enter new title name: ");
            newBookTitle = UserInputCheck(Console.ReadLine()!);
            if (newBookTitle.Length > 0)
            {
                break;
            }
        }
        Console.WriteLine($"New title chosen: {newBookTitle}");

        Author? newBookAuthor = null;
        bool endAuthorBlock = true;
        while (endAuthorBlock)
        {
            Console.WriteLine("\nChoose author/create new: ");

            int i = 0;
            foreach (Author author in databaseHandler.Authors)
            {
                Console.WriteLine($"({i + 1}) {author.GetFullName()} [{author.Id}]");
                i++;
            }
            Console.WriteLine($"\n({0}) Create new");

            int authorIDInteger;
            while (true)
            {
                Console.Write("Your choice: ");
                string authorID = Console.ReadLine()!;
                authorIDInteger = MenuInputCheck(authorID);

                if (authorIDInteger > 0 && authorIDInteger <= i)
                {
                    newBookAuthor = databaseHandler.Authors[authorIDInteger - 1];
                    endAuthorBlock = false;
                    break;
                }
                else if (authorIDInteger == 0)
                {
                    CreateNewAuthor(databaseHandler);
                    break;
                }
            }
        }
        Console.WriteLine($"New author chosen: {newBookAuthor.GetFullName()}");

        Category? newBookCategory = null;
        bool endCategoryBlock = true;
        while (endCategoryBlock)
        {
            Console.WriteLine("\nChoose category/create new: ");

            int j = 0;
            foreach (Category category in databaseHandler.Categories)
            {
                Console.WriteLine($"({j + 1}) {category.Name} [{category.Id}]");
                j++;
            }
            Console.WriteLine($"\n({0}) Create new");

            int categoryIDInteger;
            while (true)
            {
                Console.Write("Your choice: ");
                string categoryID = Console.ReadLine()!;
                categoryIDInteger = MenuInputCheck(categoryID);

                if (categoryIDInteger > 0 && categoryIDInteger <= j)
                {
                    newBookCategory = databaseHandler.Categories[categoryIDInteger - 1];
                    endCategoryBlock = false;
                    break;
                }
                else if (categoryIDInteger == 0)
                {
                    CreateNewCategory(databaseHandler);
                    break;
                }
            }

        }
        Console.WriteLine($"New category chosen: {newBookCategory.Name}");

        while (true)
        {
            Console.WriteLine("Confirm changes? [Y/N]: ");
            string choice = Console.ReadLine()!;
            if (choice == "Y" || choice == "y")
            {
                Console.WriteLine("Updating book...");
                Book newBook = new(0, newBookTitle, newBookCategory, newBookAuthor);
                databaseHandler.UpdateBook(bookToUpdate, newBook);
                Console.ReadLine();
                break;
            }
            if (choice == "N" || choice == "n")
            {
                Console.WriteLine("Canceling the operation...");
                Console.ReadLine();
                break;
            }
        }
    }//UPDATE method, for modifying existing books
}
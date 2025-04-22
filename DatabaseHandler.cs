using System.Configuration;
using BookRegistry.Classes;
using Microsoft.Data.SqlClient;

namespace BookRegistry;

public class DatabaseHandler//a mainly back-end method, handling the communication with the SQL database
{
    public List<Book> Books = [];
    public List<Author> Authors = [];
    public List<Category> Categories = [];

    private string connectionString = String.Empty;

    public void Initialize()//method thats called on program startup, loading the data into it
    {
        Books = [];
        Authors = [];
        Categories = [];

        try
        {
            connectionString = ConfigurationManager.AppSettings["ConnectionString"]!;//getting the DB connection 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving database connection details from configuration file: [{ex.Message}], please check the configuration file and try again");
            ErrorLogger.LogError(ex);
            Environment.Exit(2);
        }

        using (SqlConnection sqlConnection = new(connectionString))
        {
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connection to database: [{ex.Message}], please check the configuration file and try again");
                ErrorLogger.LogError(ex);
                Environment.Exit(1);
            }

            //reading the data from the database
            SqlCommand selectAuthorsCommand = new("SELECT TOP(100) author_id, first_name, last_name, birthdate FROM authors", sqlConnection);
            SqlCommand selectCategoriesCommand = new("SELECT TOP(100) category_id, category_name FROM categories", sqlConnection);
            SqlCommand selectBooksCommand = new("SELECT TOP(100) book_id, title, category_id, author_id FROM books", sqlConnection);

            try
            {
                SqlDataReader reader = selectAuthorsCommand.ExecuteReader();
                while (reader.Read())
                {
                    Author newAuthor = new(int.Parse(reader["author_id"].ToString()!), reader["first_name"].ToString()!, reader["last_name"].ToString()!, DateOnly.FromDateTime((DateTime)reader["birthdate"]));
                    Authors.Add(newAuthor);
                }
                reader.Close();
                reader = selectCategoriesCommand.ExecuteReader();
                while (reader.Read())
                {
                    Category newCategory = new(int.Parse(reader["category_id"].ToString()!), reader["category_name"].ToString()!);
                    Categories.Add(newCategory);
                }
                reader.Close();
                reader = selectBooksCommand.ExecuteReader();
                while (reader.Read())
                {
                    int authorId = int.Parse(reader["author_id"].ToString()!);
                    int categoryId = int.Parse(reader["category_id"].ToString()!);
                    Category bookCategory = Categories.Find(cat => cat.Id == categoryId)!;
                    Author bookAuthor = Authors.Find(auth => auth.Id == authorId)!;

                    Book newBook = new(int.Parse(reader["book_id"].ToString()!), reader["title"].ToString()!, bookCategory, bookAuthor);
                    Books.Add(newBook);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching data from database: {ex}, please contact your administrator, or check the log file for more information");
                ErrorLogger.LogError(ex);
                Console.ReadLine();
            }

            sqlConnection.Close();
        }
    }

    public void Update()//method thats called after every db operation so that the program has up-to-date data
    {
        Initialize();
    }

    public void InsertNewBook(Book newBook)
    {
        using (SqlConnection sqlConnection = new(connectionString))
        {
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connection to database: {ex.Message}, please check the configuration file and try again");
                ErrorLogger.LogError(ex);
            }

            SqlCommand insertBook = new("INSERT INTO books(title,category_id,author_id,date_added) VALUES(@Title, @CategoryID, @AuthorID, GETDATE())", sqlConnection);
            insertBook.Parameters.AddRange([
                new SqlParameter("Title", newBook.Title),
                new SqlParameter("CategoryID", newBook.Category.Id),
                new SqlParameter("AuthorID", newBook.Author.Id)
            ]);

            try
            {
                if (insertBook.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine($"Succesfully created new book {newBook.Title}!");
                }
                else
                {
                    Console.WriteLine("The program ran into an issue while creating the new book###");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting record into database: {ex.Message}, please contact your administrator, or check the log file for more information");
                ErrorLogger.LogError(ex);
            }
        }
        Update();
    }

    public void InsertNewAuthor(Author newAuthor)
    {
        using (SqlConnection sqlConnection = new(connectionString))
        {
            try
            {
                sqlConnection.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connection to database: {ex.Message}, please check the configuration file and try again");
                ErrorLogger.LogError(ex);
            }

            SqlCommand insertAuthor = new("INSERT INTO authors(first_name,last_name,birthdate) VALUES(@Name,@LastName,@Birthdate)", sqlConnection);
            insertAuthor.Parameters.AddRange([
                new SqlParameter("@Name", newAuthor.Name),
                new SqlParameter("@LastName", newAuthor.LastName),
                new SqlParameter("@Birthdate", newAuthor.Birthdate)
            ]);

            try
            {
                if (insertAuthor.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine($"Succesfully created new author {newAuthor.GetFullName()}!");
                }
                else
                {
                    Console.WriteLine("The program ran into an issue while creating the new author###");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting record into database: {ex.Message}, please contact your administrator, or check the log file for more information");
                ErrorLogger.LogError(ex);
            }
        }
        Update();
    }

    public void InsertNewCategory(Category newCategory)
    {
        using (SqlConnection sqlConnection = new(connectionString))
        {
            try
            {
                sqlConnection.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connection to database: {ex.Message}, please check the configuration file and try again");
                ErrorLogger.LogError(ex);
            }

            SqlCommand insertCategory = new("INSERT INTO categories(category_name) VALUES(@CategoryName)", sqlConnection);
            insertCategory.Parameters.Add(new SqlParameter("@CategoryName", newCategory.Name));

            try
            {
                if (insertCategory.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine($"Succesfully created new category {newCategory.Name}!");
                }
                else
                {
                    Console.WriteLine("The program ran into an issue while creating the new category###");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting record into database: {ex.Message}, please contact your administrator, or check the log file for more information");
                ErrorLogger.LogError(ex);
            }
        }
        Update();
    }

    public void UpdateBook(Book bookToEdit, Book newBook)
    {
        using (SqlConnection sqlConnection = new(connectionString))
        {
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connection to database: {ex.Message}, please check the configuration file and try again");
                ErrorLogger.LogError(ex);
            }

            SqlCommand insertBook = new("UPDATE books SET title = @Title, category_id = @CategoryID, author_id = @AuthorID WHERE book_id = @BookToEditID", sqlConnection);
            insertBook.Parameters.AddRange(
            [
                new SqlParameter("@Title", newBook.Title),
                new SqlParameter("@CategoryID", newBook.Category.Id),
                new SqlParameter("@AuthorID", newBook.Author.Id),
                new SqlParameter("@BookToEditID", bookToEdit.Id)
            ]);

            try
            {
                if (insertBook.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine($"Succesfully updated book [{bookToEdit.Title}] to book [{newBook.Title}]!");
                }
                else
                {
                    Console.WriteLine("The program ran into an issue while updating the new book###");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting record into database: {ex.Message}, please contact your administrator, or check the log file for more information");
                ErrorLogger.LogError(ex);
            }
        }
        Update();
    }

    public void DeleteBook(Book bookToRemove)
    {
        using (SqlConnection sqlConnection = new(connectionString))
        {
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connection to database: {ex.Message}, please check the configuration file and try again");
                ErrorLogger.LogError(ex);
            }

            SqlCommand insertBook = new("DELETE FROM books WHERE book_id = @ID", sqlConnection);
            insertBook.Parameters.Add(new SqlParameter("@ID", bookToRemove.Id));

            try
            {
                if (insertBook.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine($"Succesfully removed book [{bookToRemove.Title}]!");
                }
                else
                {
                    Console.WriteLine("The program ran into an issue while removing the book###");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while removing record from database: {ex.Message}, please contact your administrator, or check the log file for more information");
                ErrorLogger.LogError(ex);
            }
        }
        Update();
    }
}

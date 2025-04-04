using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookRegistry.Classes;
using Microsoft.Data.SqlClient;

namespace BookRegistry
{
    public class DatabaseHandler
    {
        public List<Book> Books = [];
        public List<Author> Authors = [];
        public List<Category> Categories = [];

        private string connectionString = "";

        //public List<Category> GetCategories()
        //{
        //    return Categories;
        //}

        public void Initialize()
        {
            Books = [];
            Authors = [];
            Categories = [];

            try
            {
                connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving database connection details from configuration file: [{ex.Message}], please check the configuration file and try again");
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
                    Environment.Exit(1);
                }

                SqlCommand selectAuthorsCommand = new("SELECT TOP(100) author_id, first_name, last_name, birthdate FROM authors", sqlConnection);
                SqlCommand selectCategoriesCommand = new("SELECT TOP(100) category_id, category_name FROM categories", sqlConnection);
                SqlCommand selectBooksCommand = new("SELECT TOP(100) book_id, title, category_id, author_id FROM books", sqlConnection);

                try
                {
                    SqlDataReader reader = selectAuthorsCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Author newAuthor = new(int.Parse(reader["author_id"].ToString()), reader["first_name"].ToString(), reader["last_name"].ToString(), DateOnly.FromDateTime((DateTime)reader["birthdate"]));
                        Authors.Add(newAuthor);
                    }
                    reader.Close();
                    reader = selectCategoriesCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Category newCategory = new(int.Parse(reader["category_id"].ToString()), reader["category_name"].ToString());
                        Categories.Add(newCategory);
                    }
                    reader.Close();
                    reader = selectBooksCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        int authorId = int.Parse(reader["author_id"].ToString());
                        int categoryId = int.Parse(reader["category_id"].ToString());
                        Category bookCategory = Categories.Find(cat => cat.Id == categoryId);
                        Author bookAuthor = Authors.Find(auth => auth.Id == authorId);

                        Book newBook = new(int.Parse(reader["book_id"].ToString()), reader["title"].ToString(), bookCategory, bookAuthor);
                        Books.Add(newBook);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while fetching data from database: {ex}, please contact your administrator, or check the log file for more information");
                }

                sqlConnection.Close();
            }
        }

        public void Update()
        {
            Initialize();
        }

        public void InsertNewBook(Book newBook)//since all three methods are similar, maybe merge into one with a few input params?
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
                }

                SqlCommand insertBook = new("INSERT INTO books(title,category_id,author_id,date_added) VALUES(@Title, @CategoryID, @AuthorID, GETDATE())", sqlConnection);
                insertBook.Parameters.Add(new SqlParameter("Title", newBook.Title));
                insertBook.Parameters.Add(new SqlParameter("CategoryID", newBook.Category.Id));
                insertBook.Parameters.Add(new SqlParameter("AuthorID", newBook.Author.Id));

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
                }
                string query = $"INSERT INTO authors(first_name,last_name,birthdate) VALUES('{newAuthor.Name}','{newAuthor.LastName}','{newAuthor.Birthdate.ToString("MM-dd-yyyy")}')";
                SqlCommand insertBook = new(query, sqlConnection);

                try
                {
                    if (insertBook.ExecuteNonQuery() > 0)
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
                }
                string query = $"INSERT INTO categories(category_name) VALUES('{newCategory.Name}')";
                SqlCommand insertBook = new(query, sqlConnection);

                try
                {
                    if (insertBook.ExecuteNonQuery() > 0)
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
                }

                SqlCommand insertBook = new("UPDATE books SET title = 'UPDATED', category_id = 3, author_id = 12 WHERE book_id = 19", sqlConnection);
                insertBook.Parameters.Add(new SqlParameter("Title", newBook.Title));
                insertBook.Parameters.Add(new SqlParameter("CategoryID", newBook.Category.Id));
                insertBook.Parameters.Add(new SqlParameter("AuthorID", newBook.Author.Id));

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
                }
            }
            Update();
        }

        public void RemoveBook(Book bookToRemove)//change to int-id?
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
                }
                string query = $"DELETE FROM books WHERE book_id = {bookToRemove.Id}";
                SqlCommand insertBook = new(query, sqlConnection);

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
                }
            }
            Update();
        }
    }
}

namespace BookRegistry.Classes;

public class Book(int id, string title, Category category, Author author)//a class representing a single book
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public Category Category { get; set; } = category;
    public Author Author { get; set; } = author;
}

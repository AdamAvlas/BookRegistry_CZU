namespace BookRegistry.Classes;

public class Category(int id, string name)//a class representing a single category
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
}

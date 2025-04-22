namespace BookRegistry.Classes;

public class Author(int id, string first_name, string lastName, DateOnly birthdate)//a class representing a single author
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = first_name;
    public string LastName { get; set; } = lastName;
    public DateOnly Birthdate { get; set; } = birthdate;
    
    public string GetFullName()//returns the entire name as one string
    {
        return $"{Name} {LastName}";
    }
}

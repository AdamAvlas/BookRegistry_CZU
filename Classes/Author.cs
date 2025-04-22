using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRegistry.Classes;

public class Author(int id, string first_name, string lastName, DateOnly birthdate)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = first_name;
    public string LastName { get; set; } = lastName;
    public DateOnly Birthdate { get; set; } = birthdate;
    
    public string GetFullName()
    {
        return $"{Name} {LastName}";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRegistry.Classes;

public class Category(int id, string name)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
}

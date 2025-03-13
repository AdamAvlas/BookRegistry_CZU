using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRegistry.Classes
{
    public class Book(int id, string title, Category category, Author author)
    {
        public int Id { get; set; } = id;
        public string Title { get; set; } = title;
        public Category Category { get; set; } = category;
        public Author Author { get; set; } = author;
    }
}

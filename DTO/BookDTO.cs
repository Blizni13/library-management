using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management.DTO
{
    public class BookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public BookDTO(string title, string author)
        {
            Title = title;
            Author = author;
        }
        public override string ToString()
        {
            return $"{Title} by {Author}";
        }
    }
}

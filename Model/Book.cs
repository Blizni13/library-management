using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management.Model
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publication { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Book(int id, string title, string author, string publication, decimal price, int quantity)
        {
            ID = id;
            Title = title;
            Author = author;
            Publication = publication;
            Price = price;
            Quantity = quantity;
        }
    }

}

using library_management.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace library_management.View
{
    /// <summary>
    /// Interaction logic for UpdateBook.xaml
    /// </summary>
    public partial class ChangeBookDetails : Window
    {
        public ChangeBookDetails()
        {
            InitializeComponent();
        }

        public ChangeBookDetails(int id, string title, string author, string publication, decimal price, int quantity)
        {
            InitializeComponent();
            var result = (DataContext as ChangeBookDetailsViewModel);
            result.Id = id;
            result.Title = title;
            result.Author = author;
            result.Publication = publication;
            result.Price = price;
            result.Quantity = quantity;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

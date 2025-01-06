using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace library_management.ViewModel
{
    public class ChangeBookDetailsViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _author;
        private string _publication;
        private decimal _price;
        private int _quantity;

        public int Id { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        public string Publication
        {
            get => _publication;
            set
            {
                _publication = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ChangeBookDetailsViewModel()
        {
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Update()
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = @"UPDATE Books 
                                 SET title = @title, 
                                    author = @author, 
                                    publication = @publication, 
                                    price = @price, 
                                    quantity = @quantity 
                                 WHERE id = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@title", Title);
                command.Parameters.AddWithValue("@author", Author);
                command.Parameters.AddWithValue("@publication", Publication);
                command.Parameters.AddWithValue("@price", Price);
                command.Parameters.AddWithValue("@quantity", Quantity);
                command.Parameters.AddWithValue("@id", Id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Book updated successfully.");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("No book found with the given id.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured: {ex.Message}");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }

        public void Delete()
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = @"DELETE FROM Books 
                                 WHERE id = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", Id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Book deleted successfully.");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("No book found with the given id.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured: {ex.Message}");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }
    }
}

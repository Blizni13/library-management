using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.XPath;

namespace library_management.ViewModel
{
    public class ChangeReaderDetailsViewModel : INotifyPropertyChanged
    {
        private readonly string _dateFormat;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;
        private string _gender;
        private string _address;
        private DateTime _membershipStartDate;

        public int ReaderId { get; set; }
        
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth.Date;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }
        
        public DateTime MembershipStartDate
        {
            get => _membershipStartDate.Date;
            set
            {
                _membershipStartDate = value;
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
        public ChangeReaderDetailsViewModel()
        {
            _dateFormat = "yyyy-MM-dd";
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Update()
        {
            if (!ValidateFields())
                return;
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = @"UPDATE Readers 
                                 SET FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    DateOfBirth = @DateOfBirth, 
                                    Gender = @Gender, 
                                    Address = @Address,
                                    MembershipStartDate = @MembershipStartDate
                                 WHERE ReaderId = @ReaderId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth.ToString(_dateFormat));
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@MembershipStartDate", MembershipStartDate.ToString(_dateFormat));
                command.Parameters.AddWithValue("@ReaderId", ReaderId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Reader updated successfully.");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("No reader found with the given id.");
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

        private bool ValidateFields()
        {
            bool result = false;
            if (DateOfBirth > DateTime.Now)
                MessageBoxService.ShowErrorBox("Date of birth cannot be in future.");
            else if (MembershipStartDate > DateTime.Now)
                MessageBoxService.ShowErrorBox("Membership start date cannot be in future.");
            else
                result = true;

            return result;

        }

        public void Delete()
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = @"DELETE FROM Readers
                                 WHERE ReaderId = @ReaderId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReaderId", ReaderId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Reader deleted successfully.");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("No reader found with the given id.");
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

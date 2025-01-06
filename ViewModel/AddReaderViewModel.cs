using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace library_management.ViewModel
{
    internal class AddReaderViewModel
    {
        private readonly string _dateFormat;
        private string _firstName;
        private string _lastName;
        private string _gender;
        private string _address;
        private DateTime _dateOfBirth;
        private DateTime _membershipStartDate;

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

        public string Gender
        {
            get {
                return String.IsNullOrEmpty(_gender) ? "" : _gender.Substring(_gender.LastIndexOf(":") + 2);
            }
            set
            {
                _gender = value;
                OnPropertyChanged();
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

        public DateTime DateOfBirth
        {
            get => _dateOfBirth.Date;
            set
            {
                _dateOfBirth = value;
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

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly DashboardViewModel _parentViewModel;
        public AddReaderViewModel(DashboardViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            _dateFormat = "yyyy-MM-dd";
            _dateOfBirth = DateTime.Now;
            _membershipStartDate = DateTime.Now;

            // Commands initialization
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
        }
        public void Add()
        {
            if (!ValidateFields(out string errorMessage))
            {
                MessageBoxService.ShowErrorBox(errorMessage);
                return;
            }
            // check if this reader wasn't already added
            if (DBOperationService.CheckRowExistence(
                    "Readers",
                    [
                        ("FirstName", FirstName),
                        ("LastName", LastName),
                        ("DateOfBirth", DateOfBirth.ToString(_dateFormat)),
                        ("Gender", Gender),
                        ("Address", Address),
                        ("MembershipStartDate", MembershipStartDate.ToString(_dateFormat))
                    ]
                ))
            {
                MessageBoxService.ShowErrorBox("This reader was already added.");
                return;
            }

            // add book
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query
                  = "INSERT INTO Readers (FirstName, LastName, DateOfBirth, Gender, Address, MembershipStartDate) " +
                    "VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Address, @MembershipStartDate)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth.ToString(_dateFormat));
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@MembershipStartDate", MembershipStartDate.ToString(_dateFormat));

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Reader added successfully");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("There was an issue connecting to the database. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured: {ex.Message}", "Error");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }

        public void Cancel()
        {
            _parentViewModel.CloseAddReader();
        }
        private bool ValidateFields(out string errorMessage)
        {
            errorMessage = String.Empty;

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                errorMessage = "First Name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                errorMessage = "Last Name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Gender))
            {
                errorMessage = "Gender is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                errorMessage = "Address is required.";
                return false;
            }

            if (DateOfBirth == default)
            {
                errorMessage = "Date of Birth is required.";
                return false;
            }

            if (MembershipStartDate == default)
            {
                errorMessage = "Membership Start Date is required.";
                return false;
            }

            if (!ValidateDateOfBirth(out errorMessage))
                return false;

            if (MembershipStartDate > DateTime.Now)
            {
                errorMessage = "Membership Start Date cannot be in the future.";
                return false;
            }

            return true; // All fields are valid
        }

        private bool ValidateDateOfBirth(out string errorMessage)
        {
            if (DateOfBirth > DateTime.Now)
            {
                errorMessage = "Date of Birth cannot be in the future.";
                return false;
            }

            int age = CalculateAge(DateOfBirth);

            if (age > 120)
            {
                errorMessage = "The age cannot be more than 120 years.";
                return false;
            }
            else
            {
                errorMessage = string.Empty;
                return true;
            }
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            return age;
        }
    }
}

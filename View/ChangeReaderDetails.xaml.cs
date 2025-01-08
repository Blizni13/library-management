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
    /// Interaction logic for ChangeReaderDetails.xaml
    /// </summary>
    public partial class ChangeReaderDetails : Window
    {
        public ChangeReaderDetails()
        {
            InitializeComponent();
        }

        public ChangeReaderDetails(
            int readerId, 
            string firstName, 
            string lastName, 
            DateTime dateOfBirth, 
            string gender, 
            string address,
            DateTime membershipStartDate)
        {
            InitializeComponent();
            var result = (DataContext as ChangeReaderDetailsViewModel);
            result.ReaderId = readerId;
            result.FirstName = firstName;
            result.LastName = lastName;
            result.DateOfBirth = dateOfBirth;
            result.Gender = gender;
            result.Address = address;
            result.MembershipStartDate = membershipStartDate;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

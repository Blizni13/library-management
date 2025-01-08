using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management.Model
{
    public class Reader
    {
        public int ReaderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public Reader(
            int readerId, 
            string firstName, 
            string lastName, 
            DateTime dateOfBirth, 
            string gender, 
            string address, 
            DateTime membershipStartDate
        )
        {
            ReaderId = readerId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            MembershipStartDate = membershipStartDate;
        }
    }
}

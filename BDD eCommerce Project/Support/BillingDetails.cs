using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_eCommerce_Project.Support {
    internal class BillingDetails {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public BillingDetails(string firstName, string lastName, string streetName, string city, string postcode, string phoneNumber, string email) {
            FirstName = firstName;
            LastName = lastName;
            StreetName = streetName;
            City = city;
            Postcode = postcode;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}

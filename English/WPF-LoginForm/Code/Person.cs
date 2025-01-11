using System;

namespace BankSystem
{
    public class Person
    {
        public  string FullName { get; set; }
        public  string NationalID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public  string ContactNumber { get; set; }
    }

    public enum UserType
    {
        Bank,
        User
    }
}

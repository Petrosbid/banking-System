using System;
using System.Collections.Generic;

namespace BankSystem
{
    public class Account : Person
    {
        public List<Transaction> Transactions;
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Enable { get; set; }
        public int Number_Transactions { get; set; }
        public List<string> CardNumbers { get; set; }

        public double Balance {  get; set; }

        public Account() : base()
        {
            CardNumbers = new List<string>();
        }
    }
}

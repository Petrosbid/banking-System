namespace BankSystem
{
    public class Transaction
    {
        public string TransactionID { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public string TransactionDate { get; set; }
        public string AccountNumber { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace BankSystem
{

    public class Bankers
    {
        public string BankName { get; set; }
        public string Location { get; set; }
        public int Number_accounts { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Enable { get; set; }

        public List<Account> Accounts { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Bankers()
        {
            Accounts = new List<Account>();
            Transactions = new List<Transaction>();
        }

        private string GenerateUniqueAccountNumber()
        {
            Random random = new Random();
            string accountNumber;
            do
            {
                accountNumber = random.Next(100000, 999999).ToString();
            } while (Accounts.Any(a => a.AccountNumber == accountNumber));

            return accountNumber;
        }

        // Generate IBAN based on the account number
        private string GenerateIBAN(string accountNumber)
        {
            Random random = new Random();
            string rearranged = "IR" + "98" + accountNumber + random.Next(0, 10).ToString();
            return rearranged;
        }

        static string GetPersianDateTime(DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return $"{persianCalendar.GetYear(dateTime):0000}/{persianCalendar.GetMonth(dateTime):00}/{persianCalendar.GetDayOfMonth(dateTime):00} " +
                   $"{dateTime:HH:mm:ss}";
        }

        // ثبت نام کاربر جدید
        public void AddAccount(Account account)
        {
            string accountNumber = GenerateUniqueAccountNumber();
            string iban = GenerateIBAN(accountNumber);
            account.AccountNumber = accountNumber;
            account.IBAN = iban;
            account.Number_Transactions = 0;
            Accounts.Add(account);
            SaveToJson();
        }

        // حذف حساب کاربر
        public void DeleteAccount(Account account)
        {
            if (account != null)
            {
                Accounts.Remove(account);
                SaveToJson();
            }
        }

        // انتقال وجه
        public int Transfer(Account account, string toAccountNumber, double amount)
        {
            if (account != null)
            {
                var toAccount = Accounts.FirstOrDefault(a => a.AccountNumber == toAccountNumber);
                if (toAccount != null)
                {
                    if (account.Balance - amount - (amount * 0.01) >= 10000)
                    {
                        account.Balance -= amount + (amount * 0.01);
                        toAccount.Balance += amount;
                        if (account.Transactions == null)
                        {
                            account.Transactions = new List<Transaction>();
                        }

                        var transaction = new Transaction
                        {
                            TransactionID = "00" + (account.Number_Transactions).ToString("D3"),
                            TransactionType = "انتقال",
                            Amount = amount,
                            TransactionDate = GetPersianDateTime(DateTime.Now),

                            AccountNumber = account.AccountNumber
                        };

                        account.Transactions.Add(transaction);
                        account.Number_Transactions++;
                        SaveToJson();
                        return 1;
                    }
                    return 0;
                }
                return -1;
            }
            return -2;
        }

        // گرفتن موجودی
        public double GetBalance(Account account)
        {
            if (account != null && account.Balance - 750 >= 10000)
            {
                account.Balance -= 750;
                if (account.Transactions == null)
                {
                    account.Transactions = new List<Transaction>();
                }

                var transaction = new Transaction
                {
                    TransactionID = "30" + (account.Number_Transactions).ToString("D3"),
                    TransactionType = "موجودی",
                    Amount = 750,
                    TransactionDate = GetPersianDateTime(DateTime.Now),
                    AccountNumber = account.AccountNumber
                };

                account.Transactions.Add(transaction);
                account.Number_Transactions++;
                SaveToJson();
                return account.Balance;
            }
            return -1; // Error code
        }

        // برداشت وجه (خرید)
        public int Withdraw(Account account, double amount)
        {
            if (account != null && account.Balance - amount >= 10000)
            {
                account.Balance -= amount;
                if (account.Transactions == null)
                {
                    account.Transactions = new List<Transaction>();
                }

                var transaction = new Transaction
                {
                    TransactionID = "20" + (account.Number_Transactions).ToString("D3"),
                    TransactionType = "خرید",
                    Amount = amount,
                    TransactionDate = GetPersianDateTime(DateTime.Now),
                    AccountNumber = account.AccountNumber
                };

                account.Transactions.Add(transaction);
                account.Number_Transactions++;
                SaveToJson();
                return 1;
            }
            return 0;
        }

        // واریز به حساب
        public void Deposit(Account account, double amount)
        {
            if (account != null)
            {
                account.Balance += amount;
                if (account.Transactions == null)
                {
                    account.Transactions = new List<Transaction>();
                }

                var transaction = new Transaction
                {
                    TransactionID = "10" + (account.Number_Transactions).ToString("D3"),
                    TransactionType = "واریز",
                    Amount = amount,
                    TransactionDate = GetPersianDateTime(DateTime.Now),
                    AccountNumber = account.AccountNumber
                };

                account.Transactions.Add(transaction);
                account.Number_Transactions++;
                SaveToJson();
            }
        }

        // جستجو بین تمامی تراکنش‌های یک کاربر
        public List<Transaction> searchTransactionsByUser(Account account, string search)
        {
            if (account != null && account.Transactions != null && !string.IsNullOrWhiteSpace(search))
            {
                return account.Transactions
                    .Where(at => at.TransactionID.ToString().Equals(search))
                    .ToList();
            }
            return new List<Transaction>();
        }

        // ذخیره اطلاعات در فایل JSON
        public void SaveToJson()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText($"{BankName}.json", json);
        }

        // بازیابی اطلاعات از فایل JSON
        public void LoadFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var bank = JsonConvert.DeserializeObject<Bankers>(json);
                this.BankName = bank.BankName;
                this.Accounts = bank.Accounts;
                this.Transactions = bank.Transactions;
            }
        }

        public void DeleteBank()
        {
            string filePath = $"{BankName}.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static ObservableCollection<Bankers> LoadBanks()
        {
            var Banks = new ObservableCollection<Bankers>();
            string[] bankFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.json");
            foreach (var file in bankFiles)
            {
                string json = File.ReadAllText(file);
                var bank = JsonConvert.DeserializeObject<Bankers>(json);
                Banks.Add(bank);
            }
            return Banks;
        }

        public string GenerateCardNumber(Account account)
        {
            if (account.CardNumbers.Count < 5)
            {
                Random random = new Random();
                string cardNumber = string.Join("", Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));
                account.CardNumbers.Add(string.Join("-", Enumerable.Range(0, 4).Select(i => cardNumber.Substring(i * 4, 4))));
                SaveToJson();
                return string.Join("-", Enumerable.Range(0, 4).Select(i => cardNumber.Substring(i * 4, 4)));
            }
            return "شما به سقف تعداد کارت برای یک حساب رسیدید.";
        }


    }

}

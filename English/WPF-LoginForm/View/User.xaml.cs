using System;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.View
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        private Account targetaccount;
        private Bankers bank;
        public User(Account account, Bankers bank)
        {
            InitializeComponent();
            targetaccount = account;
            name_account.Text = account.FullName;
            acc_ID.Text = account.AccountNumber;
            nationalID.Text = account.NationalID;
            balancetxt.Text = account.Balance.ToString();
            userlable.Text = account.Username;
            Listtransactions.ItemsSource = account.Transactions;
            numtransaction.Text = account.Number_Transactions.ToString();
            this.bank = bank;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            LoginView loginView = new LoginView();
            loginView.Show();
        }

        private void getbalance_Click(object sender, RoutedEventArgs e)
        {
            double balance = bank.GetBalance(targetaccount);
            if (balance != -1)
            {
                MessageBox.Show($"transaction completed successfully.\bank fee: 750\nBalance: {targetaccount.Balance}");
                targetaccount.Number_Transactions++;
                balancetxt.Text = targetaccount.Balance.ToString();
                numtransaction.Text = targetaccount.Number_Transactions.ToString();
                Listtransactions.ItemsSource = null;
                Listtransactions.ItemsSource = targetaccount.Transactions;

            }
            else
            {
                MessageBox.Show($"Your balance is not enough to complete this transaction");
            }
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(depositcount.Text))
            {
                bank.Deposit(targetaccount, double.Parse(depositcount.Text));
                MessageBox.Show($"transaction completed successfully.\bank fee: 0\nBalance: {targetaccount.Balance}");
                targetaccount.Number_Transactions++;
                balancetxt.Text = targetaccount.Balance.ToString();
                numtransaction.Text = targetaccount.Number_Transactions.ToString();
                Listtransactions.ItemsSource = null;
                Listtransactions.ItemsSource = targetaccount.Transactions;
            }
            else
            {
                MessageBox.Show($"Please specify the amount.");

            }
        }

        private void Transfer_money_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(moneytransfer.Text) && !string.IsNullOrEmpty(targettansfer.Text))
            {
                int transfer = bank.Transfer(targetaccount, targettansfer.Text, double.Parse(moneytransfer.Text));
                if (transfer == 1)
                {
                    MessageBox.Show($"transaction completed successfully.\nBalance: {targetaccount.Balance}");
                    targetaccount.Number_Transactions++;
                    balancetxt.Text = targetaccount.Balance.ToString();
                    numtransaction.Text = targetaccount.Number_Transactions.ToString();
                    Listtransactions.ItemsSource = null;
                    Listtransactions.ItemsSource = targetaccount.Transactions;
                }
                else if (transfer == 0)
                {
                    MessageBox.Show($"Your balance is not enough to complete this transaction");
                }
                else
                {
                    MessageBox.Show($"No user found with this account number.");
                }
            }
        }

        private void Withdraw_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(withdrawcount.Text))
            {
                int withdraw = bank.Withdraw(targetaccount, double.Parse(withdrawcount.Text));
                if (withdraw == 1)
                {
                    MessageBox.Show($"transaction completed successfully.\bank fee: 0\nBalance: {targetaccount.Balance}");

                    targetaccount.Number_Transactions++;
                    balancetxt.Text = targetaccount.Balance.ToString();
                    numtransaction.Text = targetaccount.Number_Transactions.ToString();
                    Listtransactions.ItemsSource = null;
                    Listtransactions.ItemsSource = targetaccount.Transactions;
                }
                else
                {
                    MessageBox.Show($"Your balance is not enough to complete this transaction");

                }
            }
            else
            {
                MessageBox.Show($"No user found with this account number.");

            }
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            var searchtransactions = bank.searchTransactionsByUser(targetaccount, search.Text);
            Listtransactions.ItemsSource = null;
            Listtransactions.ItemsSource = searchtransactions;

        }

        private void Show_Full_informations_Click(object sender, RoutedEventArgs e)
        {
            string cardNumbersFormatted = string.Join("\n", targetaccount.CardNumbers);
            MessageBox.Show($"Full name: {targetaccount.FullName}\nNational ID: {targetaccount.NationalID}\nDate of birth: {targetaccount.DateOfBirth}\n" +
                $"Contact number: {targetaccount.ContactNumber}\nAccount number: {targetaccount.AccountNumber}\nIBAN: {targetaccount.IBAN}\n" +
                $"Number of cards: {targetaccount.CardNumbers.Count}\nnumber of transactions: {targetaccount.Number_Transactions.ToString()}\nBalance: {targetaccount.Balance}\nCards:\n {cardNumbersFormatted}");
        }

        private void addcard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(bank.GenerateCardNumber(targetaccount));
        }
    }
}

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
                MessageBox.Show($"تراکنش با موفقیت انجام شد.\nکارمزد: 750\nموجودی حساب شما: {targetaccount.Balance}");
                targetaccount.Number_Transactions++;
                balancetxt.Text = targetaccount.Balance.ToString();
                numtransaction.Text = targetaccount.Number_Transactions.ToString();
                Listtransactions.ItemsSource = null;
                Listtransactions.ItemsSource = targetaccount.Transactions;

            }
            else
            {
                MessageBox.Show($"موجودی شما برای انجام این تراکنش کافی نمیباشد");
            }
        }

        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(depositcount.Text))
            {
                bank.Deposit(targetaccount, double.Parse(depositcount.Text));
                MessageBox.Show($"تراکنش با موفقیت انجام شد.\nکارمزد: 0\nموجودی حساب شما: {targetaccount.Balance}");
                targetaccount.Number_Transactions++;
                balancetxt.Text = targetaccount.Balance.ToString();
                numtransaction.Text = targetaccount.Number_Transactions.ToString();
                Listtransactions.ItemsSource = null;
                Listtransactions.ItemsSource = targetaccount.Transactions;
            }
            else
            {
                MessageBox.Show($"لطفا مقدار را تعیین کنید.");

            }
        }

        private void Transfer_money_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(moneytransfer.Text) && !string.IsNullOrEmpty(targettansfer.Text))
            {
                int transfer = bank.Transfer(targetaccount, targettansfer.Text, double.Parse(moneytransfer.Text));
                if (transfer == 1)
                {
                    MessageBox.Show($"تراکنش با موفقیت انجام شد.\nموجودی حساب شما: {targetaccount.Balance}");
                    targetaccount.Number_Transactions++;
                    balancetxt.Text = targetaccount.Balance.ToString();
                    numtransaction.Text = targetaccount.Number_Transactions.ToString();
                    Listtransactions.ItemsSource = null;
                    Listtransactions.ItemsSource = targetaccount.Transactions;
                }
                else if (transfer == 0)
                {

                    MessageBox.Show($"موجودی شما برای انجام این تراکنش کافی نمیباشد");


                }
                else
                {
                    MessageBox.Show($"کاربری با این شماره حساب یافت نشد.");
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
                    MessageBox.Show($"تراکنش با موفقیت انجام شد.\nکارمزد: 0\nموجودی حساب شما: {targetaccount.Balance}");
                    targetaccount.Number_Transactions++;
                    balancetxt.Text = targetaccount.Balance.ToString();
                    numtransaction.Text = targetaccount.Number_Transactions.ToString();
                    Listtransactions.ItemsSource = null;
                    Listtransactions.ItemsSource = targetaccount.Transactions;
                }
                else
                {
                    MessageBox.Show($"موجودی شما برای انجام این تراکنش کافی نمیباشد");
                }
            }
            else
            {
                MessageBox.Show($"لطفا مقدار را تعیین کنید.");
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
            MessageBox.Show($"نام و نام‌خانوادگی: {targetaccount.FullName}\nکد ملی: {targetaccount.NationalID}\n تاریخ تولد: {targetaccount.DateOfBirth}\n" +
                $"شماره تماس: {targetaccount.ContactNumber}\nشماره حساب: {targetaccount.AccountNumber}\n شماره شبا: {targetaccount.IBAN}\n" +
                $"تعداد کارت های موجود: {targetaccount.CardNumbers.Count}\nتعداد تراکنش ها: {targetaccount.Number_Transactions.ToString()}\nموجودی: {targetaccount.Balance}\nکارت های حساب:\n {cardNumbersFormatted}");
        }

        private void addcard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(bank.GenerateCardNumber(targetaccount));
        }
    }
}

using System.Windows;
using System.Windows.Input;

namespace BankSystem.View
{
    /// <summary>
    /// Interaction logic for Bank.xaml
    /// </summary>
    public partial class Bank : Window
    {
        Bankers bank = new Bankers();
        private Account Account { get; set; }
        private bool edit_account = false;
        public Bank(Bankers Bankk)
        {
            InitializeComponent();
            Title.Text = $"{Bankk.BankName}";
            bank = Bankk;
            ListAccounts.ItemsSource = bank.Accounts;
            loclable.Text = bank.Location;
            userlable.Text = bank.Username;
            numaccount.Text = bank.Number_accounts.ToString();
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

        private void enable_Click(object sender, RoutedEventArgs e)
        {
            Account.Enable = !Account.Enable;
            bank.SaveToJson();
            ListAccounts.ItemsSource = null;
            ListAccounts.ItemsSource = bank.Accounts;
        }

        private void btnedit_Click(object sender, RoutedEventArgs e)
        {
            edit_account = true;
            Full_name.Text = Account.FullName;
            national_ID.Text = Account.NationalID;
            birthdate.SelectedDate = Account.DateOfBirth;
            contactnumber.Text = Account.ContactNumber;
            balance.Text = Account.Balance.ToString();
            txtname.Text = Account.Username;
            txtloc.Text = Account.Password;


        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"آیا مطمئن هستید که می‌خواهید {Account.FullName} حذف کنید؟", "حذف حساب", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                bank.DeleteAccount(Account);
                bank.Number_accounts--;
                bank.SaveToJson();
                ListAccounts.ItemsSource = null;
                ListAccounts.ItemsSource = bank.Accounts;
            }
        }

        private void ListAccounts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Account = (Account)ListAccounts.SelectedItem;

        }

        private void Show_Full_informations_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"نام: {Account.FullName}\nکد ملی: {Account.NationalID}\nتاریخ تولد: {Account.DateOfBirth}\nشماره تماس: {Account.ContactNumber}\nIBAN: {Account.IBAN}\nموجودی: {Account.Balance}\nتعداد کارت ها: {Account.CardNumbers.Count}", "اطلاعات کامل حساب", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtloc.Text) || string.IsNullOrEmpty(txtname.Text) || string.IsNullOrEmpty(balance.Text) || string.IsNullOrEmpty(contactnumber.Text) || string.IsNullOrEmpty(national_ID.Text) || string.IsNullOrEmpty(Full_name.Text))
            {
                MessageBox.Show("لطفاً تمامی فیلد های مورد نیاز را وارد کنید.");
            }
            else
            {
                if (edit_account)
                {
                    Account.FullName = Full_name.Text; Account.NationalID = national_ID.Text; Account.DateOfBirth = (System.DateTime)birthdate.SelectedDate; Account.ContactNumber = contactnumber.Text; Account.Balance = double.Parse(balance.Text); Account.Username = txtname.Text; Account.Password = txtloc.Text;
                 
                    txtloc.Text = ""; txtname.Text = ""; balance.Text = ""; contactnumber.Text = ""; national_ID.Text = ""; Full_name.Text = ""; birthdate.Text = "";

                }
                else
                {
                    if (double.Parse(balance.Text) < 10000)
                    {
                        MessageBox.Show("حداقل موجودی باید 10,000 تومان باشد.");
                    }
                    else
                    {
                        Account account = new Account
                        {
                            FullName = Full_name.Text,
                            NationalID = national_ID.Text,
                            DateOfBirth = (System.DateTime)birthdate.SelectedDate,
                            ContactNumber = contactnumber.Text,
                            IBAN = txtloc.Text,
                            Balance = double.Parse(balance.Text),
                            CardNumbers = new System.Collections.Generic.List<string>(),
                            Username = txtname.Text,
                            Password = txtloc.Text,
                            Enable = true

                        };
                        bank.Number_accounts++;
                        bank.AddAccount(account);
                        MessageBox.Show($"حساب با موفقیت اضافه شد.\n شماره حساب: {account.AccountNumber} \n شماره شبا: {account.IBAN}");
                        ListAccounts.ItemsSource = null;
                        ListAccounts.ItemsSource = bank.Accounts;
                        txtloc.Text = ""; txtname.Text = ""; balance.Text = ""; contactnumber.Text = ""; national_ID.Text = ""; Full_name.Text = ""; birthdate.Text = "";

                    }
                }
                bank.SaveToJson();
                ListAccounts.ItemsSource = null;
                ListAccounts.ItemsSource = bank.Accounts;
            }
        }
    }
}

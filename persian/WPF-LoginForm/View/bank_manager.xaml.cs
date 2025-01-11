using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.View
{
    /// <summary>
    /// Interaction logic for bank_manager.xaml
    /// </summary>
    public partial class bank_manager : Window
    {
        private ObservableCollection<Bankers> Banks { get; set; }
        private Bankers selectedbank { get; set; }
        private bool edit_bank = false;
        public bank_manager()
        {
            InitializeComponent();
            Banks = Bankers.LoadBanks();
            ListBanks.ItemsSource = Banks;
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ListBanks.SelectedItem is Bankers selectedBank)
            {

                MessageBoxResult result = MessageBox.Show($"آیا مطمئن هستید که می‌خواهید {selectedBank.BankName} حذف کنید؟", "حذف بانک", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    selectedBank.DeleteBank();
                    Banks.Remove(selectedBank);
                    ListBanks.ItemsSource = Banks;
                }

            }
            else
            {
                MessageBox.Show("لطفاً یک بانک را انتخاب کنید.");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtloc.Text) || string.IsNullOrEmpty(txtname.Text) || string.IsNullOrEmpty(txtPass.Password) || string.IsNullOrEmpty(txtUser.Text))
            {
                MessageBox.Show("لطفاً تمامی فیلد های مورد نیاز را وارد کنید.");
            }
            else if (!edit_bank)
            {
                if (!Banks.Any(b => b.Username.Equals(txtUser.Text)))
                {
                    var newBank = new Bankers { BankName = txtname.Text, Location = txtloc.Text, Password = txtPass.Password, Username = txtUser.Text };
                    newBank.SaveToJson();
                    Banks.Add(newBank);
                    ListBanks.ItemsSource = Banks;
                }
                else
                {
                    MessageBox.Show("نام کاربری تکراری است.");
                }
            }
            else
            {
                selectedbank.BankName = txtname.Text;
                selectedbank.Username = txtUser.Text;
                selectedbank.Location = txtloc.Text;
                selectedbank.Password = txtPass.Password;
                selectedbank.Accounts = selectedbank.Accounts;
                selectedbank.Number_accounts = selectedbank.Number_accounts;
                selectedbank.Transactions = selectedbank.Transactions;
                selectedbank.SaveToJson();
                ListBanks.ItemsSource = null;
                ListBanks.ItemsSource = Banks;
                edit_bank = false;
            }
            txtloc.Text = "";
            txtname.Text = "";
            txtPass.Password = "";
            txtUser.Text = "";

        }


        private void btnedit_Click(object sender, RoutedEventArgs e)
        {
            if (ListBanks.SelectedItem is Bankers bank)
            {
                selectedbank = bank;
                edit_bank = true;
                txtloc.Text = bank.Location;
                txtname.Text = bank.BankName;
                txtPass.Password = bank.Password;
                txtUser.Text = bank.Username;
            }
            else
            {
                MessageBox.Show("لطفا یک بانک را انتخاب کنید.");

            }

        }

        private void enable_Click(object sender, RoutedEventArgs e)
        {
            if (ListBanks.SelectedItem is Bankers bank)
            {
                    bank.Enable = !bank.Enable;
                    bank.SaveToJson();
                    ListBanks.ItemsSource = null;
                    ListBanks.ItemsSource = Banks;
            }
            else
            {
                MessageBox.Show("لطفا یک بانک را انتخاب کنید.");

            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            LoginView loginView = new LoginView();
            loginView.Show();
        }

    }
}
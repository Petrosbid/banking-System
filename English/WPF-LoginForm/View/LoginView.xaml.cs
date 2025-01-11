using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private ObservableCollection<Bankers> Banks { get; set; }
        public LoginView()
        {
            InitializeComponent();
            Banks = Bankers.LoadBanks();
            banks.ItemsSource = Banks.Select(b => b.Username).ToList();

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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                emptypass.Visibility = Visibility.Hidden;
                emptypass.Text = "Please enter your username.";
                emptypass.Visibility = Visibility.Visible;
            }
            else if (string.IsNullOrEmpty(txtPass.Password))
            {
                emptypass.Visibility = Visibility.Hidden;
                emptypass.Text = "Please enter your password.";
                emptypass.Visibility = Visibility.Visible;
            }
            else
            {
                string username = txtUser.Text;
                string password = txtPass.Password;
                if (username == "Centeral" && password == "12@34")
                {
                    emptypass.Visibility = Visibility.Hidden;
                    emptyLog.Text = "You entered as Centeral bank.";
                    emptyLog.Visibility = Visibility.Visible;

                    await Task.Delay(1000);
                    this.Hide();
                    bank_manager bank_Manager = new bank_manager();
                    bank_Manager.Show();
                }
                else
                {
                    // Ensure Banks is initialized
                    if (Banks != null && Banks.Any(b => b.Username.Equals(txtUser.Text)))
                    {
                        if (txtPass.Password == Banks.FirstOrDefault(b => b.Username.Equals(txtUser.Text)).Password)
                        {
                            Bankers bank = Banks.FirstOrDefault(b => b.Username.Equals(txtUser.Text));
                            if (bank.Enable)
                            {
                                emptypass.Visibility = Visibility.Hidden;
                                emptyLog.Text = "You entered as a bank.";
                                emptyLog.Visibility = Visibility.Visible;
                                await Task.Delay(1000);
                                this.Hide();
                                Bank searchbank = new Bank(Banks.FirstOrDefault(b => b.Username.Equals(txtUser.Text)));
                                searchbank.Show();
                            }
                            else
                            {
                                emptypass.Text = "The bank is diasable.";
                                emptypass.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            emptypass.Text = "username or password is wrong.";
                            emptypass.Visibility = Visibility.Visible;
                        }
                    }
                    else if (banks.SelectedItem != null)
                    {
                        var bank = Banks.FirstOrDefault(a => a.Username.Equals(banks.SelectedItem));
                        Account account = bank.Accounts.FirstOrDefault(b => b.Username.Equals(txtUser.Text));

                        if (account != null && !string.IsNullOrEmpty(txtPass.Password) && txtPass.Password.Equals(txtPass.Password))
                        {
                            emptypass.Visibility = Visibility.Hidden;
                            emptyLog.Text = "You entered as customer.";
                            emptyLog.Visibility = Visibility.Visible;
                            await Task.Delay(1000); 
                            this.Hide();
                            User user = new User(account, bank);
                            user.Show();
                        }
                        else
                        {
                            emptypass.Text = "username or password is wrong.";
                            emptypass.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        emptypass.Text = "Please choose a bank.";
                        emptypass.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}

using System.Windows;

namespace ApplicationManagementSystem
{
    public partial class MainWindow : Window
    {
        private string UserRole;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Text;
            DatabaseManager db = new DatabaseManager();
            bool isAuthenticated = db.AuthenticateUser(username, password);
            if (!isAuthenticated)
            {
                MessageBox.Show("Неправильный username или пароль");
            }
            else
            {
                UserRole = db.GetUserRoleByUsername(username);
                MainWindowApplication mw = new MainWindowApplication(UserRole);
                Close();
                mw.Show();
            }
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
        private void LoginAsGuest_Click(object sender, RoutedEventArgs e)
        {
            UserRole = "Гость";
            MainWindowApplication mw = new MainWindowApplication(UserRole);
            Close();
            mw.Show();
        }
    }
}

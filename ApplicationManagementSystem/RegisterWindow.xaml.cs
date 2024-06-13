using System.Windows;
using System.Windows.Controls;

namespace ApplicationManagementSystem
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Text;
            string role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            DatabaseManager db = new DatabaseManager();
            db.CreateUser(username, password, role);
            MessageBox.Show("Вы успешно зарегистрировались");
            this.Close();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
namespace ApplicationManagementSystem
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void EnrollStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(IdAbiturentTextBox.Text);
            string status = ((ComboBoxItem)ChangeStatusComboBox.SelectedItem).Content.ToString();
            string specialty = ((ComboBoxItem)SpecialtyComboBox.SelectedItem).Content.ToString();

            DatabaseManager db = new DatabaseManager();
            db.UpdateStatus(id, status);
            if (status == "Зачислено")
            {
                db.ReductionQuantityBudgetPlaces(specialty);
            }
            MessageBox.Show("Статус успешно изменен");
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string role = ((ComboBoxItem)AddUserComboBox.SelectedItem).Content.ToString();

            DatabaseManager db = new DatabaseManager();
            db.CreateUser(username, password, role);
            MessageBox.Show("Новый пользователь успешно добавлен");
        }

        private void CreateOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(IdAbiturentTextBox.Text);
            DatabaseManager db = new DatabaseManager();
            db.CreateOrder(id);
            MessageBox.Show("Приказ успешно сформирован");
        }

        private void UpdateResultExamBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(IdExamTextBox.Text);
            int results = int.Parse(ResultsTextBox.Text);

            DatabaseManager db = new DatabaseManager();
            db.UpdateExamResults(id, results);
            MessageBox.Show("Результаты успешно обновлены");
        }

        private void UpdateObserverBtn_Click(object sender, RoutedEventArgs e)
        {
            string observer = ((ComboBoxItem)ObserverComboBox.SelectedItem).Content.ToString();
            int id = int.Parse(IdExamTextBox.Text);

            DatabaseManager db = new DatabaseManager();
            db.UpdateExamObserver(id, observer);
            MessageBox.Show("Наблюдатель успешно установлен");
        }
    }
}

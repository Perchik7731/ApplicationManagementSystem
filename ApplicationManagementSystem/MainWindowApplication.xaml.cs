using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationManagementSystem
{
    public partial class MainWindowApplication : Window
    {
        private string userRole;
        public MainWindowApplication(string role)
        {
            userRole = role;
            LabelContent = $"Ваша роль: {role}";
            InitializeComponent();
            LoadApplicationsAndExams();
            DataContext = this;
        }
        public string LabelContent { get; set; }

        private void LoadApplicationsAndExams()
        {
            if (userRole == "Администратор" || userRole == "Редактор")
            {
                ApplicationsDataGrid.Visibility = Visibility.Visible;
                statementsLabel.Visibility = Visibility.Visible;
                ExamsDataGrid.Visibility = Visibility.Visible;
                DatabaseManager db = new DatabaseManager();
                List<Abiturent> abiturents = db.GetAllApplications();
                List<Examination> exams = db.GetAllExams();
                ApplicationsDataGrid.ItemsSource = abiturents;
                ExamsDataGrid.ItemsSource = exams;
                TotalScoreLabel.Content = db.CalculateTotalScore();
            }
            else
            {
                ExamsDataGrid.Visibility = Visibility.Collapsed;
                ApplicationsDataGrid.Visibility = Visibility.Collapsed;
                statementsLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void ApplyApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userRole == "Абитурент" || userRole == "Редактор" || userRole == "Администратор")
            {
                ApplyWindow aw = new ApplyWindow();
                aw.ShowDialog();
            }
            else
            {
                MessageBox.Show("Зарегистрируйтесь или Войдите");
            }
        }

        private void AdminWindowDtn_Click(object sender, RoutedEventArgs e)
        {
            if (userRole == "Администратор" || userRole == "Редактор")
            {
                AdminWindow aw = new AdminWindow();
                aw.Show();
            }
            else
            {
                MessageBox.Show("Вы не администратор");
            }
        }

        private void RegisterForExam_Click(object sender, RoutedEventArgs e)
        {
            if (userRole == "Гость")
            {
                MessageBox.Show("Войдите или зарегистрируйтесь");
            }
            else
            {
                string specialty = ((ComboBoxItem)SpecialtyComboBox.SelectedItem).Content.ToString();
                string fio = FullNameTextBox.Text;
                string datetime = ((ComboBoxItem)DateTimeComboBox.SelectedItem).Content.ToString();
                string results = "Не сданы";
                string observer = "Не назначен";

                DateTime examdatetime = DateTime.Parse(datetime);

                DatabaseManager db = new DatabaseManager();
                db.AddExam(specialty, fio, examdatetime, observer, results);
                MessageBox.Show("Успешно подана заявка на экзамены");
            }
            
        }

        private void UpdateDataBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadApplicationsAndExams();
        }
    }
}

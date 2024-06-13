using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationManagementSystem
{
    public partial class ApplyWindow : Window
    {
        public ApplyWindow()
        {
            InitializeComponent();
        }
        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            string FIO = FIOTextBox.Text;
            string Pasport = PasportTextBox.Text;
            string PhoneNumber = PhoneNumberTextBox.Text;
            string AverageGrade = AverageGradeTextBox.Text;
            string Specialty = ((ComboBoxItem)SpecialtyComboBox.SelectedItem).Content.ToString();
            string Status = "В обработке";
            string examsStatus = "Не сданы";

            if (!Regex.IsMatch(FIO, @"^[a-zA-Zа-яА-Я\s]+$"))
            {
                MessageBox.Show("Некорректное ФИО");
                return;
            }

            if (!Regex.IsMatch(Pasport, @"^\d+$"))
            {
                MessageBox.Show("Некорректный номер паспорта");
                return;
            }

            if (!Regex.IsMatch(PhoneNumber, @"^\+[0-9]{11}$"))
            {
                MessageBox.Show("Некорректный номер телефона");
                return;
            }

            if (double.TryParse(AverageGrade, out double averageScore))
            {
                MessageBox.Show("Некорректный средний балл");
                return;
            }

            DatabaseManager db = new DatabaseManager();
            db.AddAbiturent(FIO, Pasport, PhoneNumber, AverageGrade, Specialty, Status, examsStatus);
            db.ReductionQuantityBudgetPlaces(Specialty);
            MessageBox.Show("Заявление отправлено успешно");
            Close();
        }
    }
}

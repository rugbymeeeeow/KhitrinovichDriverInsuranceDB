using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DriverInsuranceDB.Pages
{

    public partial class AddPage : Page
    {
        private InsurancePolicies _currentPolicy;

        public AddPage(InsurancePolicies selectedPolicy)
        {
            InitializeComponent();

            // Инициализация объекта полиса
            _currentPolicy = selectedPolicy ?? new InsurancePolicies();

            // Загрузка списка страховых компаний, если необходимо
            ComboBoxInsuranceCompany.SelectedIndex = 0; 

            if (_currentPolicy.PolicyID != 0)
            {
                // Редактирование существующего полиса
                ComboBoxInsuranceCompany.SelectedItem = _currentPolicy.InsuranceCompany;
                TextBoxPolicyNumber.Text = _currentPolicy.PolicyNumber;
                DatePickerIssueDate.SelectedDate = _currentPolicy.IssueDate;
                DatePickerExpirationDate.SelectedDate = _currentPolicy.ExpirationDate;
                TextBoxCarNumber.Text = _currentPolicy.StateNumber;
                TextBoxCost.Text = _currentPolicy.Cost.ToString();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // Возврат на предыдущую страницу
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка заполненности полей
            if (ComboBoxInsuranceCompany.SelectedItem == null)
                errors.AppendLine("Выберите страховую компанию.");
            if (string.IsNullOrEmpty(TextBoxPolicyNumber.Text))
                errors.AppendLine("Укажите номер полиса.");
            if (DatePickerIssueDate.SelectedDate == null)
                errors.AppendLine("Укажите дату оформления.");
            if (DatePickerExpirationDate.SelectedDate == null)
                errors.AppendLine("Укажите срок действия.");
            if (string.IsNullOrEmpty(TextBoxCarNumber.Text))
                errors.AppendLine("Укажите номер автомобиля.");
            if (!decimal.TryParse(TextBoxCost.Text, out decimal cost) || cost <= 0)
                errors.AppendLine("Укажите корректную стоимость.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Заполнение объекта полиса
            _currentPolicy.InsuranceCompany = ComboBoxInsuranceCompany.SelectedItem.ToString();
            _currentPolicy.PolicyNumber = TextBoxPolicyNumber.Text;
            _currentPolicy.IssueDate = DatePickerIssueDate.SelectedDate.Value;
            _currentPolicy.ExpirationDate = DatePickerExpirationDate.SelectedDate.Value;
            _currentPolicy.StateNumber = TextBoxCarNumber.Text;
            _currentPolicy.Cost = cost;

            // Добавление или обновление записи в базе
            if (_currentPolicy.PolicyID == 0)
            {
                Entities.GetContext().InsurancePolicies.Add(_currentPolicy); // Для добавления нового полиса
            }

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }

}

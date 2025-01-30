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
    /// <summary>
    /// Логика взаимодействия для DBPage.xaml
    /// </summary>
    public partial class DBPage : Page
    {
        private CurrentUser currentUser;
        public DBPage(CurrentUser user)
        {
            InitializeComponent();
            currentUser = user;

            LoadTestResults();
        }
        private void LoadTestResults()
        {

            DataGridUser.ItemsSource = Entities.GetContext().InsurancePolicies.ToList();
            if (currentUser.Role == "Страховой агент")
            {
                ButtonAdd.Visibility = Visibility.Visible;
                ButtonDel.Visibility = Visibility.Visible;
            }
        }

        private void ButtonClearFilters_Click(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Clear();
            CmbSorting.SelectedIndex = 0;
            UpdateUsers();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void UpdateUsers()
        {
            if (DataGridUser == null)
            {
                return;
            }

            string searchText = TextBoxSearch.Text.ToLower();
            string selectedInsuranceCompany = (CmbSorting.SelectedItem as ComboBoxItem)?.Content.ToString();

            var currentInsurancePolicies = Entities.GetContext().InsurancePolicies.AsQueryable();

            // Фильтрация по поисковому тексту
            if (!string.IsNullOrEmpty(searchText))
            {
                currentInsurancePolicies = currentInsurancePolicies.Where(x => x.StateNumber.ToLower().Contains(searchText));
            }

            // Фильтрация по выбранной страховой компании
            if (selectedInsuranceCompany != "Все" && !string.IsNullOrEmpty(selectedInsuranceCompany))
            {
                currentInsurancePolicies = currentInsurancePolicies.Where(x => x.InsuranceCompany == selectedInsuranceCompany);
            }

            DataGridUser.ItemsSource = currentInsurancePolicies.ToList();
        }


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = DataGridUser.SelectedItems.Cast<dynamic>().ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну запись для удаления.");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {selectedItems.Count()} элементов?", "Внимание",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                foreach (var item in selectedItems)
                {
                    var testDate = (DateTime)item.TestDate;

                    var reportToRemove = Entities.GetContext().InsurancePolicies
                        .FirstOrDefault(r => r.IssueDate == testDate);

                    if (reportToRemove != null)
                    {
                        Entities.GetContext().InsurancePolicies.Remove(reportToRemove);
                    }
                }

                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно удалены!");

                LoadTestResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}");
            }
        }

        private void ClickButtonEdit(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridUser.SelectedItem as dynamic;

            if (selectedItem == null)
            {
                MessageBox.Show("Выберите запись для редактирования.");
                return;
            }

            var testDate = (DateTime)selectedItem.IssueDate;

            var reportToEdit = Entities.GetContext().InsurancePolicies
                .FirstOrDefault(r => r.IssueDate == testDate);

            if (reportToEdit != null)
            {
                NavigationService.Navigate(new AddPage(reportToEdit));
            }
            else
            {
                MessageBox.Show("Не удалось найти запись для редактирования.");
            }
        }
    
    }
}

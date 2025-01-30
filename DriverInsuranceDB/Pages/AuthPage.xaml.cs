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
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void RegButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }

        private void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
            }
            string password = PasswordBox.Password;
            using (var db = new Entities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Username == TextBoxLogin.Text && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Такой пользователь не найден! Попробуйте ещё раз ввести данные");
                    return;
                }
                else
                {
                    CurrentUser rol = new CurrentUser();

                    if (user.Role == "Водитель")
                    {
                        MessageBox.Show($"Вы успешно зашли под ролью водителя, {user.Username}");
                        rol.Role = "Водитель";
                        NavigationService.Navigate(new DBPage(rol));
                    }
                    if (user.Role == "Страховой агент")
                    {
                        MessageBox.Show($"Вы успешно зашли под ролью страхового агента, {user.Username}");
                        rol.Role = "Страховой агент";
                        NavigationService.Navigate(new DBPage(rol));
                    }

                }
            }
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (TextBoxLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }
    }
}


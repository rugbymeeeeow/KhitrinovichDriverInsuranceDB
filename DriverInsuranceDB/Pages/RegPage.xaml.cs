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
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();

        }
        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (LoginTextBox.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void RegButtonClick(object sender, RoutedEventArgs e)
        {
            using (var db = new Entities())

                if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Password) || string.IsNullOrEmpty(PasswordTextBoxAgain.Password))
                {
                    MessageBox.Show("Вы заполнили не все данные");
                }

            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Password) || string.IsNullOrEmpty(PasswordTextBoxAgain.Password))
            {
                MessageBox.Show("Вы заполнили не все данные");
            }
            if (PasswordTextBox.Password.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать 6 или более символов");
            }
            if (PasswordTextBox.Password != PasswordTextBoxAgain.Password)

            {
                MessageBox.Show("Пароли не совпадают");
            }
            else if (PasswordTextBox.Password == PasswordTextBoxAgain.Password)
            {
                bool en = true;
                bool number = false;
                for (int i = 0; i < PasswordTextBox.Password.Length; i++)
                {

                    if (PasswordTextBox.Password[i] >= 'А' && PasswordTextBox.Password[i] <= 'Я' || PasswordTextBox.Password[i] >= 'а' && PasswordTextBox.Password[i] <= 'я')
                    {

                        en = false;
                    }

                    if (PasswordTextBox.Password[i] >= '0' && PasswordTextBox.Password[i] <= '9')
                    {
                        number = true;
                    }
                }
                if (!en)
                {
                    MessageBox.Show("Доступна только английская раскладка");
                }
                else if (!number)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру");
                }
                else
                {
                    Entities db = new Entities();

                    bool ex = Entities.GetContext().Users.Any(u => u.Username == LoginTextBox.Text);
                    if (ex) { MessageBox.Show("Такой пользователь уже существует!"); }
                    else
                    {
                        Users userObject = new Users
                        {
                            Username = LoginTextBox.Text,
                            Password = PasswordTextBox.Password,
                            Role = ComboBoxRole.Text
                        };

                        db.Users.Add(userObject);
                        db.SaveChanges();
                        CurrentUser rol = new CurrentUser();
                        if (ComboBoxRole.SelectedIndex == 0)
                        {
                            MessageBox.Show("Вы успешно зарегистрировались!");
                            rol.Role = "Водитель";
                            NavigationService.Navigate(new DBPage(rol));
                        }
                        if (ComboBoxRole.SelectedIndex == 1)
                        {
                            MessageBox.Show("Вы успешно зарегистрировались!");
                            rol.Role = "Страховой агент";
                            NavigationService.Navigate(new DBPage(rol));

                        }
                    }
                }

            }
        }
    }
}

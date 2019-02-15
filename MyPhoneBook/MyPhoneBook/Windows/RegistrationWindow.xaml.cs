using MyPhoneBook.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyPhoneBook.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            email.Text = email.Text.Trim();
            password.Password = password.Password.Trim();
            password_confirm.Password = password_confirm.Password.Trim();

            bool emailIsCorrect = Validator.EmailCheck(email.Text);
            bool passwordIsCorrect = Validator.PassWordCheck(password.Password);
            bool passwordsIsEquals = password.Password == password_confirm.Password;

            StringBuilder str = new StringBuilder();
            if (!emailIsCorrect)
            {
                str.Append(Properties.Resources.IncorrectEmail);
                str.AppendLine();
            }
            if (!passwordIsCorrect)
            {
                str.Append(Properties.Resources.IncorrectPassword);
                str.AppendLine();

            }
            if (!passwordsIsEquals)
            {
                str.Append(Properties.Resources.DifferentPassword);
                str.AppendLine();
            } 
            info.Text = str.ToString();

            if (emailIsCorrect && passwordIsCorrect && passwordsIsEquals)
            {
                DialogResult = true;
            }
            else return;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

       
       
       
    }
}

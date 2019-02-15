using MyPhoneBook.Manager;
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
using System.Windows.Shapes;
using System.Xml;

namespace MyPhoneBook.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private EntitiesManager manager;

        public LoginWindow(EntitiesManager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            email.Text = email.Text.Trim();
            password.Password = password.Password.Trim();

            bool resultLogin = false;
           
                try
                {
                    resultLogin = manager.LoginUser(email.Text, password.Password);
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            if (resultLogin == true)
            {
                DialogResult = true;
            }
            else
            {
                info.Text = Properties.Resources.ErrorLogin;
                email.Text = string.Empty;
                password.Password = string.Empty;
            }
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            
            email.Text = string.Empty;
            password.Password = string.Empty;
            DialogResult = false;
        }
    }
}

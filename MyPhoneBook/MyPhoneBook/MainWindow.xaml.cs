using DataLibrary.Entities;
using DataLibrary.Enum;
using MyPhoneBook.Manager;
using MyPhoneBook.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Xml;

namespace MyPhoneBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        private EntitiesManager manager;
        public MainWindow()
        {
            InitializeComponent();
            manager = new EntitiesManager(DataProvider.XML);
            AddContact.IsEnabled = false;
            DeleteAllContact.IsEnabled = false;
        }

       

        private void Login_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.LoginStatus) return;

            LoginWindow loginWindow = new LoginWindow(manager);
            loginWindow.Owner = this;
            loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginWindow.ShowDialog();

           
            if (loginWindow.DialogResult == true)
            {
                manager.CurrentUser = new User { EmailAddress = loginWindow.email.Text };
                SuccessfulLogin();

            }
           


        }
        
        private void SuccessfulLogin()
        {
            Registration.Text = Properties.Resources.Logoff;
            Login.Text = String.Format(Properties.Resources.Hello, manager.CurrentUser.EmailAddress);
            Properties.Settings.Default.LoginStatus = true;
            AddContact.IsEnabled = true;
            DeleteAllContact.IsEnabled = true;
            
        }

        private void SuccessfulLogOff()
        {
            Registration.Text = Properties.Resources.Registration;
            Login.Text = Properties.Resources.Login;
            Properties.Settings.Default.LoginStatus = false;
            AddContact.IsEnabled = false;
            DeleteAllContact.IsEnabled = false;
            manager.Contacts.Clear();
            search_name.Text = string.Empty;
            search_data.Text = string.Empty;
        }

        private void Registration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.LoginStatus)
            {
                ConfirmWindow confirm = new ConfirmWindow();
                confirm.Owner = this;
                confirm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                confirm.Message.Text = Properties.Resources.ConfirmLogoff;

                confirm.ShowDialog();

                if(confirm.DialogResult == true)
                {
                    SuccessfulLogOff();
                    manager.CurrentUser = new User { EmailAddress = string.Empty };
                }
                
                return;
            }
            else
            {
                RegistrationWindow regWindow = new RegistrationWindow();
                regWindow.Owner = this;
                regWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                regWindow.ShowDialog();

                if (regWindow.DialogResult == true)
                {
                    try
                    {
                        if (manager.ExistUser(regWindow.email.Text))
                        {
                            Message(Properties.Resources.UserExist);
                            return;
                        }
                        bool result = manager.RegistrationUser(regWindow.email.Text, regWindow.password.Password);
                        if (result)
                        {
                            Message(Properties.Resources.RegistrationOk);
                            MailSender mailSender = new MailSender();
                            mailSender.SendMessage(regWindow.email.Text,Properties.Resources.SubjectMail, Properties.Resources.BodyMail + regWindow.password.Password);
                        }
                    }
                    catch (XmlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }


        }

        private void Message(string info)
        {
            InfoWindow infowindow = new InfoWindow();
            infowindow.Owner = this;
            infowindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            infowindow.Message.Text = info;
            infowindow.ShowDialog();
        }

        private void AddContact_Click(object sender, RoutedEventArgs e)
        {
            AddContactWindow addWindow = new AddContactWindow(manager);
            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addWindow.ShowDialog();

            if (addWindow.DialogResult == true)
            {
                try
                {
                    Contact contact = new Contact
                    {
                        DataCreate = DateTime.Now,
                        PhoneNumber = addWindow.phoneNumber.Text,
                        ContactName = addWindow.name.Text,
                        Description = addWindow.description.Text
                    };

                    manager.AddContact(manager.CurrentUser, contact);
                    Message(Properties.Resources.AddContactOk);
                       
                    
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
            }
        }

        private void DeleteAllContact_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow();
            confirm.Owner = this;
            confirm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            confirm.Message.Text = Properties.Resources.ConfirmClearContacts;
            confirm.ShowDialog();

            if (confirm.DialogResult == true)
            {
                manager.ClearContacts(manager.CurrentUser);
            }
           
        }

        private void Delete_Contact_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Contact contact = button.DataContext as Contact;

            ConfirmWindow confirm = new ConfirmWindow();
            confirm.Owner = this;
            confirm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            confirm.Message.Text = string.Format(Properties.Resources.ConfirmDeleteContact, contact.ContactName);
            confirm.ShowDialog();

            if (confirm.DialogResult == true)
            {
                
                try
                {
                    manager.DeleteContact(contact);
                    //Message(Properties.Resources.DeleteComplete);

                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        private void Contacts_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double windowWidth = this.Width;
            column_data.Width = windowWidth / 8;
            search_data.Width = column_data.Width;

            column_phone.Width = windowWidth / 8;
            search_phone.Width = column_phone.Width;
            column_name.Width = windowWidth / 5.333333;

            search_name.Width = column_name.Width;
            column_descr.Width = windowWidth / 5.333333;
            column_operation.Width = windowWidth / 8;

        }

        private void Login_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void Login_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = null;
        }

        private void PhoneBook_Selected(object sender, RoutedEventArgs e)
        {
            contacts.ItemsSource = manager.Contacts;
        }

        private void Search_phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Contact> sourse = manager.Contacts;

            contacts.ItemsSource = manager.Contacts.Where(c => (c.PhoneNumber.IndexOf(search_phone.Text)) >= 0);

            if (search_phone.Text.Equals(string.Empty))
            {
                contacts.ItemsSource = sourse;
            }
        }

        private void Search_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Contact> sourse = manager.Contacts;

            contacts.ItemsSource = manager.Contacts.Where(c => (c.ContactName.IndexOf(search_name.Text))>=0);

            if (search_name.Text.Equals(string.Empty))
            {
                contacts.ItemsSource = sourse;
            }

        }
    }
}

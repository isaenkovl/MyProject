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

namespace MyPhoneBook.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        private EntitiesManager manager;
        public AddContactWindow(EntitiesManager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            phoneNumber.Text = phoneNumber.Text.Trim();
            name.Text = name.Text.Trim();
            description.Text = description.Text.Trim();

            bool phoneNumberIsCorrect = Validator.PhoneCheck(phoneNumber.Text);
            bool nameIsCorrect = Validator.ContactNameCheck(name.Text);

            bool contactNameNotFree = manager.HasContactName(name.Text);
            bool phoneNumberNotFree = manager.HasPhoneNumber(phoneNumber.Text);

            StringBuilder str = new StringBuilder();
            if (!phoneNumberIsCorrect)
            {
                str.Append(Properties.Resources.IncorrectPhoneNumber);
                str.AppendLine();
            }
            if (!nameIsCorrect)
            {
                str.Append(Properties.Resources.IncorrectContactName);
                str.AppendLine();

            }

            if (contactNameNotFree)
            {
                str.Append(Properties.Resources.NameNotFree);
                str.AppendLine();

            }

            if (phoneNumberNotFree)
            {
                str.Append(Properties.Resources.PhoneNotFree);
                str.AppendLine();

            }

            info.Text = str.ToString();

            if (phoneNumberIsCorrect && nameIsCorrect && !contactNameNotFree && !phoneNumberNotFree)
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

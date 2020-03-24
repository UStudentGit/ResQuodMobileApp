using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ResQuod
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OpenRegisterPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new RegisterPage();
        }

        private void TryLogin(object sender, EventArgs e)
        {
            if (!InputDataCorrect())
                return;

            //Check user data in database
            //if incorrect
            //DisplayAlert(title: "Error", message: "Incorreto email or password", cancel: "Ok");
            //else

            Preferences.Set("UserId", 2);
            string nick = Email.Text;
            Preferences.Set("UserNick", nick);
            App.Current.MainPage = new MainPage();
            
        }

        private bool InputDataCorrect()
        {
            //Check email
            if (!IsValidEmail(Email.Text))
            {
                EmailErrorLabel.Text = "Incorrect email";
                return false;
            }
            EmailErrorLabel.Text = "";

            //Check password
            if (String.IsNullOrEmpty(Password.Text) || Password.Text.Length < 6)
            {
                PasswordErrorLabel.Text = "At least 6 characters equired";
                return false;
            }
            PasswordErrorLabel.Text = "";

            return true;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

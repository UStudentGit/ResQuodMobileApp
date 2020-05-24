using ResQuod.Controllers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void OpenLoginPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }

        private async void TryRegister(object sender, EventArgs e)
        {
            if (!InputDataCorrect())
                return;

            //Send request to database
            var credentials = new RegisterModel() { 
                Email = EmailInput.Text,
                Password = PasswordInput.Text,
                Name = NameInput.Text,
                Surname = SurnameInput.Text
            };

            Tuple<APIController.Response, string> response = await APIController.Register(credentials);

            if (response.Item1 != APIController.Response.Success)
            {
                await DisplayAlert(title: "Error", message: "Error: " + response.Item2, cancel: "Ok");
                return;
            }
            else
            {
                await DisplayAlert(title: "Success", message: "Account succesfully created!", cancel: "Ok");
                App.Current.MainPage = new LoginPage();
            }           
        }

        private bool InputDataCorrect()
        {
            //Check name
            if (String.IsNullOrEmpty(NameInput.Text) || NameInput.Text.Length < 3)
            {
                NameErrorLabel.Text = "At least 3 characters equired";
                return false;
            }
            NameErrorLabel.Text = "";

            //Check surname
            if (String.IsNullOrEmpty(SurnameInput.Text) || SurnameInput.Text.Length<3)
            {
                SurnameErrorLabel.Text = "At least 3 characters equired";
                return false;
            }
            SurnameErrorLabel.Text = "";

            //Check email
            if (!IsValidEmail(EmailInput.Text))
            {
                EmailErrorLabel.Text = "Incorrect email";
                return false;
            }
            EmailErrorLabel.Text = "";

            //Check password
            if (String.IsNullOrEmpty(PasswordInput.Text) || PasswordInput.Text.Length < 6)
            {
                PasswordErrorLabel.Text = "At least 6 characters equired";
                return false;
            }
            PasswordErrorLabel.Text = "";

            //Check passwords equality
            if (String.IsNullOrEmpty(PasswordInput.Text) || !ConfirmPasswordInput.Text.Equals(PasswordInput.Text))
            {
                ConfirmPasswordErrorLabel.Text = "Password aren't equal";
                return false;
            }
            ConfirmPasswordErrorLabel.Text = "";

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
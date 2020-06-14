using ResQuod.Controllers;
using ResQuod.Helpers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Views.StartPageViews
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClick(object sender, EventArgs e)
        {
            if (!ValidateInput())
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
                await DisplayAlert(title: "Success", message: "Account succesfully created! You can log in now", cancel: "Ok");
                //App.Current.MainPage = new LoginPage();
            }           
        }

        private bool ValidateInput()
        {
            //Check name
            if (String.IsNullOrEmpty(NameInput.Text) || NameInput.Text.Length < 3)
            {
                NameErrorLabel.Text = ValidationMessages.NameMin;
                NameErrorLabel.IsVisible = true;
                return false;
            }
            NameErrorLabel.IsVisible = false;

            //Check surname
            if (String.IsNullOrEmpty(SurnameInput.Text) || SurnameInput.Text.Length<3)
            {
                SurnameErrorLabel.Text = ValidationMessages.NameMin;
                SurnameErrorLabel.IsVisible = true;
                return false;
            }
            SurnameErrorLabel.IsVisible = false;

            //Check email
            if (!FormDataHelper.IsEmailValid(EmailInput.Text))
            {
                EmailErrorLabel.Text = ValidationMessages.IncorrectEmail;
                EmailErrorLabel.IsVisible = true;
                return false;
            }
            EmailErrorLabel.IsVisible = false;

            //Check password
            if (String.IsNullOrEmpty(PasswordInput.Text) || PasswordInput.Text.Length < 6)
            {
                PasswordErrorLabel.Text = ValidationMessages.PasswordMin;
                PasswordErrorLabel.IsVisible = true;
                return false;
            }
            PasswordErrorLabel.IsVisible = false;

            //Check passwords equality
            if (String.IsNullOrEmpty(PasswordInput.Text) || !ConfirmPasswordInput.Text.Equals(PasswordInput.Text))
            {
                ConfirmPasswordErrorLabel.Text = ValidationMessages.PasswordsNotEqual;
                ConfirmPasswordErrorLabel.IsVisible = true;
                return false;
            }
            ConfirmPasswordErrorLabel.IsVisible = false;

            return true;
        }
    }
}
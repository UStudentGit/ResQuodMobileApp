using ResQuod.Controllers;
using ResQuod.Helpers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ResQuod.Views.StartPageViews
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            ReadUserData();
        }

        public void ReadUserData()
        {
            var user = SessionController.GetUserData();
            if (user != null)
            {
                Email.Text = user.Email;
                Password.Text = user.Password;
            }
            else
            {
                Email.Text = string.Empty;
                Password.Text = string.Empty;
            }
        }

        private async void OnLoginClick(object sender, EventArgs e)
        {
            await LogIn(Email.Text, Password.Text);
        }

        private async Task LogIn(string email, string password)
        {
            if (!ValidateInput())
                return;

            //Send request to database
            var credentials = new LoginModel() { Email = email, Password = password };
            Tuple<APIController.Response, string> login_response = await APIController.Login(credentials);

            if (login_response.Item1 != APIController.Response.Success)
            {
                await DisplayAlert(title: "Error", message: "Error: " + login_response.Item2, cancel: "Ok");
                return;
            }
            else
            {
                //await DisplayAlert(title: "Success", message: "Token: " + login_response.Item2, cancel: "Ok");
            }

            var getUser_response = await APIController.GetUser();
            if (getUser_response.Item1 != APIController.Response.Success)
            {
                await DisplayAlert(title: "Error", message: "Error: " + getUser_response.Item2, cancel: "Ok");
                return;
            }
            else
            {
                SaveUserData(getUser_response.Item3);
                SessionController.SaveUserData(new UserSessionData() { Email = Email.Text, Password = Password.Text });
                await Shell.Current.GoToAsync(AppShell.Routes.Home);
            }
        }

        private bool ValidateInput()
        {
            //Check email
            if (!FormDataHelper.IsEmailValid(Email.Text))
            {
                EmailErrorLabel.Text = ValidationMessages.IncorrectEmail;
                EmailErrorLabel.IsVisible = true;
                return false;
            }
            EmailErrorLabel.IsVisible = false;

            //Check password
            if (String.IsNullOrEmpty(Password.Text) || Password.Text.Length < 6)
            {
                PasswordErrorLabel.Text = ValidationMessages.PasswordMin;
                PasswordErrorLabel.IsVisible = true;
                return false;
            }
            PasswordErrorLabel.IsVisible = false;

            return true;
        }

        private void SaveUserData(User user)
        {
            Preferences.Set("UserName", user.Name);
            Preferences.Set("UserSurname", user.Surname);
            Preferences.Set("UserEmail", user.Email);
            Preferences.Set("Role", user.Role);
        }
    }
}

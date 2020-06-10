using ResQuod.Controllers;
using ResQuod.Models;
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

            var user = SessionController.GetUserData();
            if (user != null)
            {
                Email.Text = user.Email;
                Password.Text = user.Password;
            }
        }

        private void OpenRegisterPage(object sender, EventArgs e)
        {
            App.Current.MainPage = new RegisterPage();
        }

        private async void TryLogin(object sender, EventArgs e)
        {
            await LogIn(Email.Text, Password.Text);
        }

        private async Task LogIn(string email, string password)
        {
            if (!IsInputDataCorrect())
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
                var user = getUser_response.Item3;
                string nick = user.Name + " " + user.Surname;
                await DisplayAlert(title: "Success", message: "Welcome back " + nick + "!", cancel: "Continue");
                Preferences.Set("UserNick", nick);
                SessionController.SaveUserData(new UserSessionData() { Email = Email.Text, Password = Password.Text });
                App.Current.MainPage = new MainPage();
            }
        }

        private bool IsInputDataCorrect()
        {
            //Check email
            if (!FormDataHelper.IsEmailValid(Email.Text))
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

        
    }
}

using ResQuod.Models;
using ResQuod.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ResQuod.Helpers;
using Xamarin.Forms.Internals;

namespace ResQuod.Views.MainViews
{
    public partial class UserPanel : ContentPage
    {
        private User UserData;

        public UserPanel()
        {
            InitializeComponent();

            UserData = new User
            {
                Name = Preferences.Get("UserName", string.Empty),
                Surname = Preferences.Get("UserSurname", string.Empty),
                Email = Preferences.Get("UserEmail", string.Empty)
            };

            Nick.Text = UserData.Name + " " + UserData.Surname;
            Email.Text = UserData.Email;

            NameInput.Text = UserData.Name;
            SurnameInput.Text = UserData.Surname;
            EmailInput.Text = UserData.Email;
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs args)
        {
            Tuple<APIController.Response, string> logout_response = await APIController.Logout();
            if (logout_response.Item1 != APIController.Response.Success)
            {
                LabelErrorAlert.Text = FeedbackMessages.RequestFail;
                LabelErrorAlert.IsVisible = true;
                Console.WriteLine("[REQUEST ERROR] " + logout_response.Item2);
                return;
            }

            Preferences.Set("UserName", string.Empty);
            Preferences.Set("UserSurname", string.Empty);
            Preferences.Set("UserEmail", string.Empty);
            SessionController.ClearUserData();

            // IMPORTANT! startPage jest route'em globalnym,
            // wiec nie mozna go ustawic jako absolutnej sciezki, wiec
            // po tej akcji ponizej aktualna lokalizacja bedzie taka: {aktualna_sciezka}/startPage
            await Shell.Current.GoToAsync("startPage");
        }

        private void OnCancelPatchButtonClicked(object sender, EventArgs args)
        {
            LabelSuccessAlert.IsVisible = false;
            LabelErrorAlert.IsVisible = false;
            NameErrorLabel.IsVisible = false;
            SurnameErrorLabel.IsVisible = false;
            EmailErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;

            NameInput.Text = string.Empty;
            SurnameInput.Text = string.Empty;
            EmailInput.Text = string.Empty;
            PasswordInput.Text = string.Empty;
        }

        private void OnConfirmPatchButtonClicked(object sender, EventArgs args)
        {
            TryPatch();
        }

        //private async void LogOut()
        //{
            
        //}

        private async void TryPatch()
        {
            if (!ValidateInput())
                return;
            
            //Send request to database
            var credentials = new UserPatchModel()
            {
                Email = EmailInput.Text,
                Password = PasswordInput.Text,
                Name = NameInput.Text,
                Surname = SurnameInput.Text
            };

            Tuple<APIController.Response, string> response = await APIController.UserPatch(credentials);

            if (response.Item1 != APIController.Response.Success)
            {
                LabelErrorAlert.Text = FeedbackMessages.RequestFail;
                LabelErrorAlert.IsVisible = true;
                Console.WriteLine("[REQUEST ERROR] " + response.Item2);
                return;
            }

            LabelErrorAlert.IsVisible = false;
            LabelSuccessAlert.Text = FeedbackMessages.SaveSuccess;
            LabelSuccessAlert.IsVisible = true;
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
            if (String.IsNullOrEmpty(SurnameInput.Text) || SurnameInput.Text.Length < 3)
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

            return true;
        }
    }
}
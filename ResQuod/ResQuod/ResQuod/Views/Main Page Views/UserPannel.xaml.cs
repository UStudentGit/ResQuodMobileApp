using ResQuod.Models;
using ResQuod.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Views.Main_Page_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPannel : ContentView
    {
        public UserPannel()
        {
            InitializeComponent();
        }

        private void OnPatchDataButtonClicked(object sender, EventArgs args)
        {
            PatchForm.IsVisible = true;
            PatchDataButton.IsVisible = false;
        }

        private void OnCancelPatchButtonClicked(object sender, EventArgs args)
        {
            PatchForm.IsVisible = false;
            PatchDataButton.IsVisible = true;
            Patch_error1.IsVisible = true;
            Patch_error2.IsVisible = true;
            NameErrorLabel.Text = "";
            SurnameErrorLabel.Text = "";
            EmailErrorLabel.Text = "";
            PasswordErrorLabel.Text = "";
        }

        private void OnConfirmPatchButtonClicked(object sender, EventArgs args)
        {
            TryPatch();
        }

        private async void TryPatch()
        {
            if (!InputDataCorrect())
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
                Patch_error1.Text = response.Item2;
                Patch_error1.IsVisible = true;
                Patch_error2.IsVisible = true;
                return;
            }
            //sukces i co dalej?
            PatchForm.IsVisible = false;
            PatchDataButton.IsVisible = true;
            PatchSuccess.IsVisible = true;
            
        }

        private bool InputDataCorrect()
        {
            //Check name
            if (String.IsNullOrEmpty(NameInput.Text) || NameInput.Text.Length < 3)
            {
                NameErrorLabel.Text = "At least 3 characters required";
                return false;
            }
            NameErrorLabel.Text = "";

            //Check surname
            if (String.IsNullOrEmpty(SurnameInput.Text) || SurnameInput.Text.Length < 3)
            {
                SurnameErrorLabel.Text = "At least 3 characters required";
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
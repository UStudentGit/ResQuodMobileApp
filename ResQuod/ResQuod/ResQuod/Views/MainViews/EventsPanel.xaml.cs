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

namespace ResQuod.Views.MainViews
{
    public partial class EventsPanel : ContentPage, IMainView
    {
        public EventsPanel()
        {
            InitializeComponent();
        }

        public void OnNavigated()
        {

        }

        private async void OnJoinClick(object sender, EventArgs e)
        {
            await JoinEvent(Password.Text);
        }

        private async Task JoinEvent(string password)
        {
            //Send request to database
            var joinResponse = await APIController.JoinEvent(password);

            if (joinResponse.Item1 != APIController.Response.Success)
            {
                await DisplayAlert(title: "Error", message: "Error: " + joinResponse.Item2, cancel: "Ok");
                return;
            }

            await DisplayAlert(title: "Success", message: "Success: " + joinResponse.Item2, cancel: "Ok");
        }

        //byc moze niepotrzebne
        private bool ValidateInput()
        {
            //Check password
            if (String.IsNullOrEmpty(Password.Text))
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
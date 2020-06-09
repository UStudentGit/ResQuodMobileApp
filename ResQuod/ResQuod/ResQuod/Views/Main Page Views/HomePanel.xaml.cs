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
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePanel : ContentView
    {
        public HomePanel()
        {
            InitializeComponent();
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs args)
        {
            await LogOut();
        }

        private async Task LogOut()
        {
            Tuple<APIController.Response, string> logout_response = await APIController.Logout();
            if (logout_response.Item1 != APIController.Response.Success)
            {
                Logout_error1.Text = logout_response.Item2;
                Logout_error1.IsVisible = true;
                Logout_error2.IsVisible = true;
                return;
            }
            
            string nick = "";
            Preferences.Set("UserNick", nick);
            App.Current.MainPage = new LoginPage();
        }

        public void ResetMessages()
        {
            Logout_error1.IsVisible = false;
            Logout_error2.IsVisible = false;
        }
    }
}
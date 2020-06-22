using ResQuod.Controllers;
using ResQuod.Models;
using ResQuod.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ResQuod.Controllers.APIController;

namespace ResQuod
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            TryLogin();
        }

        private async void TryLogin()
        {
            if (SessionController.IsSaved)
            {
                var sessionData = SessionController.GetUserData();
                
                if (sessionData != null)
                {
                    var userData = new LoginModel() { Email = sessionData.Email, Password = sessionData.Password };
                    Tuple<Response, string> response = await APIController.Login(userData);

                    if (response.Item1 == Response.Success)
                    {
                        await Shell.Current.GoToAsync(AppShell.Routes.Home);
                    }
                }
            }
        }

        protected override void OnStart()
        {
            NFCController.StopAll();
        }

        protected override void OnSleep()
        {
            NFCController.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            NFCController.Resume();
        }
    }
}

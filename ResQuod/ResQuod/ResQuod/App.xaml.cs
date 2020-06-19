using ResQuod.Controllers;
using ResQuod.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            if (!SessionController.IsLogged)
            {
                Shell.Current.GoToAsync("startPage");
            }
        }

        protected override void OnStart()
        {
            NFCController.StopAll();
        }

        protected override void OnSleep()
        {
            NFCController.StopAll();
        }

        protected override void OnResume()
        {
        }
    }
}

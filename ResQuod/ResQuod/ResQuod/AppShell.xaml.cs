using ResQuod.Models;
using ResQuod.Views;
using ResQuod.Views.MainViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    public partial class AppShell : Shell
    {
        public static class Routes
        {
            public const string Home = "//home";
            public const string Attendance = "//attendance";
            public const string Events = "//events";
            public const string AddChip = "//addChip";
            public const string User = "//user";
        }

        
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("startPage", typeof(StartPage));
            StartNFCRedirecting();
        }

        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            // hax: disable back button on start page
            //TODO: make dependency service that will kill app
            //reflink: https://stackoverflow.com/questions/29257929/how-to-terminate-a-xamarin-application
            if (e.Source == ShellNavigationSource.Pop && Shell.Current.CurrentState.Location.ToString().IndexOf("startPage") > -1)
            {
                e.Cancel();
            }


            //Application.Current.MainPage.DisplayAlert(e.Source.ToString(), e.Target.Location.ToString(), "Ok");
            if ((e.Source == ShellNavigationSource.ShellSectionChanged || e.Source == ShellNavigationSource.Unknown) 
                && e.Target.Location.ToString() != Routes.Attendance)
            {
                NFCController.StartListening(OnMessageReceived, true);
            }

        }

        private static void OnMessageReceived(NFCTag tag)
        {
            Redirect();
        }

        private static async void Redirect()
        {
            await Task.Run(async () =>
            {
                await Task.Delay(100);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync(Routes.Attendance);
                });

            });
        }

        public static async void StartNFCRedirecting()
        {
            await Task.Delay(100);
            NFCController.StartListening(OnMessageReceived, true);
        }

    }
}
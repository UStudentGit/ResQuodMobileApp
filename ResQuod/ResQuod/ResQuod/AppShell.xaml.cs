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
        string attendanceRoute = "//attendence";
        static bool nfcRedirecting = false;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("startPage", typeof(StartPage));
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

            
            if ((e.Source == ShellNavigationSource.ShellSectionChanged || e.Source == ShellNavigationSource.Unknown ) && e.Target.Location.ToString() != attendanceRoute && nfcRedirecting)
            {
                NFCController.StartListening(OnMessageReceived, true);
            }

        }

        private static void OnMessageReceived(NFCTag tag)
        {
            Shell.Current.GoToAsync("//attendence");
        }

        public static async void StartNFCRedirecting()
        {
            nfcRedirecting = true;
            await Task.Delay(100);
            NFCController.StartListening(OnMessageReceived, true);
        }

    }
}
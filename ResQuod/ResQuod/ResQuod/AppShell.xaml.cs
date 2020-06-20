using ResQuod.Models;
using ResQuod.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ResQuod
{
    public partial class AppShell : Shell
    {
        public static class Routes
        {
            public const string Home = "//home";
            public const string Attendance = "//attendence";
            public const string Events = "//events";
            public const string AddChip = "//addChip";
            public const string User = "//user";
        }

        static bool nfcRedirecting = false;
        
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("startPage", typeof(StartPage));
        }

        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if (Shell.Current == null)
                return;

            // hax: disable back button on start page
            //TODO: make dependency service that will kill app
            //reflink: https://stackoverflow.com/questions/29257929/how-to-terminate-a-xamarin-application
            if (e.Source == ShellNavigationSource.Pop && Shell.Current.CurrentState.Location.ToString().IndexOf("startPage") > -1)
            {
                e.Cancel();
            }
            
            if ((e.Source == ShellNavigationSource.ShellSectionChanged || e.Source == ShellNavigationSource.Unknown) 
                && e.Target.Location.ToString() != Routes.Attendance 
                && nfcRedirecting)
            {
                NFCController.StartListening(OnMessageReceived, true);
            }

            MapRouteToPage(e.Target.Location)?.onNavigated();
        }

        private static void OnMessageReceived(NFCTag tag)
        {
            Shell.Current.GoToAsync(Routes.Attendance);
        }

        public static async void StartNFCRedirecting()
        {
            nfcRedirecting = true;
            await Task.Delay(100);
            NFCController.StartListening(OnMessageReceived, true);
        }

        private IMainView MapRouteToPage(Uri location)
        {
            switch (location.ToString())
            {               
                case Routes.Home:
                    return homePanel;
                case Routes.Attendance:
                    return attendancePanel;
                case Routes.Events:
                    return eventsPanel;
                case Routes.AddChip:
                    return addChipPanel;
                case Routes.User:
                    return userPanel;
                default:
                    return null;
            }
        }

    }
}
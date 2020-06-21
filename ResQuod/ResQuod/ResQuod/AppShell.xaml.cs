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
            public const string StartPage = "//startPage";
            public const string Home = "//home";
            public const string Attendance = "//attendance";
            public const string Events = "//events";
            public const string AddChip = "//addChip";
            public const string User = "//user";
        }

        
        public AppShell()
        {
            InitializeComponent();
            StartNFCRedirecting();
        }

        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if (Shell.Current == null)
                return;
            
            if ((e.Source == ShellNavigationSource.ShellSectionChanged || e.Source == ShellNavigationSource.Unknown) 
                && e.Target.Location.ToString() != Routes.Attendance)
            {
                NFCController.StartListening(OnMessageReceived, true);
            }

            MapRouteToPage(e.Target.Location)?.OnNavigated();
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

        private IMainView MapRouteToPage(Uri location)
        {
            switch (location.ToString())
            {
                case Routes.StartPage:
                    return startPage;
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
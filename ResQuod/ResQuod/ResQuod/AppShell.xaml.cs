﻿using ResQuod.Views;
using ResQuod.Views.MainViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("startPage", typeof(StartPage));
        }

        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            NFCController.StopAll();

            // hax: disable back button on start page
            //TODO: make dependency service that will kill app
            //reflink: https://stackoverflow.com/questions/29257929/how-to-terminate-a-xamarin-application
            if (e.Source == ShellNavigationSource.Pop && Shell.Current.CurrentState.Location.ToString().IndexOf("startPage") > -1)
            {
                e.Cancel();
            }
        }
    }
}
using ResQuod.Views.StartPageViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Views
{
    public partial class StartPage : TabbedPage, IMainView
    {
        public StartPage()
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);
        }

        public void OnNavigated()
        {
            loginPanel?.ReadUserData();
        }
    }
}
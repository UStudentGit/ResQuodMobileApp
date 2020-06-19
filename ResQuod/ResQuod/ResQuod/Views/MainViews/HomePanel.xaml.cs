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

namespace ResQuod.Views.MainViews
{
    public partial class HomePanel : ContentPage
    {
        public HomePanel()
        {
            InitializeComponent();
            AppShell.StartNFCRedirecting();
        }
    }
}
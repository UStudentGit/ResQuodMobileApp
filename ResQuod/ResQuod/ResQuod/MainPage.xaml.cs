using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            //Init panels
            HomePanel.Content = new HomePanel();
            AttendancePanel.Content = new AttendancePanel();
            //kolejnyElement.Content = new NazwaPanelu();



            //StartLabel.Text = "Welcome" + Preferences.Get("UserNick", "").ToString();
        }
    }
}
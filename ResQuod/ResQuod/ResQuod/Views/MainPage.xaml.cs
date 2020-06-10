using ResQuod.Main_Page_Views;
using ResQuod.Views.Main_Page_Views;
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
        private HomePanel homePanel;
        private AttendancePanel attendancePanel;
        private AddChipPanel addChipPanel;
        private UserPanel userPanel;
        public MainPage()
        {
            InitializeComponent();

            //Init panels
            homePanel = new HomePanel();
            HomePanel_Content.Content = homePanel;

            attendancePanel = new AttendancePanel();
            AttendancePanel_Content.Content = attendancePanel;

            addChipPanel = new AddChipPanel();
            AddChipPanel_Content.Content = addChipPanel;

            userPanel = new UserPanel();
            UserPanel_Content.Content = userPanel;

            //kolejnyElement.Content = new NazwaPanelu();



            //StartLabel.Text = "Welcome" + Preferences.Get("UserNick", "").ToString();
        }

        private void TabbedPage_PagesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Application.Current.MainPage.DisplayAlert("Error", "Test", "Ok");
            if (MainPage_TabbedPage != null && MainPage_TabbedPage.CurrentPage != null)
                if(MainPage_TabbedPage.CurrentPage.Title == "Home")
                {
                    //homePanel.ResetMessages();
                }
                else if(MainPage_TabbedPage.CurrentPage.Title == "Attendance")
                {
                    addChipPanel.StopAll();
                    attendancePanel = new AttendancePanel();
                    AttendancePanel_Content.Content = attendancePanel;
                
                }
                else if(MainPage_TabbedPage.CurrentPage.Title == "Add chip")
                {
                    attendancePanel.StopListening();
                    addChipPanel = new AddChipPanel();
                    AddChipPanel_Content.Content = addChipPanel;
                }
                else
                {
                    if (attendancePanel == null)
                        return;

                    addChipPanel.StopAll();
                    attendancePanel.StopListening();
                }
        }

    }
}
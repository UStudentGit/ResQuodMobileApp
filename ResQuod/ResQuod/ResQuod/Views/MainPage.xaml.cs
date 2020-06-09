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
        private HomePanel homePannel;
        private AttendancePanel attendancePanel;
        private AddChipPanel addChipPanel;
        private UserPannel userPannel;
        public MainPage()
        {
            InitializeComponent();

            //Init panels
            homePannel = new HomePanel();
            HomePanel_Content.Content = homePannel;

            attendancePanel = new AttendancePanel();
            AttendancePanel_Content.Content = attendancePanel;

            addChipPanel = new AddChipPanel();
            AddChipPanel_Content.Content = addChipPanel;

            userPannel = new UserPannel();
            UserPanel_Content.Content = userPannel;

            //kolejnyElement.Content = new NazwaPanelu();



            //StartLabel.Text = "Welcome" + Preferences.Get("UserNick", "").ToString();
        }

        private void TabbedPage_PagesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Application.Current.MainPage.DisplayAlert("Error", "Test", "Ok");
            if (MainPage_TabbedPage != null && MainPage_TabbedPage.CurrentPage != null)
                if(MainPage_TabbedPage.CurrentPage.Title == "Home")
                {
                    //homePannel.ResetMessages();
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
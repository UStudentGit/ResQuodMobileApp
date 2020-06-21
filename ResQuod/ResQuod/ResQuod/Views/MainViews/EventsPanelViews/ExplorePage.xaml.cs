using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ResQuod.Controllers;
using ResQuod.Helpers;
using ResQuod.Models;
using System.Collections.ObjectModel;


namespace ResQuod.Views.MainViews.EventPanelViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExplorePage : ContentPage
    {
        public ExplorePage()
        {
            InitializeComponent();
            Events = new ObservableCollection<Event>();
            GetEventsCommand = new Command(async () => await TryGetEvents());
        }

        public Command GetEventsCommand { get; }

        private ObservableCollection<Event> Events { get; }

        private async Task TryGetEvents()
        {
            if (IsBusy) return;

            IsBusy = true;
            var response = await APIController.GetUserEvents();
            if (response.Item1 != APIController.Response.Success)
            {
                //LabelErrorAlert.Text = FeedbackMessages.RequestFail;
                //LabelErrorAlert.IsVisible = true;
                Console.WriteLine("[REQUEST ERROR] " + response.Item2);
                return;
            }
            // wydaje mi sie ze tu trzeba cos jeszcze zrobic z response.Item3 bo to jest ta lista eventow 
            // ale w filmiku nic z tym babeczka nie robila poza ta petla i dzialalo wiec nie wiem
            // https://www.youtube.com/watch?v=JY900hOQCKQ
            Events.Clear();
            foreach (var e in response.Item3)
                Events.Add(e);
            IsBusy = false;

        }

    }
}
using ResQuod.Controllers;
using ResQuod.Models;
using ResQuod.Views.ViewComponents;
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
    public partial class HomePanel : ContentPage, IMainView
    {
        private List<Event> events = new List<Event>();

        public HomePanel()
        {
            InitializeComponent();
            //AppShell.StartNFCRedirecting();
        }

        public void OnNavigated()
        {
            if (events.Count == 0)
            {
                GetEvents();
            }
        }

        private async void GetEvents()
        {
            var response = await APIController.GetUserEvents();

            if (response.Item1 != APIController.Response.Success)
            {
                events.Clear();
                CurrentEventsEmptyLabel.IsVisible = true;
                Console.WriteLine("[REQUEST ERROR] " + response.Item1 + response.Item2);
                await Application.Current.MainPage.DisplayAlert("Error", response.Item2, "Close");
                return;
            }

            events = response.Item3;

            if (events.Count == 0)
            {
                CurrentEventsEmptyLabel.IsVisible = true;
                return;
            }

            CurrentEventsEmptyLabel.IsVisible = false;
            foreach (var eventItem in events)
            {
                var card = new EventCard
                {
                    CardTitle = eventItem.Name,
                    RoomName = eventItem.Room.Name
                };

                CurrentEventsWrapper.Children.Add(card);
            }
        }
    }
}
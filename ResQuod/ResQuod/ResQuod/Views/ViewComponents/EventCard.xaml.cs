using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Views.ViewComponents
{
    public partial class EventCard : ContentView
    {
        public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(
            "CardTitle", typeof(string), typeof(EventCard));
        public string CardTitle
        {
            get => (string)GetValue(CardTitleProperty);
            set => SetValue(CardTitleProperty, value);
        }

        public static readonly BindableProperty RoomNameProperty = BindableProperty.Create(
            "RoomName", typeof(string), typeof(EventCard));
        public string RoomName
        {
            get => "Room: " + (string)GetValue(RoomNameProperty);
            set => SetValue(RoomNameProperty, value);
        }

        public EventCard()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync(AppShell.Routes.Attendance);
        }
    }
}
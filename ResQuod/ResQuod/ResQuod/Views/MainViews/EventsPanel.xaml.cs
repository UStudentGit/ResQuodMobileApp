using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ResQuod.Views.MainViews.EventPanelViews;

namespace ResQuod.Views.MainViews
{
    public partial class EventsPanel : ContentPage, IMainView
    {
        public EventsPanel()
        {
            InitializeComponent();
            BindingContext = new ExplorePage();
        }

        public void onNavigated()
        {

        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var selected_event = args.CurrentSelection.FirstOrDefault() as Event;
            if (selected_event == null) return;

            //TODO
            //[before burdel] async nawigacja do strony z wpisaniem kodu dostepu do wydarzenia
            //[after burdel] emm no po kliknieciu w wydarzenie to chyba juz do strony z nfc tagiem powinno przenosic tez z async

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}

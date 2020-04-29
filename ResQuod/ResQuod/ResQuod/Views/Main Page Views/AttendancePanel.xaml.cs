using Plugin.NFC;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendancePanel : ContentView
    {
        public AttendancePanel()
        {
            InitializeComponent();
            StartNFCListening();
            
        }

        public void StartNFCListening()
        {
            NFCAvailable_Label.Text = NFCController.IsAvailable ? "NFC available" : "NFC not supportet on Your phone";
            NFCAvailable_Label.TextColor = NFCController.IsAvailable? Color.Green : Color.Red;
            NFCEnabled_Label.Text = NFCController.IsEnabled ? "NFC enabled" : "NFC disabled";
            NFCEnabled_Label.TextColor = NFCController.IsEnabled? Color.Green : Color.Red;

            if (NFCController.IsEnabled)
            {
                Title_Label.TextColor = Color.Green;
                NFCController.StartListening(OnMessageReceived);
            }
                
        }

        private void OnMessageReceived(NFCTag tag)
        {
            TagId_Label.Text = "Tag ID: " + tag.TagId;

            //Messages reader
            string message = tag.MeetingCode;
            if (message.Length==0)
            {
                EventId_Label.Text = "Event ID: empty";
            }
            else
            {
                EventId_Label.Text = "Event ID: " + message;
            }

            ImageWait.IsVisible = false;
            ImageOk.IsVisible = true;
            StartAgain();

        }

        public async Task StartAgain()
        {
            await Task.Delay(100);
            NFCController.StartListening(OnMessageReceived);
        }

        public void StopListening()
        {
            NFCController.StopAll();
        }

    }
}
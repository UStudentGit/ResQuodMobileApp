using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Main_Page_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddChipPanel : ContentView
    {
        public AddChipPanel()
        {
            InitializeComponent();
            StopAll();
            InitGUI();

        }

        private void InitGUI()
        {
            NFCAvailable_Label.Text = NFCController.IsAvailable ? "NFC available" : "NFC not supportet on Your phone";
            NFCAvailable_Label.TextColor = NFCController.IsAvailable ? Color.Green : Color.Red;
            NFCEnabled_Label.Text = NFCController.IsEnabled ? "NFC enabled" : "NFC disabled";
            NFCEnabled_Label.TextColor = NFCController.IsEnabled ? Color.Green : Color.Red;

            if (NFCController.IsEnabled)
            {
                Title_Label.TextColor = Color.Green;
            }
            else
            {
                Start.IsEnabled = false;
            }

        }

        private void StartWriting(object sender, EventArgs e)
        {
            ImageWait.IsVisible = true;
            ImageOk.IsVisible = false;
            NFCController.StartPublishing(EventId_Entry.Text, OnMessagePublished);
        }

        private void OnMessagePublished(ITagInfo tagInfo)
        {
            ImageWait.IsVisible = false;
            ImageOk.IsVisible = true;
            TagId_Label.Text = tagInfo.SerialNumber.ToString();
            EventId_Label.Text = tagInfo.Records is null ? "Id: empty" : "Id: " + tagInfo.Records[0].Message;
            NFCController.StopAll();
        }

        public void StopAll()
        {
            NFCController.StopAll();
        }
    }
}
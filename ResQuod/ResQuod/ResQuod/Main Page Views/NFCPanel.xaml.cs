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
    public partial class NFCPanel : ContentView
    {
        NFCController NFC;
        public NFCPanel()
        {
            InitializeComponent();
            NFC = new NFCController();
            Label1.Text = "Available: " + NFC.IsAvailable;
            Label2.Text = "Enabled: " + NFC.IsEnabled;
        }

        private void StartNFC(object sender, EventArgs e)
        {
            NFC.StartListening();
            Label2.Text = "Enabled: " + NFC.IsEnabled;
            NFC.OnMessageReceived_AddHandler(OnMessageReceived);
        }

        

        private void RefreshNFC(object sender, EventArgs e)
        {
            Label2.Text = "Enabled: " + NFC.IsEnabled;
        }

        private void OnMessageReceived(ITagInfo tagInfo)
        {
            Label3.Text = "Message: " + tagInfo.SerialNumber.ToString();
        }


    }
}
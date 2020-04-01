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

        private void StartNFCListening(object sender, EventArgs e)
        {
            NFC.StartListening(OnMessageReceived);
            Label2.Text = "Enabled: " + NFC.IsEnabled;
        }

        private void StopNFCListening(object sender, EventArgs e)
        {
            NFC.StopListening(OnMessageReceived);
            Label2.Text = "Enabled: " + NFC.IsEnabled;
        }

        private void OnMessageReceived(ITagInfo tagInfo)
        {
            Label3.Text = "Id: " + tagInfo.SerialNumber.ToString();

            //Messages reader
            string message = "Message: \n";
            if (tagInfo.IsEmpty)
            {
                Label4.Text = "Empty";
            }
            else
            {
                foreach (NFCNdefRecord record in tagInfo.Records)
                {
                    message += Encoding.ASCII.GetString(record.Payload).ToString() + ",\n";
                    message += record.Message != null ? record.Message.ToString() + ",\n" : "-\n";
                    message += record.MimeType != null ? record.MimeType.ToString() + ",\n" : "-\n";
                    message += record.TypeFormat != null ? record.TypeFormat.ToString() + ",\n" : "-\n";
                    message += record.Uri != null ? record.Uri.ToString() + ",\n" : "-\n";
                    message += record.ExternalDomain != null ?  record.ExternalDomain.ToString() + ",\n" : "-\n";
                    message += record.ExternalType != null ?  record.ExternalType.ToString() + ",\n" : "-\n";
                }
                    
                Label4.Text = message;
            }

            //
            // NFC.StopListening();
            // NFC.StartListening();
            StartAgain();

        }

        public async Task StartAgain()
        {
            await Task.Delay(100);
            NFC.StartListening(OnMessageReceived);

        }

        private void StartSending(object sender, EventArgs e)
        {
            
            List<string> messages = new List<string>() { MessageEntry.Text };
            NFC.StartPublishing(messages, OnMessagePublished);
            
        }

        private void OnMessagePublished(ITagInfo tagInfo)
        {
            Label6.Text = "Finito";
            
        }
    }
}
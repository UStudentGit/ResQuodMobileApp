using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ResQuod
{
    //https://github.com/franckbour/Plugin.NFC
    class NFCController
    {

        public bool IsAvailable {
            get {
                return CrossNFC.IsSupported && CrossNFC.Current.IsAvailable;
            } 
        }

        public bool IsEnabled
        {
            get
            {
                return CrossNFC.Current.IsEnabled;
            }
        }

        private List<string> messages;
        public void StartListening(NdefMessageReceivedEventHandler MessageReceivedHandler)
        {
            StopAll();
            CrossNFC.Current.StartListening();
            CrossNFC.Current.OnMessageReceived += MessageReceivedHandler;
            //CrossNFC.Current.OnMessageReceived += StopListening;
        }

        private void StopAll()
        {
            CrossNFC.Current.StartListening();
            CrossNFC.Current.StopPublishing();
        }

        public void StopListening(NdefMessageReceivedEventHandler MessageReceivedHandler)
        {
            CrossNFC.Current.StartListening();
            CrossNFC.Current.StopPublishing();
            CrossNFC.Current.StopListening();
            CrossNFC.Current.OnMessageReceived -= MessageReceivedHandler;
        }

        public void StartPublishing(List<string> messages, NdefMessagePublishedEventHandler MessagePublishedHandler)
        {
            StopAll();
            this.messages = messages;
            CrossNFC.Current.StartListening();
            CrossNFC.Current.StartPublishing();
            CrossNFC.Current.OnTagDiscovered += SendData;
            CrossNFC.Current.OnMessagePublished += MessagePublishedHandler;
            CrossNFC.Current.OnMessagePublished += StopPublishing;
        }

        private void StopPublishing(ITagInfo tagInfo)
        {
            //CrossNFC.Current.StopPublishing();
            //CrossNFC.Current.StopListening();
        }

        private void SendData(ITagInfo tagInfo, bool format)
        {
            try
            {
                ITagInfo info = tagInfo;
                List<NFCNdefRecord> records = new List<NFCNdefRecord>();
                foreach (string message in messages)
                {
                    NFCNdefRecord record = new NFCNdefRecord() { MimeType = "", TypeFormat = NFCNdefTypeFormat.WellKnown, Payload = Encoding.ASCII.GetBytes(message) };
                    Application.Current.MainPage.DisplayAlert("Witam", Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(message)).ToString(), "Ok");
                    records.Add(record);
                }

                info.Records = records.ToArray();
                CrossNFC.Current.PublishMessage(info);
            }
            catch(Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }
            
            
        }
       


    }
}

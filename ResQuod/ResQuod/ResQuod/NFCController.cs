using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ResQuod
{
    //https://github.com/franckbour/Plugin.NFC
    static class NFCController
    {

        public static bool IsAvailable {
            get {
                return CrossNFC.IsSupported && CrossNFC.Current.IsAvailable;
            } 
        }

        public static bool IsEnabled
        {
            get
            {
                return CrossNFC.Current.IsEnabled;
            }
        }

        private static string currentMessage;
        private static NdefMessageReceivedEventHandler messageReceivedHandler;
        private static NdefMessagePublishedEventHandler messagePublishedHandler;
        private static TagDiscoveredEventHandler tagDiscoveredHandler;


        public static void StartListening(NdefMessageReceivedEventHandler MessageReceivedHandler)
        {
            //Clear all
            StopAll();

            //Start NFC
            CrossNFC.Current.StartListening();

            //Handlers
            messageReceivedHandler = MessageReceivedHandler;
            CrossNFC.Current.OnMessageReceived += messageReceivedHandler;
            
        }

        public static void StopAll()
        {
            try
            {
                CrossNFC.Current.OnMessageReceived -= messageReceivedHandler;
                messageReceivedHandler = null;

                CrossNFC.Current.OnMessagePublished -= messagePublishedHandler;
                messagePublishedHandler = null;

                CrossNFC.Current.OnTagDiscovered -= tagDiscoveredHandler;
                tagDiscoveredHandler = null;

                CrossNFC.Current.StopListening();
                CrossNFC.Current.StopPublishing();
            }
            catch(Exception ex)
            {

            }
            
        }


        public static void StartPublishing(string messages, NdefMessagePublishedEventHandler MessagePublishedHandler)
        {
            //Clear all
            StopAll();
            currentMessage = messages;

            //Start NFC
            CrossNFC.Current.StartListening();
            CrossNFC.Current.StartPublishing();

            //Hnadlers
            tagDiscoveredHandler = SendData;
            CrossNFC.Current.OnTagDiscovered += tagDiscoveredHandler;

            messagePublishedHandler = MessagePublishedHandler;
            CrossNFC.Current.OnMessagePublished += messagePublishedHandler;
        }

        private static void SendData(ITagInfo tagInfo, bool format)
        {
            try
            {
                ITagInfo info = tagInfo;

                List<NFCNdefRecord> records = new List<NFCNdefRecord>();

                NFCNdefRecord record = new NFCNdefRecord() { MimeType = "", TypeFormat = NFCNdefTypeFormat.WellKnown, Payload = Encoding.ASCII.GetBytes(currentMessage) };

                records.Add(record);
                

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

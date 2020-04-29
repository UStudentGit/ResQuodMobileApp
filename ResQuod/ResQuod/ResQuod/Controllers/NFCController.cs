using Plugin.NFC;
using ResQuod.Models;
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
        private static NdefMessageReceivedEventHandler messageReceived;
        private static NdefMessagePublishedEventHandler messagePublished;
        private static TagDiscoveredEventHandler tagDiscovered;

        //Local handlers
        private static OnMessageReceived OnMessageReceivedHandler;
        private static OnMessagePublished OnMessagePublishedHandler;

        public delegate void OnMessageReceived(NFCTag tag);
        public delegate void OnMessagePublished(NFCTag tag);


        public static void StartListening(OnMessageReceived MessageReceivedHandler)
        {
            //Clear all
            StopAll();

            //Start NFC
            CrossNFC.Current.StartListening();

            //Handler
            OnMessageReceivedHandler = MessageReceivedHandler;

            //Local Handlers
            messageReceived = MessageReceived;
            CrossNFC.Current.OnMessageReceived += messageReceived;
        }

        public static void StopAll()
        {
            try
            {
                CrossNFC.Current.OnMessageReceived -= messageReceived;
                messageReceived = null;

                CrossNFC.Current.OnMessagePublished -= messagePublished;
                messagePublished = null;

                CrossNFC.Current.OnTagDiscovered -= tagDiscovered;
                tagDiscovered = null;

                CrossNFC.Current.StopListening();
                CrossNFC.Current.StopPublishing();
            }
            catch(Exception ex)
            {

            }
            
        }

        public static void StartPublishing(string meetingID, OnMessagePublished MessagePublishedHandler)
        {
            //Clear all
            StopAll();
            currentMessage = meetingID;

            //Start NFC
            CrossNFC.Current.StartListening();
            CrossNFC.Current.StartPublishing();

            //Handlers
            tagDiscovered = SendData;
            CrossNFC.Current.OnTagDiscovered += tagDiscovered;

            OnMessagePublishedHandler = MessagePublishedHandler;

            messagePublished = MessagePublished;
            CrossNFC.Current.OnMessagePublished += messagePublished;
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
                CrossNFC.Current.PublishMessage(info, makeReadOnly: false);
            }
            catch(Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }
            
            
        }

        private static void MessageReceived(ITagInfo tagInfo)
        {
            string id = tagInfo.SerialNumber;

            //Messages reader
            string message = "";
            if (!tagInfo.IsEmpty)
            {
                foreach (NFCNdefRecord record in tagInfo.Records)
                {
                    // message += Encoding.ASCII.GetString(record.Payload).ToString() + ",\n";
                    message += record.Message != null ? record.Message.ToString() : " ";
                    //message += record.MimeType != null ? record.MimeType.ToString() + ",\n" : "-\n";
                    //message += record.TypeFormat.ToString() + ",\n";
                    //message += record.Uri != null ? record.Uri.ToString() + ",\n" : "-\n";
                    //message += record.ExternalDomain != null ? record.ExternalDomain.ToString()    + ",\n" : "-\n";
                    //message += record.ExternalType != null ? record.ExternalType.ToString() + ",\n" : "-\n";
                }
            }
            var tag = new NFCTag()
            {
                TagId = id,
                MeetingCode = message
            };
            OnMessageReceivedHandler(tag);
        }

        private static void MessagePublished(ITagInfo tagInfo)
        {
            string id = tagInfo.SerialNumber;

            //Messages reader
            string message = "";
            if (!tagInfo.IsEmpty)
            {
                foreach (NFCNdefRecord record in tagInfo.Records)
                {
                    message += record.Message != null ? record.Message.ToString() : " ";
                }
            }
            var tag = new NFCTag()
            {
                TagId = id,
                MeetingCode = message
            };

            OnMessagePublishedHandler(tag);
        }



    }
}

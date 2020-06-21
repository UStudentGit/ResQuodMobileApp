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
        enum State
        {
            Waiting, Listening, Publishing
        }

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

        private static State state = State.Waiting;

        public static bool IsListening { get; private set; }

        private static string currentMessage;
        private static NdefMessageReceivedEventHandler messageReceived;
        private static NdefMessagePublishedEventHandler messagePublished;
        private static TagDiscoveredEventHandler tagDiscovered;

        //Local handlers
        private static List<OnMessageReceived> OnMessageReceivedHandler = new List<OnMessageReceived>();
        private static OnMessagePublished OnMessagePublishedHandler;

        public delegate void OnMessageReceived(NFCTag tag);
        public delegate void OnMessagePublished(NFCTag tag);        

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

                IsListening = false;
            }
            catch(Exception ex)
            {

            }
            
        }

        public static void Pause()
        {
            try
            {
                CrossNFC.Current.StopListening();
                CrossNFC.Current.StopPublishing();
            }
            catch (Exception ex)
            {

            }

        }

        public static void Resume()
        {
            try
            {
                if(state != State.Waiting)
                    CrossNFC.Current.StartListening();

                if(state == State.Publishing)
                    CrossNFC.Current.StartPublishing();
            }
            catch (Exception ex)
            {

            }

        }

        public static void StartListening()
        {
            state = State.Listening;

            if (!(OnMessageReceivedHandler.Count > 1) || !IsListening)
            {
                //Start NFC
                CrossNFC.Current.StartListening();

                //Local Handlers
                messageReceived = MessageReceived;
                CrossNFC.Current.OnMessageReceived += messageReceived;
                IsListening = true;
            }
        }

        public static void StartListening(OnMessageReceived MessageReceivedHandler, bool additional = false)
        {
            state = State.Listening;

            if (!additional)
            {
                //Clear all
                StopAll();
                OnMessageReceivedHandler.Clear();
            }


            //Start NFC
            CrossNFC.Current.StartListening();

            //Handler
            if (!OnMessageReceivedHandler.Contains(MessageReceivedHandler))
                OnMessageReceivedHandler.Add(MessageReceivedHandler);

            //Local Handlers
            messageReceived = MessageReceived;

            if (!(OnMessageReceivedHandler.Count > 1) || !IsListening)
            {
                CrossNFC.Current.OnMessageReceived += messageReceived;
                IsListening = true;
            }
        }


        public static void StartPublishing(string meetingID, OnMessagePublished MessagePublishedHandler)
        {
            state = State.Publishing;
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
                    message += record.Message != null ? record.Message.ToString() : " ";
                }
            }
            var tag = new NFCTag()
            {
                TagId = id,
                MeetingCode = message
            };
            foreach (var handler in OnMessageReceivedHandler)
                handler(tag);
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

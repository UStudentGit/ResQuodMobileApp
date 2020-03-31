using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod
{
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

        public NFCController()
        {  

        }

        public void StartListening()
        {
            CrossNFC.Current.StartListening();
        }

        public void OnMessageReceived_AddHandler(NdefMessageReceivedEventHandler Current_OnMessageReceived)
        {
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
        }


        public void OnMessageReceived_DeleteHandler(NdefMessageReceivedEventHandler Current_OnMessageReceived)
        {
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
        }

        public void OnMessagePublished_AddHandler(NdefMessagePublishedEventHandler Current_OnMessagePublished)
        {
            CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
        }

        public void OnMessagePublished_DeleteHandler(NdefMessagePublishedEventHandler Current_OnMessagePublished)
        {
            CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
        }

    }
}

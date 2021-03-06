﻿using Plugin.NFC;
using ResQuod.Controllers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Views.MainViews
{
    public partial class AttendancePanel : ContentPage, IMainView
    {
        string currentTag = "test";
        bool firstOpen = false;
        //MainPage parent;

        public AttendancePanel()
        {
            InitializeComponent();
        }

        public void OnNavigated()
        {
            if(firstOpen == false)
                StartNFCListening();
        }

        public void StartNFCListening()
        {
            firstOpen = true;
            NFCAvailable_Label.Text = NFCController.IsAvailable ? "NFC available" : "NFC not supportet on Your phone";
            NFCAvailable_Label.TextColor = NFCController.IsAvailable? Color.Green : Color.Red;
            NFCEnabled_Label.Text = NFCController.IsEnabled ? "NFC enabled" : "NFC disabled";
            NFCEnabled_Label.TextColor = NFCController.IsEnabled? Color.Green : Color.Red;
            TagId_Label.Text = "ID: ";

            if (NFCController.IsEnabled)
            {
                Title_Label.TextColor = Color.Green;
                NFCController.StartListening(OnMessageReceived, true);
            }

            ImageWait.IsVisible = true;
            ImageOk.IsVisible = false;
            Check_Button.IsEnabled = false;
        }

        private void OnMessageReceived(NFCTag tag)
        {
            TagId_Label.Text = "ID: " + tag.TagId;
            //TODO
            currentTag = tag.TagId;
            Check_Button.IsEnabled = true;
            Check_Button.BackgroundColor = Color.FromHex("008B8B"); ;

            ImageWait.IsVisible = false;
            ImageOk.IsVisible = true;

            StartAgain();
        }

        public async Task StartAgain()
        {
            await Task.Delay(100);
            NFCController.StartListening(OnMessageReceived, true);
        }

        public void StopNFC()
        {
            NFCController.StopAll();
        }

        private async void Check_Button_Clicked(object sender, EventArgs e)
        {
            Tuple<APIController.Response, string, PresenceResponse> response = await APIController.ReportPresence(currentTag);
            //Logs.Text = response.Item1.ToString() + ": " + response.Item2;
            if(response.Item1 == APIController.Response.Success)
            {
                var eventResponse = response.Item3;
                await Application.Current.MainPage.DisplayAlert(response.Item1.ToString(), response.Item2 + "\nEvent: " + eventResponse.EventName , "Ok");
                await Task.Run(async () =>
                {
                    await Task.Delay(300);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        
                        // TODO: jak chodzi o zmianę strony to Shell.Current.GoToAsync
                        await Shell.Current.GoToAsync(AppShell.Routes.Home);
                        StartNFCListening();

                    });

                });
                return;
            }

            if (response.Item1 == APIController.Response.BadRequest)
            {
                var eventResponse = response.Item3;
                await Application.Current.MainPage.DisplayAlert(response.Item1.ToString(), response.Item2, "Ok");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(response.Item1.ToString(), response.Item2, "Ok");
            return;
            
        }
    }
}
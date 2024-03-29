﻿using Plugin.NFC;
using ResQuod.Controllers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResQuod.Views.MainViews
{
    public partial class AddChipPanel : ContentPage, IMainView
    {
        private List<RoomPosition> positions = new List<RoomPosition>();
        private RoomPosition currentPosition;

        public AddChipPanel()
        {
            InitializeComponent();
            InitGUI();

        }

        public void OnNavigated()
        {
            if (Preferences.Get("Role", "") == "ROLE_ADMIN")
                GeneratePickerElements();
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
                //Start.IsEnabled = false;
            }

        }

        private async void GeneratePickerElements()
        {
            PositionPicker.Items.Clear();
            PositionPicker.IsEnabled = true;
            Tuple<APIController.Response, string, RoomPosition[]> result = await APIController.GetPositionsWithoutTag();
            var posList = result.Item3.ToList();
            positions = posList;
            var positionsStrings = posList.Select(p => p.RoomName.ToString() + "/" + p.PositionNumber.ToString()).ToList();
            foreach (var pos in positionsStrings)
                PositionPicker.Items.Add(pos);
        }

        private void StartWriting(object sender, EventArgs e)
        {
            ImageWait.IsVisible = true;
            ImageOk.IsVisible = false;
            NFCController.StartPublishing(currentPosition.RoomName + "/" + currentPosition.PositionNumber, OnMessagePublished);

            StartWriting_Button.BackgroundColor = Color.White;
            StartWriting_Button.IsEnabled = false;
            LockButtons();
        }

        private void OnMessagePublished(NFCTag tag)
        {
            ImageWait.IsVisible = false;
            ImageOk.IsVisible = true;
            //TagId_Label.Text = tag.TagId;
            //EventId_Label.Text = tag.MeetingCode;
            NFCController.StopAll();
            SendRequest(tag.TagId);
            //Application.Current.MainPage.DisplayAlert("Success", "Succesfully assigned tag " + tag.TagId + " to " + currentPosition.RoomName + "/" + currentPosition.PositionNumber, "Ok");
            currentPosition = null;
            GeneratePickerElements();
            NFCController.StartListening();
        }

        private async void SendRequest(string tagId)
        {
            Tuple<APIController.Response, string> result = await APIController.AssignTagToPosition(tagId, currentPosition.PositionId);
            await Application.Current.MainPage.DisplayAlert(result.Item1.ToString(), result.Item2, "Ok");
        }

        public void StopAll()
        {
            NFCController.StopAll();
        }

        private void LockButtons()
        {
            PositionPicker.IsEnabled = false;
        }

        private void PositionPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!PositionPicker.IsEnabled && positions.Count > PositionPicker.SelectedIndex)
                    return;
                currentPosition = positions[PositionPicker.SelectedIndex];
                StartWriting_Button.BackgroundColor = Color.FromHex("008B8B");
                StartWriting_Button.IsEnabled = NFCController.IsEnabled;
                ImageWait.IsVisible = true;
                ImageOk.IsVisible = false;
            }
            catch(Exception exception)
            {
                Console.WriteLine("Out of range exception");
            }
        }
    }
}
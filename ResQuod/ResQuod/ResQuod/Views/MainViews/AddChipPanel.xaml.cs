using Plugin.NFC;
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
    public partial class AddChipPanel : ContentPage
    {
        private List<RoomPosition> positions = new List<RoomPosition>();
        private RoomPosition currentPosition;

        public AddChipPanel()
        {
            InitializeComponent();
            InitGUI();
            CreateButtons();

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

        private void CreateButtons()
        {
            LoadPositions();
            EmptyRooms.Children.Clear();
            foreach (var pos in positions)
            {
                var position = pos;
                var button = new Button
                {
                    Text = pos.RoomName + "\n" + pos.PositionNumber,
                    CornerRadius = 0,
                    Margin = 0,
                    Command = new Command(() => {
                        ChooseRoom(position);
                    })
                };
                EmptyRooms.Children.Add(button);
            }
        }

        private async void LoadPositions()
        {
            Tuple<APIController.Response, string, RoomPosition[]> result = await APIController.GetPositionsWithoutTag();
            await DisplayAlert(title: result.Item1.ToString(), message: result.Item2.ToString(), cancel: "Continue");
            positions = result.Item3.ToList();
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

            positions.Remove(currentPosition);
            Room_Label.Text = "";
            Application.Current.MainPage.DisplayAlert("Success", "Succesfully assigned tag " + tag.TagId + " to " + currentPosition.RoomName + "/" + currentPosition.PositionNumber, "Ok");
            currentPosition = null;
            CreateButtons();
        }

        public void StopAll()
        {
            NFCController.StopAll();
        }

        private void ChooseRoom(RoomPosition position)
        {
            currentPosition = position;
            Room_Label.Text = "Choosen " + position.RoomName + "/" + position.PositionNumber;
            StartWriting_Button.BackgroundColor = Color.FromHex("008B8B");
            StartWriting_Button.IsEnabled = NFCController.IsEnabled;
            ImageWait.IsVisible = true;
            ImageOk.IsVisible = false;
        }

        private void LockButtons()
        {
            foreach (var child in EmptyRooms.Children)
                child.IsEnabled = false;
        }



    }
}
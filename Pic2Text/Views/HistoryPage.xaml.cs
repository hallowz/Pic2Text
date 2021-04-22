using Pic2Text.Models;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2Text.Views
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();

            List<P2T> history = App.getHistory();

            StackLayout sl = buttonStack;

            foreach(P2T p in history)
            {
                ImageButton imageButton = new ImageButton
                {
                    Source = p.imageLocation,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(110, 10, 110, 10),
                    BorderColor = Color.Black,
                    BackgroundColor = Color.Gold,
                    BorderWidth = 5,
                };
                imageButton.ClassId = "" + p.ID;
                imageButton.Clicked += OnImageButtonClicked;
                sl.Children.Add(imageButton);
            }
        }
        async void OnImageButtonClicked(object sender, System.EventArgs e)
        {
            ImageButton ie = (ImageButton)sender;
            P2T p = App.getP2T(int.Parse(ie.ClassId));
            await Navigation.PushAsync(new ElementPage(p.imageLocation, p.Title, p.Text));
        }
        async void GalleryButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new GalleryPage());
        }
        async void CameraButton_Clicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new GalleryPage());
        }
    }
}
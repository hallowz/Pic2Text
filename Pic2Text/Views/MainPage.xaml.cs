using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2Text.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void GalleryButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new GalleryPage());
        }
        async void CameraButton_Clicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new GalleryPage());
        }
        async void HistoryButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
    }
}
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2Text.Views
{
    public partial class GalleryPage : ContentPage
    {
        ImageSource placeholder;
        public GalleryPage()
        {
            InitializeComponent();
            placeholder = resultImage.Source;
        }

        async void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please choose a photo"
            });

            

            try
            {
                var stream = await result.OpenReadAsync();
                resultImage.Source = ImageSource.FromStream(() => stream);
            }
            catch
            {
                await DisplayAlert("Alert", "No picture has been selected", "OK");
            }
            
        }

        async void ConfirmButton_Clicked(object sender, System.EventArgs e)
        {
            if (resultImage.Source.Equals(placeholder))
            {
                await DisplayAlert("Alert", "No picture has been selected", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Success?", "OK");
                //await Navigation.PushAsync(new ElementPage(resultImage.Source));
            }
        }
    }
}
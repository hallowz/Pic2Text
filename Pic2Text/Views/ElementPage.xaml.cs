using Pic2Text.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2Text.Views
{
    public partial class ElementPage : ContentPage
    {
        public ElementPage()
        {
            InitializeComponent();
            imageToRead.Source = "placeholder.png";
            
            //addP2T(Content);
        }
        public ElementPage(string fileName)
        {
            InitializeComponent();
            imageToRead.Source = ImageSource.FromFile(fileName);

            var x = true;
            do
            {
                if (App.getHistorySize() != 0)
                {
                    x = false;
                }
            } while (x);
            P2T currentP2T = App.history[App.history.Count - 1];
            contentText.Text = currentP2T.Text;
            //contentText.Text = "test";
            //addP2T(Content);
        }

        async void GalleryButton_Clicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new GalleryPage());
        }
        async void CameraButton_Clicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new GalleryPage());
        }
        async void LoadNote(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                // Retrieve the note and set it as the BindingContext of the page.
                P2T p2t = await App.Database.GetNoteAsync(id);
                BindingContext = p2t;
            }
            catch (Exception)
            {
                await DisplayAlert("Alert", "Something went wrong", "OK");
            }
        }

        async void addP2T(string content)
        {
            var p2t = (P2T)BindingContext;

            p2t.Text = content;

            p2t.Title = "New Pic2Text";

            if (!string.IsNullOrWhiteSpace(p2t.Text))
            {
                await App.Database.SaveElementAsync(p2t);
            }

            // Navigate backwards
            //await Shell.Current.GoToAsync("..");
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var p2t = (P2T)BindingContext;


            await App.Database.DeleteElementAsync(p2t);

            // Navigate backwards
            //await Shell.Current.GoToAsync("..");
        }
    }
}
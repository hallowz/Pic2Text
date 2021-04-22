using Pic2Text.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2Text.Views
{
    public partial class ElementPage : ContentPage
    {
        private int id;
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
            id = App.getHistorySize() - 1;
            P2T currentP2T = App.history[id];
            contentText.Text = currentP2T.Text;
            //contentText.Text = "test";
            //addP2T(Content);
        }

        public ElementPage(string fileName, string title, string content)
        {
            InitializeComponent();
            imageToRead.Source = ImageSource.FromFile(fileName);

            contentText.Text = content;
            elementTitle.Text = title;
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

        void EditorCompleted(object sender, EventArgs e)
        {
            if (sender.Equals(contentText))
            {
                P2T p = App.getP2T(id);
                p.Text = contentText.Text;
                App.setP2T(p, id);
            } 
            else if (sender.Equals(elementTitle))
            {
                P2T p = App.getP2T(id);
                p.Title = elementTitle.Text;
                App.setP2T(p, id);
            }
        }
        void EditorTextChanged(object sender, TextChangedEventArgs e)
        {
            var oldText = e.OldTextValue;
            var newText = e.NewTextValue;

            if (sender.Equals(contentText))
            {
                P2T p = App.getP2T(id);
                p.Text = newText;
                App.setP2T(p, id);
            }
            else if (sender.Equals(elementTitle))
            {
                P2T p = App.getP2T(id);
                p.Title = newText;
                App.setP2T(p, id);
            }
        }

        void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var oldText = e.OldTextValue;
            var newText = e.NewTextValue;

            if (sender.Equals(contentText))
            {
                P2T p = App.getP2T(id);
                p.Text = newText;
                App.setP2T(p, id);
            }
            else if (sender.Equals(elementTitle))
            {
                P2T p = App.getP2T(id);
                p.Title = newText;
                App.setP2T(p, id);
            }
        }

        async void CopyButton_Clicked(object sender, System.EventArgs e)
        {
            await Clipboard.SetTextAsync(contentText.Text);
        }
    }
}
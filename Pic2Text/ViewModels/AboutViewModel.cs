using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2Text.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.google.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
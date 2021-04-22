using Android.Graphics;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.Search.Models;
using Microsoft.CodeAnalysis.Diagnostics;
using Pic2Text.Models;
using Plugin.Connectivity;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Path = Android.Graphics.Path;

namespace Pic2Text.Views
{
    public partial class GalleryPage : ContentPage
    {
        ImageSource placeholder;

        static string subscriptionKey = "febd9d9c459a4490bd940605166f4d78";
        static string endpoint = "https://hallowzreadimage.cognitiveservices.azure.com/";

        //private string READ_TEXT_URL_IMAGE = "placeholder.png";

        public string content { get; set; }

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
                Save(stream, "temp");
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filename = System.IO.Path.Combine(path, "temp" + App.getHistorySize() + ".jpg");
                resultImage.Source = ImageSource.FromFile(filename);
                //READ_TEXT_URL_IMAGE = (string)filename;
            }
            catch (Exception err)
            {
                await DisplayAlert("Alert", "No picture has been selected\n" + err, "OK");
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
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filename = System.IO.Path.Combine(path, "temp" + App.getHistorySize() + ".jpg");
                readImage(filename);
                await Navigation.PushAsync(new ElementPage(filename));
            }
        }

        public void Save(Stream stream, string _name)
        {
            var _filename = _name + App.getHistorySize() + ".jpg";

            if (_filename.ToLower().Contains(".jpg") || _filename.ToLower().Contains(".png"))
            {
                stream.Position = 0;
                var bitmap = BitmapFactory.DecodeStream(stream);

                var finalStream = new MemoryStream();

                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 25, finalStream);
                bitmap = null;

                finalStream.Position = 0;

                var path2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filename2 = System.IO.Path.Combine(path2, _filename);

                using (var fileStream = File.Create(filename2))
                {
                    finalStream.Seek(0, SeekOrigin.Begin);
                    finalStream.CopyTo(fileStream);
                    fileStream.Close();

                    finalStream.Dispose();
                    stream.Dispose();
                    fileStream.Dispose();
                    GC.Collect();
                }
                return;
            }

            stream.Position = 0;

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filename = System.IO.Path.Combine(path, _filename);

            using (var fileStream = File.Create(filename))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();

                stream.Dispose();
                fileStream.Dispose();
                GC.Collect();
            }
        }

        void readImage(string READ_IMAGE)
        {
            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            // Extract text (OCR) from a URL image using the Read API
            System.Threading.CancellationToken ct = default;
            Task.Run(async () =>
            {
                try
                {
                    ReadFileLocal(client, READ_IMAGE).Wait();
                }
                catch (System.OperationCanceledException ex)
                {
                    Console.WriteLine($"Text load cancelled: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }, ct);
        }

        public ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public async Task ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            FileStream fs = File.OpenRead(localFile);
            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(fs, language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            //Thread.Sleep(2000);

            // <snippet_extract_response>
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
                Console.WriteLine("Waiting for results");
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            // </snippet_extract_response>

            // <snippet_extract_display>
            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;

            string content = "";
            if (textUrlFileResults != null)
            {
                foreach (ReadResult page in textUrlFileResults)
                {
                    foreach (Line line in page.Lines)
                    {
                        content += line.Text + "\n";
                    }
                }
            }
            
            //addP2T(content);
            App.addP2T(new P2T() { Text = content, Title = "New P2T", imageLocation = localFile });
        }

        static async void addP2T(string content)
        {
            var p2t = new P2T();

            p2t.Text = content;

            p2t.Title = "New Pic2Text";

            if (!string.IsNullOrWhiteSpace(p2t.Text))
            {
                await App.Database.SaveElementAsync(p2t);
            }

            // Navigate backwards
            //await Shell.Current.GoToAsync("..");
        }
    }
}
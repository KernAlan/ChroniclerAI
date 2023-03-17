using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoshAndAlanTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string? ApiKey { get; set; }
        public string? AudioFilePathTextBox { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        private async void Transcribe(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Transcribe();
                // Do something with the transcribed text result
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transcription failed: {ex.Message}");
            }
        }

        private async Task<string> Transcribe()
        {
            try
            {
                if (AudioFilePathTextBox is null)
                {
                    throw new ArgumentNullException("No audio file detected!");
                }

                if (ApiKey is null)
                {
                    throw new ArgumentNullException("No API key detected!");
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "TOKEN");

                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(File.OpenRead(AudioFilePathTextBox)), "file", System.IO.Path.GetFileName(AudioFilePathTextBox));
                        content.Add(new StringContent("whisper-1"), "model");

                        using (var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", content))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                return responseContent;
                            }
                            else
                            {
                                throw new Exception($"Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }


        private void Browse(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                AudioFilePathTextBox = openFileDialog.FileName;
            }
        }
    }
}

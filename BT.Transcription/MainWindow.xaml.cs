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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string? _apiKey;
        private string? _audioFilePath;
        private string? _outputText;

        public string? ApiKey
        {
            get => _apiKey;
            set
            {
                if (_apiKey != value)
                {
                    _apiKey = value;

                    // Trim the API key if it has leading or trailing whitespace
                    if (_apiKey is not null)
                    {
                        _apiKey = _apiKey.Trim();
                    }
                    
                    OnPropertyChanged(nameof(ApiKey));
                }
            }
        }

        public string? AudioFilePath
        {
            get => _audioFilePath;
            set
            {
                if (_audioFilePath != value)
                {
                    _audioFilePath = value;
                    OnPropertyChanged(nameof(AudioFilePath));
                }
            }
        }

        public string? OutputText
        {
            get => _outputText;
            set
            {
                if (_outputText != value)
                {
                    _outputText = value;
                    OnPropertyChanged(nameof(OutputText));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
                OutputText += "\n" + result;
                
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
                if (AudioFilePath is null)
                {
                    throw new ArgumentNullException("No audio file detected!");
                }

                if (ApiKey is null)
                {
                    throw new ArgumentNullException("No API key detected!");
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(File.OpenRead(AudioFilePath)), "file", System.IO.Path.GetFileName(AudioFilePath));
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
                                var responseContent = await response.Content.ReadAsStringAsync();
                                throw new Exception($"Request failed with status code {response.StatusCode}: {responseContent}");
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
                AudioFilePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(AudioFilePath));
            }
        }
    }
}

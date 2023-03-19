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
using System.Collections.Generic;

namespace ChroniclerAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string? _apiKey;
        private string? _audioFilePath;
        private List<string> _outputText = new List<string>();
        private static string _recordedAudioFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recordedaudio.mp3");
        private AudioRecorder _recorder;
        private bool _isRecording;
        private HttpClient _client = new HttpClient();

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

                    if (_apiKey is not null && _apiKey.Length > 0)
                    {
                        SaveApiKeyToFile(_apiKey);
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

        public List<string> OutputText
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
        
        public string OutputTextString
        {
            get => string.Join(Environment.NewLine, _outputText);
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadApiKeyFromFile();
            _isRecording = false;
            _recorder = new AudioRecorder(_recordedAudioFilePath);
            _client.Timeout = TimeSpan.FromSeconds(600);
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
                if (string.IsNullOrEmpty(AudioFilePath))
                {
                    throw new ArgumentNullException("No audio file detected!");
                }
                TranscribeButton.IsEnabled = false;
                TranscribeButton.Content = "Transcribing...";
                var result = "";

                // If the audio file exceeds 25mb, confirm to the user, and split it into 10 min files, and process each one individualls
                if (new FileInfo(AudioFilePath).Length > 25 * 1024 * 1024)
                {
                    MessageBoxResult confirmation = MessageBox.Show("Warning: this file is over 25mb. You can either split this file yourself, or Chronicler will split it into 10 min chunks for you. Is this OK?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (confirmation == MessageBoxResult.No)
                    {
                        return;
                    }
                    var splitAudioFilePaths = SplitFile();
                    foreach (var file in splitAudioFilePaths)
                    {
                        AudioFilePath = file;
                        result = await ProcessTranscribe();
                        OutputText.Add(result);
                    }
                }
                else
                {
                    result = await ProcessTranscribe();
                    OutputText.Add(result);
                }
                
                if (result is null)
                {
                    throw new ArgumentNullException("Transcription returned empty!");
                }
                
                OnPropertyChanged(nameof(OutputText));
                UpdateOutputTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transcription failed: {ex.Message}");
            }
            finally
            {
                TranscribeButton.IsEnabled = true;
                TranscribeButton.Content = "Transcribe";
            }
        }

        private async Task<string> ProcessTranscribe()
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
                        content.Add(new StringContent("text"), "response_format");

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

        private void UpdateOutputTextBox()
        {
            OnPropertyChanged(nameof(OutputTextString));
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

        public void SaveApiKeyToFile(string apiKey)
        {
            try
            {
                File.WriteAllText("apiKey.txt", apiKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API Key save failed: {ex.Message}");
            }
        }

        public void LoadApiKeyFromFile()
        {
            try
            {
                string apiKeyFromFile = File.ReadAllText("apiKey.txt");
                ApiKey = apiKeyFromFile;
            }
            catch (Exception)
            {
                // If we catch an error reading the file, assume this is the first time the app has been run, and create the initial .txt file
                SaveApiKeyToFile("API Key");
            }

        }

        public void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_isRecording)
                {
                    _recorder.StartRecording();
                    RecordButton.Content = "Stop Recording";
                    _isRecording = true;
                }
                else
                {
                    StopRecording(sender, e);
                    RecordButton.Content = "Start Recording";
                    _isRecording = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start recording: {ex.Message}");
            }
            
        }
        
        public void StopRecording(object sender, RoutedEventArgs e)
        {
            try
            {
                _recorder.StopRecording();
                AudioFilePath = _recordedAudioFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to stop recording: {ex.Message}");
            }
        }

        public List<string> SplitFile()
        {
            try
            {
                if (AudioFilePath is null)
                {
                    throw new ArgumentNullException("Cannot split an empty or nonexistent file.");
                }
                var audioSplitter = new AudioSplitter();
                var splitFilesList = audioSplitter.SplitAudio(AudioFilePath, Directory.GetCurrentDirectory(), TimeSpan.FromMinutes(10));
                return splitFilesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to split file: {ex.Message}");
                return new List<string>();
            }
        }
    }
}

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BT.Transcription
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Replace with your OpenAI API key
            string apiKey = "YOUR_OPENAI_API_KEY";

            // Replace with the model name and file path
            string modelName = "whisper-1";
            string filePath = "C:\\path\\to\\file\\openai.mp3";

            // Create a new HttpClient
            HttpClient client = new HttpClient();

            // Set the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            // Create a new multipart/form-data content
            MultipartFormDataContent content = new MultipartFormDataContent();

            // Add the model name field
            content.Add(new StringContent(modelName), "model");

            // Add the file field
            byte[] fileBytes = File.ReadAllBytes(filePath);
            ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
            content.Add(fileContent, "file", Path.GetFileName(filePath));

            // Send the API request
            HttpResponseMessage response = client.PostAsync("https://api.openai.com/v1/audio/transcriptions", content).Result;

            // Print the API response
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}

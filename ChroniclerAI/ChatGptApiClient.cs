using ChroniclerAI;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniclerAI
{
    public class ChatGptApiClient : IDisposable
    {
        private readonly string _apiKey;
        private readonly string _url;
        private int maxCharsPerChunk = 10000; // Estimated characters, assuming 5 characters per token
        private HttpClient? _httpClient;
        private bool _disposed = false;


        public ChatGptApiClient(string apiKey)
        {
            _apiKey = apiKey;
            _url = "https://api.openai.com/v1/chat/completions";
            InitializeHttpClient();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_httpClient != null)
                    {
                        _httpClient.Dispose();
                        _httpClient = null;
                    }
                }

                _disposed = true;
            }
        }

        private void InitializeHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GenerateCompletion(List<string> messages, ECompletionType completionType, string inputString = "")
        {
            var fullMessageBody = string.Join(", ", messages);
            if (fullMessageBody.Count() < maxCharsPerChunk)
            {
                var summary = await ProcessRequest(messages, completionType, inputString);
                return summary;
            }

            StringBuilder summarizedContext = new StringBuilder();

            // Generate summarized context
            foreach (string message in messages)
            {
                List<string> contextChunks = ChunkText(message);

                for (int i = 0; i < contextChunks.Count; i++)
                {
                    string chunk = contextChunks[i];
                    List<string> currentMessages = new List<string> { chunk };

                    if (summarizedContext.Length > 0)
                    {
                        currentMessages.Insert(0, summarizedContext.ToString());
                    }

                    string generatedSummary = await ProcessRequest(currentMessages, completionType, inputString);

                    summarizedContext.Append(generatedSummary);
                }
            }

            return summarizedContext.ToString();
        }


        public List<string> ChunkText(string text)
        {
            List<string> chunks = new List<string>();

            while (text.Length > 0)
            {
                int charCount = Math.Min(maxCharsPerChunk, text.Length);
                chunks.Add(text.Substring(0, charCount));
                text = text.Substring(charCount);
            }

            return chunks;
        }

        private async Task<string> ProcessRequest(List<string> messages, ECompletionType completionType, string inputString = "")
        {
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            var messageObjects = new List<object>();
            var systemPrompt = CreateSystemPrompt(completionType, inputString);
            messageObjects.Add(new { role = "system", content = systemPrompt });

            foreach (string message in messages)
            {
                messageObjects.Add(new { role = "user", content = message });
            }

            var request = CreateRequest(messageObjects, 500);
            var json = JsonConvert.SerializeObject(request);
            var contentData = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_url, contentData);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {response.StatusCode}: {response.ReasonPhrase} - {responseContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ChatResponse>(responseJson);

            if (responseObject == null || responseObject.Choices == null || responseObject.Choices[0].Message.Content == null)
            {
                throw new Exception("Error deserializing response JSON from OpenAI.");
            }
            response.Dispose();
            return responseObject.Choices[0].Message.Content;
        }

        public object CreateRequest(List<object> messageObjects, int maxTokens)
        {
            return new
            {
                model = "gpt-3.5-turbo", // Required: ID of the model to use
                messages = messageObjects, // Required: messages to generate chat completions for, in the chat format
                max_tokens = maxTokens, // Optional: maximum number of tokens to generate in the chat completion
                temperature = 0.7, // Optional: sampling temperature to use
                top_p = 1, // Optional: alternative to temperature
                n = 1, // Optional: how many chat completion choices to generate for each input message
                stream = false, // Optional: if set, partial message deltas will be sent
                stop = "", // Optional: up to 4 sequences where the API will stop generating further tokens
                frequency_penalty = 0, // Optional: penalizes new tokens based on their existing frequency in the text so far
                presence_penalty = 0, // Optional: penalizes new tokens based on whether they appear in the text so far
            };
        }

        public string CreateSystemPrompt(ECompletionType completion, string inputString = "")
        {
            var systemPrompt = "";

            switch (completion)
            {
                case ECompletionType.Summarize:
                    systemPrompt = "Summarize the following audio transcript with as much detail as possible: ";
                    break;
                case ECompletionType.Highlight:
                    systemPrompt = "Highlight key quotes from this transcript that would be important if a reader were trying to understand a summary of this text. Provide a brief description of why each quote is important and worthy of being highlighted: ";
                    break;
                case ECompletionType.Enumerate:
                    systemPrompt = "Enumerate the main points of this text in bullet points: ";
                    break;
                case ECompletionType.Ask:
                    systemPrompt = $"The following question or command is being asked of you: {inputString}. Please use the following transcript to answer the previous question or command:";
                    break;
                default:
                    break;
            }

            return systemPrompt;
        }

        public async Task<List<string>> FetchAvailableModels()
        {
            _httpClient.Timeout = TimeSpan.FromMinutes(10);

            var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {response.StatusCode}: {response.ReasonPhrase} - {responseContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ModelResponse>(responseJson);

            if (responseObject == null || responseObject.Data == null)
            {
                throw new Exception("Error deserializing response JSON from OpenAI.");
            }

            return responseObject.Data
                .Select(m => m.Id)
                .Where(id => id.Contains("gpt"))
                .ToList();
        }


    }


    public class ChatResponse
    {
        public string? Id { get; set; }
        public string? Object { get; set; }
        public long Created { get; set; }
        public ChatChoice[]? Choices { get; set; }
        public Usage? Usage { get; set; }
    }

    public class ChatChoice
    {
        public int Index { get; set; }
        public Message? Message { get; set; }
        public string? FinishReason { get; set; }
    }

    public class Message
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }

    public class ModelResponse
    {
        public ModelData[] Data { get; set; }
    }

    public class ModelData
    {
        public string Id { get; set; }
    }

}
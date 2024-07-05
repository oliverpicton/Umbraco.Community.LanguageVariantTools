using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

public class OpenAiTranslationService : ITranslationService
{
    private readonly string _apiKey;

    private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";

    public OpenAiTranslationService()
    {
        var apiKey = Environment.GetEnvironmentVariable("LanguageVariantTools:OpenApiKey");

        _apiKey = apiKey ?? string.Empty;
    }

    public async Task<string> Translate(string inputText, string targetLanguageCode)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var requestBody = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"You are a translator that translates text into {targetLanguageCode}."
                },
                new
                {
                    role = "user",
                    content = inputText
                }
            },
            max_tokens = 150,
            temperature = 0.7
        };

        var response = await client.PostAsJsonAsync(_apiUrl, requestBody);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<JsonDocument>();
        string translatedText = responseBody.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString().Trim();

        return translatedText;
    }
}
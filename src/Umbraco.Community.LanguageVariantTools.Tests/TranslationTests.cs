public class TranslationTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TranslationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("This is a test text to translate.", "es-ES", "Spanish (Spain)")]
    [InlineData("This is a test text to translate.", "fr-FR", "French (France)")]
    [InlineData("This is a test text to translate.", "de-DE", "German (Germany)")]
    [InlineData("This is a test text to translate.", "it-IT", "Italian (Italy)")]
    [InlineData("This is a test text to translate.", "zh-CN", "Chinese (China)")]
    public async Task TestTranslation(string inputText, string targetLanguageCode, string targetLanguageDescription)
    {
        var translationService = new OpenAiTranslationService();
        string translatedText = await translationService.Translate(inputText, targetLanguageCode);
        Assert.False(string.IsNullOrEmpty(translatedText), $"Translation to {targetLanguageDescription} should not be null or empty.");
        _testOutputHelper.WriteLine($"Translation to {targetLanguageDescription}: {translatedText}");
    }
}
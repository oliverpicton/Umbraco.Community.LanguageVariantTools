
public interface ITranslationService
{
    public Task<string> Translate(string inputText, string targetLanguageCode);
}
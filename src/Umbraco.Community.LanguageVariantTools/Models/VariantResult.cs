using System.Text.Json.Serialization;

namespace Umbraco.Community.LanguageVariantTools.Models
{
    public class VariantResult
    {
        public bool IsSuccess { get; set; }

        public Error? Error { get; private set; }

        public void AddError(string message)
        {
            Error = new Error { Message = message };
        }
    }
}
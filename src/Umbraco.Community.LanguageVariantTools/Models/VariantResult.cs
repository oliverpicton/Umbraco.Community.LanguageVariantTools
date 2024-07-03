using System.Text.RegularExpressions;

namespace Umbraco.Community.LanguageVariantTools.Models
{
    public class VariantResult
    {
        public bool IsSuccess { get; set; }

        public Error? Error { get; private set; }

        public void AddError(string message, params object?[] arg)
        {
            if (arg != null && arg.Length > 0)
            {
                message = Regex.Replace(message, @"\{(\d+)\}", match =>
                {
                    int index = int.Parse(match.Groups[1].Value);
                    return index < arg.Length ? arg[index]?.ToString() ?? string.Empty : match.Value;
                });
            }

            Error = new Error { Message = message };
        }
    }
}
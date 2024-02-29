using DeepL;

namespace RssFeedTranslator.Utils.DeepL
{
    public class DeeplTranslator : ITranslator
    {
        private readonly Translator translator;

        public DeeplTranslator(string authKey)
        {
            translator = new Translator(authKey);
        }
        
        public async Task<string> Translate(string summary)
        {
            if (string.IsNullOrWhiteSpace(summary))
            {
                return summary;
            }

            summary = summary.Trim();

            // Translate text into a target language, in this case, French:
            var translatedText = await translator.TranslateTextAsync(
                  summary,
                  null,
                  LanguageCode.German);

            return translatedText.Text;
        }
    }
}

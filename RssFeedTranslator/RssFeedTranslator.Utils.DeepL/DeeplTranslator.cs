namespace RssFeedTranslator.Utils.DeepL
{
    public class DeeplTranslator : ITranslator
    {
        public string Translate(string summary)
        {
            if (string.IsNullOrWhiteSpace(summary))
            {
                return summary;
            }

            summary = summary.Trim();

            string translated = summary.Replace('a', 'ä')
                .Replace('o', 'ö')
                .Replace('u', 'ü');

            return translated;
        }
    }
}

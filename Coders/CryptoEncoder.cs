
using System.Text;
using System.Text.RegularExpressions;
using SubstitutionCipher.KeyGenerators;
using SubstitutionCipher.Managers;

namespace SubstitutionCipher.Coders
{
    public sealed class CryptoEncoder
    {
        private Dictionary<char, char> _encoder { get; }
        private Language _language { get; }
        public CryptoEncoder(string key, Language language)
        {
            _language = language;
            var values = key.ToList();
            if (values.Count != (int)_language)
            {
                throw new ArgumentException("Key is invalid");
            }
            var keys = KeyGeneratorFactory.KeyGeneratorForLanguage(_language).Alphabet();
            _encoder = keys
                .Zip(values, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
        }
        public static string TextFormater(string rawData) =>
            new Regex(@"(\W|\d)")
                .Replace(rawData, String.Empty)
                .ToLower();
        public string TextEncoder(string formatedData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Capacity = formatedData.Length;
            foreach (char ch in formatedData)
            {
                builder.Append(_encoder[ch]);
            }
            return builder.ToString();
        }
        public void EncodeFile(string textFile, string encodedFile)
        {
            var rawData = FileManager.Read(textFile);
            var formatedData = TextFormater(rawData);
            var encodedData = TextEncoder(formatedData);
            FileManager.Write(encodedFile, encodedData);
        }
    }
}

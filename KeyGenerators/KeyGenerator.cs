
using SubstitutionCipher.Managers;

namespace SubstitutionCipher
{
    public enum Language : int
    {
        English = 26,
        Russian = 33,
    }
}

namespace SubstitutionCipher.KeyGenerators
{
    public abstract class KeyGenerator
    {
        public abstract Language language { get; }
        public abstract List<char> Alphabet();
        public string RandomKey()
        {
            var alphabet = Alphabet();
            var random = new Random();
            for (int i = 0; i < alphabet.Count - 1; i++)
            {
                int pos = random.Next(i, alphabet.Count);
                (alphabet[i], alphabet[pos]) = (alphabet[pos], alphabet[i]);
            }
            return new string(alphabet.ToArray());
        }
        public void CreateKey(string fileName)
        {
            string russianKey = RandomKey();
            FileManager.Write(fileName, russianKey);
        }
        public static KeyGenerator KeyGeneratorForLanguage(Language language) =>
            language switch
            {
                Language.Russian => new RussianKeyGenerator(),
                Language.English => new EnglishKeyGenerator(),
                _ => throw new Exception($"KeyGenerator for {Enum.GetName(typeof(Language), language)} language is not supported"),
            };
    }
}

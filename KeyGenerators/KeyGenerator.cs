
using SubstitutionCipher.Managers;

namespace SubstitutionCipher
{
    public enum Language : int
    {
        English = 26,
        Russian = 33,
    }

    namespace KeyGenerators
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

        }
    }
}


namespace SubstitutionCipher.KeyGenerators
{
    public sealed class EnglishKeyGenerator : KeyGenerator
    {
        public override Language language { get; } = Language.English;
        public override List<char> Alphabet()
        {
            List<char> alphabet = Enumerable.Range(0, 26).Select((i, x) => (char)('a' + i)).ToList();
            return alphabet;
        }
    }
}

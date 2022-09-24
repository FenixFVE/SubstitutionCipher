
namespace SubstitutionCipher.KeyGenerators
{
    public sealed class RussianKeyGenerator : KeyGenerator
    {
        public override Language language { get; } = Language.Russian;
        public override List<char> Alphabet()
        {
            List<char> alphabet = Enumerable.Range(0, 32).Select((i, x) => (char)('а' + i)).ToList();
            alphabet.Insert(6, 'ё');
            return alphabet;
        }
    }
}

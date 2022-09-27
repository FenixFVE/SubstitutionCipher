
namespace SubstitutionCipher.KeyGenerators
{
    public sealed class KeyGeneratorFactory
    {
        public static KeyGenerator KeyGeneratorForLanguage(Language language) =>
            language switch
            {
                Language.Russian => new RussianKeyGenerator(),
                Language.English => new EnglishKeyGenerator(),
                _ => throw new Exception($"KeyGenerator for {Enum.GetName(typeof(Language), language)} language is not supported"),
            };
    }
}

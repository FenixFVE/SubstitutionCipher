
using SubstitutionCipher;
using SubstitutionCipher.Coders;
using SubstitutionCipher.GenticKeyFinder;
using SubstitutionCipher.KeyGenerators;
using SubstitutionCipher.Managers;
using System.Collections.Concurrent;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

if (false)
{
    var language = Language.Russian;

    var keyGenerator = new RussianKeyGenerator();
    keyGenerator.CreateKey(@".\Data\key.txt");

    var key = FileManager.Read(@".\Data\key.txt");

    var encoder = new CryptoEncoder(key, language);
    encoder.EncodeFile(@".\Data\text.txt", @".\Data\encoded_text.txt");

    var decoder = new CryptoDecoder(language);
    decoder.SetKey(key);
    decoder.DecodeFile(@".\Data\encoded_text.txt", @".\Data\decoded_text.txt");
}

if (false)
{
    var lanugage = Language.Russian;
    int nGrammLength = 2;
    int simualationSize = 100;
    double pressision = 0.000_000_1;

    var key = FileManager.Read(@".\Data\key.txt");
    Console.WriteLine($"key = '{key}'");

    var referenceText = FileManager.Read(@".\Data\reference_text.txt");

    var encoedText = FileManager.Read(@".\Data\encoded_text.txt");

    var keyFinder = new GeneticSimulation(referenceText, encoedText, lanugage, nGrammLength, simualationSize);

    var findedKey = keyFinder.RunSimulation(pressision);
    FileManager.Write(@".\Data\finded_key.txt", findedKey);

    Console.WriteLine($"key = '{key}'");
    Console.WriteLine($"finded_key = '{findedKey}'");
}

if (true)
{
    var dictionary = new ConcurrentDictionary<char[], int>();
    var key = new char[] { 'A', 'B' };
    var value = 8;
    dictionary.TryAdd(key, value);

    Console.WriteLine(dictionary[key]);

    var qu = new Queue<char>();
    qu.Enqueue('A');
    qu.Enqueue('B');

    Console.WriteLine(dictionary[qu.ToArray()]);
}
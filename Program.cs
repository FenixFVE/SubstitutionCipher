
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

if (true)
{
    var lanugage = Language.Russian;
    int nGrammLength = 3;
    int simualationSize = 200;
    double pressision = 0.000_01;

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

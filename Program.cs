
using SubstitutionCipher;
using SubstitutionCipher.Coders;
using SubstitutionCipher.KeyGenerators;
using SubstitutionCipher.Managers;

var language = Language.Russian;

var keyGenerator = new RussianKeyGenerator();
keyGenerator.CreateKey(@".\Data\key.txt");

var key = FileManager.Read(@".\Data\key.txt");

var encoder = new CryptoEncoder(key, language);
encoder.EncodeFile(@".\Data\text.txt", @".\Data\encoded_text.txt");

var decoder = new CryptoDecoder(language);
decoder.SetKey(key);
decoder.DecodeFile(@".\Data\encoded_text.txt", @".\Data\decoded_text.txt");
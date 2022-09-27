
using SubstitutionCipher.Coders;
using System.Collections.Concurrent;
using System.Text;
using SubstitutionCipher.KeyGenerators;

namespace SubstitutionCipher.GenticKeyFinder
{
    public sealed class GeneticSimulation
    {
        private readonly Language _language;
        private readonly List<char> _alphabet;
        private readonly int _nGrammLength;
        private readonly ConcurrentDictionary<char[], int> _nGrammPositions;
        private readonly string _referenceText;
        private readonly string _encodedText;
        private readonly double[] _referenceFrequencies;
        private readonly int _simulationSize;
        private List<Phenotype> _phenotypes;

        public GeneticSimulation(string referenceText, string encodedText, Language language, int nGrammLength, int simulationSize)
        {
            _language = language;
            _alphabet = KeyGeneratorFactory.KeyGeneratorForLanguage(_language).Alphabet();
            _nGrammLength = nGrammLength;
            _simulationSize = simulationSize;
            _referenceText = CryptoEncoder.TextFormater(referenceText);
            _encodedText = CryptoEncoder.TextFormater(encodedText);
            _nGrammPositions = InitializeNGrammPositions(_alphabet, _nGrammLength);
            _referenceFrequencies = ComputeFrequencies(_referenceText, _nGrammLength, _nGrammPositions);
            _phenotypes = new List<Phenotype>(simulationSize);
            for (int i = 0; i < _simulationSize; i++)
            {
                _phenotypes.Add(new Phenotype(_language, _nGrammLength, _encodedText, _nGrammPositions, _referenceFrequencies, _alphabet));
            }
        }

        public static ConcurrentDictionary<char[], int> InitializeNGrammPositions(List<char> alphabet, int nGrammLength) 
        {
            var dictionarySize = (int)Math.Pow(alphabet.Count, nGrammLength);
            var keys = new List<char[]>(dictionarySize);

            for (int i = 0; i < dictionarySize; i++)
            {
                keys.Add(new char[nGrammLength]);
            }

            for (int grammPosition = 0; grammPosition < nGrammLength; grammPosition++)
            {
                int repeats = (int)Math.Pow(alphabet.Count, grammPosition);
                int cycles = (dictionarySize / alphabet.Count) / repeats;
                int counter = 0;
                for (int i = 0; i < cycles; i++)
                {
                    for (int letter = 0; letter < alphabet.Count; letter++)
                    {
                        for (int j = 0; j < repeats; j++, counter++)
                        {
                            keys[counter][grammPosition] = alphabet[letter];
                        }
                    }
                }
            }

            var dictionary = new ConcurrentDictionary<char[], int>();
            for (int i = 0; i < dictionarySize; i++)
            {
                dictionary.TryAdd(keys[i], i);
            }
            return dictionary;
        }

        public static double[] ComputeFrequencies(string formatedText, int nGrammLength, ConcurrentDictionary<char[], int> positions)
        {
            var frequencies = new double[positions.Count];
            var qu = new Queue<char>(nGrammLength);
            for (int i = 0; i < nGrammLength - 1; i++)
            {
                qu.Enqueue(formatedText[i]);
            }

            for (int i = nGrammLength - 1; i < formatedText.Length; i++)
            {
                qu.Enqueue(formatedText[i]);
                //Console.WriteLine(positions.Count);
                var position = positions[qu.ToArray()];
                qu.Dequeue();
                frequencies[position]++;
            }

            double div = frequencies.Sum();

            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] /= div;
            }

            return frequencies;
        }

        public string RunSimulation(double precision)
        {
            int counter = 0;
            var rand = new Random();

            while(true)
            {
                Parallel.ForEach(_phenotypes, individual =>
                {
                    individual.ComputeFitnessValue();
                });

                _phenotypes = _phenotypes.OrderBy(x => x.FitnessValue).ToList();
                double best = _phenotypes[0].FitnessValue;
                double average = _phenotypes.Average(x => x.FitnessValue);

                Console.WriteLine($"{++counter} generation: best_key = {_phenotypes[0].Genotype}, best = {best}, average = {average}");

                if (best <= precision) break; // EXIT

                int half = _phenotypes.Count / 2;
                for (int i = 0; i < half; i++)
                {
                    var genotype = _phenotypes[i].Genotype;
                    int t1 = rand.Next(0, (int)_language);
                    int t2 = rand.Next(0, (int)_language);
                    var strBuilder = new StringBuilder(genotype);
                    (strBuilder[t1], strBuilder[t2]) = (strBuilder[t2], strBuilder[t1]); 
                    _phenotypes[i + half].Genotype = strBuilder.ToString();
                }
            }

            return _phenotypes[0].Genotype;
        }
    }
}

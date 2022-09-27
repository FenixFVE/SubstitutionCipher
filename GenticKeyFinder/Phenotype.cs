
using SubstitutionCipher.KeyGenerators;
using System.Collections.Concurrent;


namespace SubstitutionCipher.GenticKeyFinder
{
    public sealed class Phenotype
    {
        private readonly Language _language;
        private readonly List<char> _alphabet;
        private readonly string _encodedText;
        private readonly int _nGrammLength;
        private readonly ConcurrentDictionary<string, int> _nGrammPositions;
        private readonly double[] _referenceNGrammFrequencies;

        private double[] _nGrammFrequencies; 
        private string _cryptoKey;
        private Dictionary<char, char> _decoder;
        private bool _isFitnessValueComputed;
        private double _fitnessValue;
        public string Genotype
        {
            get => _cryptoKey;
            set
            {
                _cryptoKey = value;
                _decoder = _cryptoKey
                    .ToList()
                    .Zip(_alphabet, (k, v) => new { k, v })
                    .ToDictionary(x => x.k, x => x.v);
                _isFitnessValueComputed = false;
            }
        }   
        public double FitnessValue
        {
            get
            {
                //if (!_isFitnessValueComputed)
                //    ComputeFitnessValue();
                return _fitnessValue;
            }
        }

        public Phenotype(
            Language language, int nGrammLength, string encodedText,
            ConcurrentDictionary<string, int> nGrammPositions,
            double[] referenceNGrammFrequencies,
            List<char> alphabet)
        {
            _language = language;
            _nGrammLength = nGrammLength;
            _encodedText = encodedText;
            _nGrammPositions = nGrammPositions;
            _referenceNGrammFrequencies = referenceNGrammFrequencies;
            int frequenciesCount = (int)Math.Pow((int)_language, _nGrammLength);
            _nGrammFrequencies = new double[frequenciesCount];
            _cryptoKey = KeyGeneratorFactory.KeyGeneratorForLanguage(_language).RandomKey();
            _alphabet = alphabet;
            _decoder = _cryptoKey
                .ToList()
                .Zip(_alphabet, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
            _isFitnessValueComputed = false;
            _fitnessValue = double.PositiveInfinity;
        }

        public void ComputeFitnessValue()
        {
            if (_isFitnessValueComputed) return;

            Array.Clear(_nGrammFrequencies, 0, _nGrammFrequencies.Length);

            var qu = new Queue<char>(_nGrammLength);
            for (int i = 0; i < _nGrammLength - 1; i++)
            {
                qu.Enqueue(_decoder[_encodedText[i]]);
            }
            
            for (int i = _nGrammLength - 1; i < _encodedText.Length; i++)
            {
                qu.Enqueue(_decoder[_encodedText[i]]);
                var position = _nGrammPositions[new string(qu.ToArray())];
                qu.Dequeue();
                _nGrammFrequencies[position]++;
            }

            double div = _nGrammFrequencies.Sum();

            double sum = 0.0;
            for (int i = 0; i < _nGrammFrequencies.Length; i++)
            {
                double dif = _nGrammFrequencies[i] / div - _referenceNGrammFrequencies[i];
                sum += dif * dif;
            }

            _fitnessValue = Math.Sqrt(sum);

            _isFitnessValueComputed = true;
        }

    }
}

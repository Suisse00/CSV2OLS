using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSV2OLS
{
    class Frequency
    {
        #region Fields

        private int _hertz;

        private decimal _frequence;
        private FrequencyUnit _frequenceUnit;

        #endregion // Fields

        #region Properties

        public static Frequency Zero = new Frequency(0, FrequencyUnit.Hz);

        public int Hertz { get { return _hertz; } }

        public decimal Frequence { get { return _frequence; } }
        public FrequencyUnit FrequenceUnit { get { return _frequenceUnit; } }

        #endregion // Properties

        public Frequency(int hz)
        {
            _frequence = hz;
            _frequenceUnit = FrequencyUnit.Hz;
            _hertz = hz;
        }

        public Frequency(decimal value, FrequencyUnit unit)
        {
            _frequence = value;
            _frequenceUnit = unit;
            _hertz = (int)(value * (int)unit);
        }

        public static Frequency Parse(string input)
        {
            Match match = new Regex("^([0-9\\.]*)(.*)$")
                                .Match(input);

            string frequencyUnitString = match.Groups[2].Value.Trim();

            decimal frequence = decimal.Parse(match.Groups[1].Value);
            FrequencyUnit frequenceUnit = String.IsNullOrEmpty(frequencyUnitString) 
                                ? FrequencyUnit.Hz 
                                : (FrequencyUnit)Enum.Parse(typeof(FrequencyUnit), frequencyUnitString, true);

            return new Frequency(frequence, frequenceUnit);
        }

        public override string ToString()
        {
            return _frequence.ToString() + _frequenceUnit.ToString();
        }
    }
}

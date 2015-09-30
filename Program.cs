using NDesk.Options;
using Suisse00.Hardware.Quantities;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSV2OLS
{
    internal class Program
    {
        private Frequency _frequency = new Frequency(100, FrequencyUnit.MHz);
        private int[] _channelsToUse = new int[] { 1 };
        private double _highSignalVoltage = 0.1;
        private string[] _delimiters = new string[] { "," };
        private int _linesToSkip = 2;

        private const char CHANNEL_SEPARATOR = ',';

        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private void Run(string[] args)
        {
            OptionSet options = GetOptions();

            try
            {
                options.Parse(args);

                Console.WriteLine(";Rate: " + _frequency.Hertz.ToString());
                Console.WriteLine(";Channels: " + _channelsToUse.Length.ToString());

                string OLSOutputFormat = "{0:x" + Math.Ceiling((decimal)_channelsToUse.Length / 8).ToString() + "}@{1}";
                int indexLine = 0;

                for (string line = Console.ReadLine(); line != null; line = Console.ReadLine())
                {
                    if (_linesToSkip > 0)
                    {
                        _linesToSkip--;

                        continue;
                    }

                    Console.WriteLine(String.Format(OLSOutputFormat,
                                                    GetOLSValue(line.Split(_delimiters, StringSplitOptions.None), _channelsToUse),
                                                    indexLine));

                    indexLine++;
                }
            }
            catch (OptionException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("Try --help");
            }
        }

        private OptionSet GetOptions()
        {
            OptionSet options = new OptionSet()
            {
                {
                    "f|frequency=", 
                    String.Format("The frequency of of simpling (default: {0})", 
                                  _frequency.ToString()),
                    v => _frequency = Frequency.Parse(v)
                },
                {
                    "c|channels=",
                    String.Format("Channels to use [1, X] divized by a comma (default: {0})",
                                  String.Join(CHANNEL_SEPARATOR.ToString(), _channelsToUse)),

                    v => _channelsToUse = v.Split(CHANNEL_SEPARATOR)
                                         .Select(channel => int.Parse(channel))
                                         .ToArray()
                },
                {
                    "hs|highsignal=",
                    String.Format("Threshold which set the signal to high (default: {0})", 
                                  _highSignalVoltage),
                    v => _highSignalVoltage = double.Parse(v)
                },
                {
                    "d|delimiter=",
                    String.Format("Column delimiter (default: {0})",
                                  _delimiters.First()),
                    v => _delimiters = new string[] { v }
                },
                {
                    "s|skipline=",
                    String.Format("Number of line to skip from the input (default: {0})",
                                  _linesToSkip),
                    v => _linesToSkip = int.Parse(v)
                }
            };

            options.Add("help",
                        "Display available commands",
                        v =>
                        {
                            options.WriteOptionDescriptions(Console.Out);

                            Environment.Exit(0);
                        });

            return options;
        }

        public static int GetOLSValue(string[] rawValues, int[] channelsToUse, double highSignalVoltage)
        {
            int result = 0;

            foreach (int channelIndex in channelsToUse)
            {
                int booleanValue = 0;

                if (rawValues.Length > channelIndex)
                {
                    booleanValue = (String.IsNullOrEmpty(rawValues[channelIndex])
                                        ? 0
                                        : double.Parse(rawValues[channelIndex]))

                                        > highSignalVoltage

                                    ? 1
                                    : 0;
                }

                result |= booleanValue << channelIndex - 1;
            }

            return result;
        }

        private int GetOLSValue(string[] rawValues, int[] channelsToUse)
        {
            return GetOLSValue(rawValues, channelsToUse, _highSignalVoltage);
        }
    }
}

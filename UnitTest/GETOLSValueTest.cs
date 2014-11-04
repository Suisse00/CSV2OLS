using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace CSV2OLS.UnitTest
{
    [TestClass]
    public class GetOLSValueTest
    {
        public const double HIGH_SIGNAL_VOLTAGE = 0.1;

        [TestMethod]
        public void TestChannel1()
        {
            int[] channelsToUser = new int[] { 1 };

            Test(new string[] { "-2.99890e-01", "-4.00e-02" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false });
            Test(new string[] { "-2.99890e-01", "3.80e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { true });
            Test(new string[] { "-2.99890e-01", "0.0e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false });
            Test(new string[] { "-2.99890e-01", "0.2e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { true });
            Test(new string[] { "-2.99890e-01", "" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false });
        }

        [TestMethod]
        public void TestChannel2()
        {
            int[] channelsToUser = new int[] { 2 };

            Test(new string[] { "-2.99890e-01", "3.80e-02" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false });
            Test(new string[] { "-2.99890e-01", "", "3.80e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { true });
            Test(new string[] { "-2.99890e-01", "", "0.00e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false });
            Test(new string[] { "-2.99890e-01", "", "0.2e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { true });
        }

        [TestMethod]
        public void TestChannels()
        {
            int[] channelsToUser = new int[] { 1, 2 };

            Test(new string[] { "-2.99890e-01", "-4.00e-02" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false, false });
            Test(new string[] { "-2.99890e-01", "3.80e+00", "-4.00e-02" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { true, false });
            Test(new string[] { "-2.99890e-01", "0.0e+00", "3.80e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false, true });
            Test(new string[] { "-2.99890e-01", "0.2e+00", "0.2e+00" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { true, true });
            Test(new string[] { "-2.99890e-01", "", "" }, channelsToUser, HIGH_SIGNAL_VOLTAGE, new bool[] { false, false });
        }

        private static void Test(string[] rawValue,
                                 int[] channelsToUse,
                                 double HIGH_SIGNAL_VOLTAGE,
                                 bool[] expectedResults)
        {
            int rawResult = Program.GetOLSValue(rawValue,
                                                channelsToUse,
                                                HIGH_SIGNAL_VOLTAGE);

            IEnumerator channelsToUseEnumerator = channelsToUse.GetEnumerator();

            foreach (bool expectedResult in expectedResults)
            {
                if (!channelsToUseEnumerator.MoveNext())
                {
                    System.Diagnostics.Debug.Fail("Malformed test, expected a result array of the same size as channelsToUse.");

                    break;
                }

                int channel = (int)channelsToUseEnumerator.Current;
                int channelIndex = channel - 1;
                int mask = 1 << channelIndex;

                System.Diagnostics.Debug.Assert((rawResult & mask)
                                                    == ((expectedResult ? 1 : 0) << channelIndex),

                                                String.Format("Channel {0} expected to be {1} but was {2}."
                                                                + Environment.NewLine
                                                                + "High Voltage : {3}"
                                                                + Environment.NewLine
                                                                + "Raw Value: {4}",

                                                                channel,
                                                                expectedResult 
                                                                    ? "High" 
                                                                    : "Low",

                                                                (rawResult & mask) != 0 
                                                                    ? "High" 
                                                                    : "Low",

                                                                HIGH_SIGNAL_VOLTAGE,
                                                                rawValue.Length <= channel 
                                                                    ? "None" 
                                                                    : rawValue[channel]));
            }
        }
    }
}

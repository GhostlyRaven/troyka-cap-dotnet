using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using System.Threading.Tasks;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example5
{
    public static class Program
    {
        private static ushort Pin = 0;
        private static ushort Freq = 250; //Гц
        private static double DutyCycle = 0.5;

        private static readonly IGpioExpander Expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            Expander = Pi.I2C.CreateGpioExpander();
            //Expander = Pi.I2C.SafeCreateGpioExpander();
        }

        public static void Main()
        {
            Console.WriteLine("Start");

            Expander.PwmFreq(Freq);

            Expander.AnalogWrite(Pin, DutyCycle);

            Task.Delay(60000).Wait();

            Expander.AnalogWrite(Pin, 2 * DutyCycle);

            Task.Delay(60000).Wait();

            Expander.AnalogWrite(Pin, DutyCycle);

            Task.Delay(60000).Wait();

            Expander.AnalogWrite(Pin, 0);

            Console.WriteLine("Stop");
        }
    }
}

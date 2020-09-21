using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example5
{
    public static class Program
    {
        private static ushort Pin = 0;
        private static ushort Freq = 500; //Гц
        private static double DutyCycle = 0.5;

        private static readonly IGpioExpander Expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            Expander = Pi.I2C.CreateGpioExpander();
        }

        public static void Main()
        {
            Console.WriteLine("Start");

            Expander.PwmFreq(Freq);
            Expander.AnalogWrite(Pin, DutyCycle);

            Console.WriteLine("Stop");
        }
    }
}

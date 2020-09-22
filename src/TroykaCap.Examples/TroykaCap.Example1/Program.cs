using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example1
{
    public static class Program
    {
        private static ushort Pin1 = 0;
        private static ushort Pin2 = 1;

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

            while (Exit())
            {
                double result = Expander.AnalogRead(Pin1);

                Expander.AnalogWrite(Pin2, result);
            }

            Console.WriteLine("Stop");
        }

        private static bool Exit()
        {
            return !(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape);
        }
    }
}

using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example2
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
        }

        public static void Main()
        {
            Console.WriteLine("Start");

            Expander.PinMode(Pin1, PinMode.Input);
            Expander.PinMode(Pin2, PinMode.Output);

            while (Exit())
            {
                bool result = Expander.DigitalRead(Pin1);

                Expander.DigitalWrite(Pin2, result);
            }

            Console.WriteLine("Stop");
        }

        private static bool Exit()
        {
            return !(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape);
        }
    }
}

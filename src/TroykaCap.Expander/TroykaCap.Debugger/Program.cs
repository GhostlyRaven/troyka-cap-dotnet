using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Debugger
{
    public static class Program
    {
        private static ushort Pin1 = 0;
        private static ushort Pin2 = 1;
        private static ushort Pin3 = 2;
        private static ushort Pin4 = 3;

        private static readonly IGpioExpander Expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            Expander = Pi.I2C.GetGpioExpander();

            Expander.Error += Expander_Error;
        }

        public static void Main()
        {
            Console.WriteLine("Start");

            Expander.PinMode(Pin3, PinMode.Input);
            Expander.PinMode(Pin3, PinMode.Output);

            while (Exit())
            {
                double analogResult = Expander.AnalogRead(Pin1);

                Console.WriteLine("Analog result: {0}", analogResult);

                Expander.AnalogWrite(Pin2, analogResult);

                bool digitalResult = Expander.TroykaButtonClick(Pin3);

                Console.WriteLine("Analog result: {0}", digitalResult);

                Expander.DigitalWrite(Pin4, digitalResult);
            }

            Console.WriteLine("Stop");
        }

        private static bool Exit()
        {
            return !(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape);
        }

        private static void Expander_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
        }
    }
}

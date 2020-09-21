using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using System.Threading.Tasks;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example3
{
    public static class Program
    {
        private static readonly IGpioExpander Expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            Expander = Pi.I2C.CreateGpioExpander();
        }

        public static void Main()
        {
            Console.WriteLine("Start");

            Console.WriteLine($"Port: {Expander.DigitalReadPort()}");

            Expander.DigitalWritePort(255);

            Console.WriteLine($"Port: {Expander.DigitalReadPort()}");

            Task.Delay(60000).Wait();

            Expander.DigitalWritePort(0);

            Console.WriteLine($"Port: {Expander.DigitalReadPort()}");

            Console.WriteLine("Stop");
        }
    }
}

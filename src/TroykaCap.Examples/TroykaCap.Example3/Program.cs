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

            Expander = Pi.I2C.GetGpioExpander();
        }

        public static void Main()
        {
            Console.WriteLine("Start.");

            Console.WriteLine("Port: {0}", Expander.DigitalReadPort());

            Expander.DigitalWritePort(255);

            Task.Delay(60000).Wait();

            Expander.DigitalWritePort(0);

            Console.WriteLine("Port: {0}", Expander.DigitalReadPort());

            Console.WriteLine("Stop.");
        }
    }
}

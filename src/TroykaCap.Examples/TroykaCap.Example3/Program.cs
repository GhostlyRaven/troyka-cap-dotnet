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

            Expander.Error += Expander_Error;
        }

        public static void Main()
        {
            Console.WriteLine("Start.");

            Console.WriteLine("Port: {0}", Expander.DigitalReadPort());

            Expander.DigitalPortHighLevel();

            Task.Delay(60000).Wait();

            Expander.DigitalPortLowLevel();

            Console.WriteLine("Port: {0}", Expander.DigitalReadPort());

            Console.WriteLine("Stop.");
        }

        private static void Expander_Error(object sender, ErrorEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine(e.Error);
            }
            else
            {
                Console.WriteLine("Error is null.");
            }
        }
    }
}

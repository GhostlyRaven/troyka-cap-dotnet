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

            Expander = Pi.I2C.GetGpioExpander();

            Expander.Error += Expander_Error;
        }

        public static void Main()
        {
            Console.WriteLine("Start.");

            Expander.PwmFreq(Freq);

            Console.WriteLine("Freq: {0}", Freq);

            Expander.AnalogWrite(Pin, DutyCycle);

            Console.WriteLine("Duty cycle: {0}", DutyCycle);

            Task.Delay(15000).Wait();

            double dutyCycle = 2 * DutyCycle;

            Expander.AnalogWrite(Pin, dutyCycle);

            Console.WriteLine("Duty cycle: {0}", dutyCycle);

            Task.Delay(30000).Wait();

            Expander.AnalogWrite(Pin, DutyCycle);

            Console.WriteLine("Duty cycle: {0}", DutyCycle);

            Task.Delay(15000).Wait();

            Expander.AnalogWrite(Pin, 0);

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

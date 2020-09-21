using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example4
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

            Console.WriteLine("Enter 1 or 2 to select a mode:");
            Console.WriteLine("1. Change address.");
            Console.WriteLine("2. Reset address.");

            string mode = Console.ReadLine();

            switch (mode)
            {
                case "1":
                    {
                        Console.WriteLine("Enter a new address:");

                        if (ushort.TryParse(Console.ReadLine(), out ushort address))
                        {
                            Console.WriteLine($"Change address: {Expander.ChangeAddress(address)}");
                            Console.WriteLine($"Save address: {Expander.SaveAddress()}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid address value.");
                        }

                        break;
                    }
                case "2":
                    {
                        Console.WriteLine($"Reset address: {Expander.Reset()}");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid mode value.");
                        break;
                    }
            }

            Console.WriteLine("Stop");
        }
    }
}

using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example4
{
    public static class Program
    {
        private static IGpioExpander Expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();
        }

        public static void Main()
        {
            Console.WriteLine("Start");

            Console.WriteLine("Enter 1 or 2 to select a mode:");
            Console.WriteLine("1. Change address.");
            Console.WriteLine("2. Reset address.");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Expander = Pi.I2C.GetGpioExpander();

                        Console.WriteLine("Enter a new address:");

                        if (ushort.TryParse(Console.ReadLine(), out ushort address))
                        {
                            Expander.ChangeAddress(address);

                            Console.WriteLine("Change address.");

                            Expander.SaveAddress();

                            Console.WriteLine("Save address.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid address value.");
                        }

                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("Enter a current address:");

                        if (ushort.TryParse(Console.ReadLine(), out ushort address))
                        {
                            Expander = Pi.I2C.GetGpioExpander(address);

                            Expander.Reset();

                            Console.WriteLine("Reset address.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid address value.");
                        }

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

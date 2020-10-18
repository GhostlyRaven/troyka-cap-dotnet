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

            Expander = Pi.I2C.GetGpioExpander();

            Expander.Error += Expander_Error;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Start.");

            Console.WriteLine("Enter 1 or 2 to select a mode:");
            Console.WriteLine("1. Change address.");
            Console.WriteLine("2. Reset address.");

            switch (ReadProgramMode(args, 0))
            {
                case "1":
                    {
                        Console.WriteLine("Enter a new address:");

                        if (ushort.TryParse(ReadProgramMode(args, 1), out ushort address))
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

                        if (ushort.TryParse(ReadProgramMode(args, 1), out ushort address))
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

            Console.WriteLine("Stop.");
        }

        private static string ReadProgramMode(string[] args, int index)
        {
            if (args.Length == 0)
            {
                return Console.ReadLine();
            }

            if (args.Length > index)
            {
                return args[index];
            }

            return string.Empty;
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

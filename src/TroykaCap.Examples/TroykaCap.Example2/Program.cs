using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using TroykaCap.Expander.Extensions;
using Unosquare.RaspberryIO.Abstractions;
using RemoteDebugger = System.Diagnostics.Debugger;

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

            Expander = Pi.I2C.GetGpioExpander();

            Expander.Error += Expander_Error;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Start.");

            Console.WriteLine("Enter 1 or 2 to select a mode:");
            Console.WriteLine("1. GPIO expander.");
            Console.WriteLine("2. GPIO.");

            switch (ReadProgramMode(args, 0))
            {
                case "1":
                    {
                        Expander.PinMode(Pin1, PinMode.Input);
                        Expander.PinMode(Pin2, PinMode.Output);

                        while (Exit())
                        {
                            bool result = Expander.TroykaButtonClick(Pin1);

                            Console.WriteLine("Result: {0}", result);

                            Expander.DigitalWrite(Pin2, result);
                        }

                        break;
                    }
                case "2":
                    {
                        if (Pi.Gpio is GpioController gpio)
                        {
                            IGpioPin pinInput = gpio[WiringPiPin.Pin07];
                            IGpioPin pinOutput = gpio[WiringPiPin.Pin22];

                            pinInput.PinMode = GpioPinDriveMode.Input;
                            pinOutput.PinMode = GpioPinDriveMode.Output;

                            while (Exit())
                            {
                                bool result = pinInput.TroykaButtonClick();

                                Console.WriteLine("Result: {0}", result);

                                pinOutput.Write(result);
                            }
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

        private static bool Exit()
        {
            return RemoteDebugger.IsAttached || !(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape);
        }

        private static void Expander_Error(object sender, ErrorEventArgs e)
        {
            if (e.HasValue)
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

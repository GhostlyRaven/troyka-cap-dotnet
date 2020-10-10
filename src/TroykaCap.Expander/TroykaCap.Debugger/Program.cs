using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using System.Threading.Tasks;
using TroykaCap.Expander.Extensions;
using RemoteDebugger = System.Diagnostics.Debugger;

namespace TroykaCap.Debugger
{
    public static class Program
    {
        private static ushort Freq = 250;
        private static double DutyCycle = 0.5;

        private static IGpioExpander _expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            _expander = Pi.I2C.GetGpioExpander();

            _expander.Error += Expander_Error;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Start.");

            switch (ReadProgramMode(args, 0))
            {
                case nameof(Analog):
                    {
                        Analog();
                        break;
                    }
                case nameof(Digital):
                    {
                        Digital();
                        break;
                    }
                case nameof(Port):
                    {
                        Port();
                        break;
                    }
                case nameof(Address):
                    {
                        Address();
                        break;
                    }
                case nameof(Pwm):
                    {
                        Pwm();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid program mode.");
                        break;
                    }
            }

            Console.WriteLine("Stop.");
        }

        private static void Analog()
        {
            while (Exit())
            {
                double result = _expander.AnalogRead(0);

                Console.WriteLine("Result: {0}", result);

                _expander.AnalogWrite(1, result);
            }
        }

        private static void Digital()
        {
            _expander.PinMode(0, PinMode.Input);
            _expander.PinMode(1, PinMode.Output);

            while (Exit())
            {
                bool result = _expander.TroykaButtonClick(0);

                Console.WriteLine("Result: {0}", result);

                _expander.DigitalWrite(1, result);
            }
        }

        private static void Port()
        {
            Console.WriteLine("Port: {0}", _expander.DigitalReadPort());

            _expander.DigitalPortHighLevel();

            Task.Delay(60000).Wait();

            _expander.DigitalPortLowLevel();

            Console.WriteLine("Port: {0}", _expander.DigitalReadPort());
        }

        private static void Address()
        {
            Console.WriteLine("Enter 1 or 2 to select a mode:");
            Console.WriteLine("1. Change address.");
            Console.WriteLine("2. Reset address.");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Console.WriteLine("Enter a new address:");

                        if (ushort.TryParse(Console.ReadLine(), out ushort address))
                        {
                            _expander.ChangeAddress(address);

                            Console.WriteLine("Change address.");

                            _expander.SaveAddress();

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
                            _expander = Pi.I2C.GetGpioExpander(address);

                            _expander.Reset();

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
        }

        private static void Pwm()
        {
            _expander.PwmFreq(Freq);

            Console.WriteLine("Freq: {0}", Freq);

            _expander.AnalogWrite(0, DutyCycle);

            Console.WriteLine("Duty cycle: {0}", DutyCycle);

            Task.Delay(15000).Wait();

            double dutyCycle = 2 * DutyCycle;

            _expander.AnalogWrite(0, dutyCycle);

            Console.WriteLine("Duty cycle: {0}", dutyCycle);

            Task.Delay(30000).Wait();

            _expander.AnalogWrite(0, DutyCycle);

            Console.WriteLine("Duty cycle: {0}", DutyCycle);

            Task.Delay(15000).Wait();

            _expander.AnalogWrite(0, 0);
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
            Console.WriteLine(e.Error);
        }
    }
}

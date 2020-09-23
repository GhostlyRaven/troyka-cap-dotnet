using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using Microsoft.Extensions.Logging;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example4
{
    public static class Program
    {
        private static IGpioExpander Expander;
        private static readonly ILogger Logger;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            Logger = LoggerFactory.Create(log => log.AddConsole()).CreateLogger(nameof(Program));
        }

        public static void Main()
        {
            Logger.LogInformation("Start");

            Logger.LogInformation("Enter 1 or 2 to select a mode:");
            Logger.LogInformation("1. Change address.");
            Logger.LogInformation("2. Reset address.");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Expander = Pi.I2C.GetGpioExpander(logger: Logger);

                        Logger.LogInformation("Enter a new address:");

                        if (ushort.TryParse(Console.ReadLine(), out ushort address))
                        {
                            Expander.ChangeAddress(address);
                            Expander.SaveAddress();

                            Logger.LogInformation($"Change address.");
                            Logger.LogInformation($"Save address.");
                        }
                        else
                        {
                            Logger.LogInformation("Invalid address value.");
                        }

                        break;
                    }
                case "2":
                    {
                        Logger.LogInformation("Enter a current address:");

                        if (ushort.TryParse(Console.ReadLine(), out ushort address))
                        {
                            Expander = Pi.I2C.GetGpioExpander(address, Logger);

                            Expander.Reset();

                            Logger.LogInformation($"Reset address.");
                        }
                        else
                        {
                            Logger.LogInformation("Invalid address value.");
                        }

                        break;
                    }
                default:
                    {
                        Logger.LogInformation("Invalid mode value.");
                        break;
                    }
            }

            Logger.LogInformation("Stop");
        }
    }
}

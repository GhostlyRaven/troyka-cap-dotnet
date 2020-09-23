using System;
using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using Microsoft.Extensions.Logging;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example2
{
    public static class Program
    {
        private static ushort Pin1 = 0;
        private static ushort Pin2 = 1;

        private static readonly ILogger Logger;
        private static readonly IGpioExpander Expander;

        static Program()
        {
            Pi.Init<BootstrapWiringPi>();

            Expander = Pi.I2C.GetGpioExpander(logger: Logger);

            Logger = LoggerFactory.Create(log => log.AddConsole()).CreateLogger(nameof(Program));
        }

        public static void Main()
        {
            Logger.LogInformation("Start");

            Expander.PinMode(Pin1, PinMode.Input);
            Expander.PinMode(Pin2, PinMode.Output);

            while (Exit())
            {
                bool result = !Expander.DigitalRead(Pin1);

                Logger.LogInformation("Result: {0}", result);

                Expander.DigitalWrite(Pin2, result);
            }

            Logger.LogInformation("Stop");
        }

        private static bool Exit()
        {
            return !(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape);
        }
    }
}

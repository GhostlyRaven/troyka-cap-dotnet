using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example3
{
    public static class Program
    {
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

            Logger.LogInformation($"Port: {Expander.DigitalReadPort()}");

            Expander.DigitalWritePort(255);

            Task.Delay(60000).Wait();

            Expander.DigitalWritePort(0);

            Logger.LogInformation($"Port: {Expander.DigitalReadPort()}");

            Logger.LogInformation("Stop");
        }
    }
}

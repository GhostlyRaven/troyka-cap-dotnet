using TroykaCap.Expander;
using Unosquare.WiringPi;
using Unosquare.RaspberryIO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TroykaCap.Expander.Extensions;

namespace TroykaCap.Example5
{
    public static class Program
    {
        private static ushort Pin = 0;
        private static ushort Freq = 250; //Гц
        private static double DutyCycle = 0.5;

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

            Expander.PwmFreq(Freq);

            Logger.LogInformation("Freq: {0}", Freq);

            Expander.AnalogWrite(Pin, DutyCycle);

            Logger.LogInformation("Duty cycle: {0}", DutyCycle);

            Task.Delay(15000).Wait();

            double dutyCycle = 2 * DutyCycle;

            Expander.AnalogWrite(Pin, dutyCycle);

            Logger.LogInformation("Duty cycle: {0}", dutyCycle);

            Task.Delay(30000).Wait();

            Expander.AnalogWrite(Pin, DutyCycle);

            Logger.LogInformation("Duty cycle: {0}", DutyCycle);

            Task.Delay(15000).Wait();

            Expander.AnalogWrite(Pin, 0);

            Logger.LogInformation("Stop");
        }
    }
}

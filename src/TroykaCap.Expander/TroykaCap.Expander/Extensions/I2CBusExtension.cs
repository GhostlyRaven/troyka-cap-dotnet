using System;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander.Extensions
{
    public static class I2CBusExtension
    {
        private const int GpioExpanderDefaultI2CAddress = 0x2A;

        public static IGpioExpander CreateGpioExpander(this II2CBus bus, int expanderAddress = GpioExpanderDefaultI2CAddress)
        {
            if (bus is null)
            {
                throw new ArgumentNullException(nameof(bus), "The I2C bus can't be null.");
            }

            if (expanderAddress < 0 || expanderAddress > 127)
            {
                throw new ArgumentOutOfRangeException(nameof(expanderAddress), expanderAddress, "The I2C bus address must be between 0 and 127.");
            }

            return new InternalGpioExpander(bus.AddDevice(expanderAddress));
        }
    }
}
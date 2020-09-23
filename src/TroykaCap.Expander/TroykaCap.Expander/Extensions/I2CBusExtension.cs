using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander.Extensions
{
    /// <summary>
    /// Extension class for I2C bus.
    /// </summary>
    public static class I2CBusExtension
    {
        /// <summary>
        /// Creates an object of type IGpioExpander.
        /// </summary>
        /// <param name="bus">Object of type II2CBus.</param>
        /// <param name="expanderAddress">Gpio expander address.</param>
        /// <param name="logger">Object of type ILogger.</param>
        /// <returns>Returns an object of type IGpioExpander.</returns>
        /// <exception cref="ArgumentNullException">Object of type II2CBus is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Invalid address value.</exception>
        /// <exception cref="KeyNotFoundException">Device not found.</exception>
        public static IGpioExpander GetGpioExpander(this II2CBus bus, int expanderAddress = 0x2A, ILogger logger = default)
        {
            if (bus is null)
            {
                throw new ArgumentNullException(nameof(bus), "The I2C bus can't be null.");
            }

            if (expanderAddress < 0 || expanderAddress > 127)
            {
                throw new ArgumentOutOfRangeException(nameof(expanderAddress), expanderAddress, "The I2C bus address must be between 0 and 127.");
            }

            return new InternalGpioExpander(bus.AddDevice(expanderAddress), logger);
        }
    }
}

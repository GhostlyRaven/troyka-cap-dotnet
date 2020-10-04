using System;
using System.Collections.Generic;
using TroykaCap.Expander.Internal;
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
        /// <param name="address">Gpio expander address.</param>
        /// <returns>Returns an object of type IGpioExpander.</returns>
        /// <exception cref="ArgumentNullException">Object of type II2CBus is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Invalid address value.</exception>
        /// <exception cref="KeyNotFoundException">Device not found.</exception>
        public static IGpioExpander GetGpioExpander(this II2CBus bus, int address = 0x2A)
        {
            if (bus is null)
            {
                throw new ArgumentNullException(nameof(bus), ErrorMessages.Bus);
            }

            if (address < 0 || address > 127)
            {
                throw new ArgumentOutOfRangeException(nameof(address), address, ErrorMessages.Address);
            }

            return new GpioExpander(bus.AddDevice(address));
        }
    }
}

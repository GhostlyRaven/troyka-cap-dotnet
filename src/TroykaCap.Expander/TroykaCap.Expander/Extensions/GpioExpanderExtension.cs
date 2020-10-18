using System;
using TroykaCap.Expander.Internal;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander.Extensions
{
    /// <summary>
    /// Provides additional functions for working with the GPIO expander.
    /// </summary>
    public static class GpioExpanderExtension
    {
        /// <summary>
        /// Fixes click of the TroykaButton.
        /// </summary>
        /// <param name="pin">Object of type IGpioPin.</param>
        /// <returns>True - click, False - not click.</returns>
        /// <exception cref="ArgumentNullException">Object of type IGpioPin is null.</exception>
        public static bool TroykaButtonClick(this IGpioPin pin)
        {
            if (pin is null)
            {
                throw ThrowHelper.ArgumentNullException(nameof(pin));
            }

            return !pin.Read();
        }

        /// <summary>
        /// Fixes click of the TroykaButton.
        /// </summary>
        /// <param name="expander">Object of type IGpioExpander.</param>
        /// <param name="pin">Pin number.</param>
        /// <returns>True - click, False - not click.</returns>
        /// <exception cref="ArgumentNullException">Object of type IGpioExpander is null.</exception>
        public static bool TroykaButtonClick(this IGpioExpander expander, ushort pin)
        {
            if (expander is null)
            {
                throw ThrowHelper.ArgumentNullException(nameof(expander));
            }

            return !expander.DigitalRead(pin);
        }

        /// <summary>
        /// Sets all pins to high level.
        /// </summary>
        /// <param name="expander">Object of type IGpioExpander.</param>
        /// <exception cref="ArgumentNullException">Object of type IGpioExpander is null.</exception>
        public static void DigitalPortHighLevel(this IGpioExpander expander)
        {
            if (expander is null)
            {
                throw ThrowHelper.ArgumentNullException(nameof(expander));
            }

            expander.DigitalWritePort(255);
        }

        /// <summary>
        /// Sets all pins to low level.
        /// </summary>
        /// <param name="expander">Object of type IGpioExpander.</param>
        /// <exception cref="ArgumentNullException">Object of type IGpioExpander is null.</exception>
        public static void DigitalPortLowLevel(this IGpioExpander expander)
        {
            if (expander is null)
            {
                throw ThrowHelper.ArgumentNullException(nameof(expander));
            }

            expander.DigitalWritePort(0);
        }
    }
}

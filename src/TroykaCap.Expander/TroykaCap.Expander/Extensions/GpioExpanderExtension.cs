namespace TroykaCap.Expander.Extensions
{
    /// <summary>
    /// Provides additional functions for working with the Gpio expander.
    /// </summary>
    public static class GpioExpanderExtension
    {
        /// <summary>
        /// Fixes click of the TroykaButton.
        /// </summary>
        /// <param name="expander">Object of type IGpioExpander.</param>
        /// <param name="pin">Pin number.</param>
        /// <returns>True - click, False - not click.</returns>
        public static bool TroykaButtonClick(this IGpioExpander expander, ushort pin)
        {
            if (expander is null)
            {
                return false;
            }

            return !expander.DigitalRead(pin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expander">Object of type IGpioExpander.</param>
        public static void DigitalPortHigh(this IGpioExpander expander)
        {
            expander?.DigitalWritePort(255);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expander">Object of type IGpioExpander.</param>
        public static void DigitalPortLow(this IGpioExpander expander)
        {
            expander?.DigitalWritePort(0);
        }
    }
}

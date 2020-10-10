using System;

namespace TroykaCap.Expander
{
    /// <summary>
    /// Object of type IGpioExpander.
    /// </summary>
    public interface IGpioExpander
    {
        #region Event functions

        /// <summary>
        /// Occurs when an error occurs while working with the GPIO expander.
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;

        #endregion

        #region Digital functions

        /// <summary>
        /// Reads from the port.
        /// </summary>
        /// <returns>The sequence of bits on the port.</returns>
        ushort DigitalReadPort();

        /// <summary>
        /// Writes to the port.
        /// </summary>
        /// <param name="value">The sequence of bits on the port.</param>
        void DigitalWritePort(ushort value);

        /// <summary>
        /// Sets the pin mode.
        /// </summary>
        /// <param name="pin">Pin number.</param>
        /// <param name="mode">Pin mode.</param>
        void PinMode(ushort pin, PinMode mode);

        /// <summary>
        /// Writes a digital signal.
        /// </summary>
        /// <param name="pin">Pin number.</param>
        /// <param name="value">High or low digital signal.</param>
        void DigitalWrite(ushort pin, bool value);

        /// <summary>
        /// Reads a digital signal.
        /// </summary>
        /// <param name="pin">Pin number.</param>
        /// <returns>High or low digital signal.</returns>
        bool DigitalRead(ushort pin);

        #endregion

        #region Analog functions

        /// <summary>
        /// Writes an analog signal.
        /// </summary>
        /// <param name="pin">Pin number.</param>
        /// <param name="value">Analog signal value.</param>
        void AnalogWrite(ushort pin, double value);

        /// <summary>
        /// Reads an analog signal.
        /// </summary>
        /// <param name="pin">Pin number.</param>
        /// <returns>Analog signal value.</returns>
        double AnalogRead(ushort pin);

        /// <summary>
        /// Sets the PWM frequency.
        /// </summary>
        /// <param name="freq">PWM frequency in Hz.</param>
        void PwmFreq(ushort freq);

        #endregion

        #region Shield settings

        /// <summary>
        /// Sets a new address to the GPIO expander.
        /// </summary>
        /// <param name="newAddress">New GPIO expander address.</param>
        void ChangeAddress(ushort newAddress);

        /// <summary>
        /// Saves the new address to the GPIO expander.
        /// </summary>
        void SaveAddress();

        /// <summary>
        /// Returns the base address to the GPIO expander.
        /// </summary>
        void Reset();

        #endregion
    }
}

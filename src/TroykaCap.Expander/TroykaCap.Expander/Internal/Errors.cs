namespace TroykaCap.Expander.Internal
{
    internal static class Errors
    {
        internal const string I2CBus = "The I2C bus can't be null.";
        internal const string GpioPin = "The GPIO pin object can't be null.";
        internal const string Expander = "The expander object can't be null.";
        internal const string PinMode = "The input mode has an invalid value.";
        internal const string I2CAddress = "The I2C bus address must be between 0 and 127.";
    }
}

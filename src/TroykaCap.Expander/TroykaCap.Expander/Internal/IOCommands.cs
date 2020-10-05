namespace TroykaCap.Expander.Internal
{
    internal static class IOCommands
    {
        #region STM32

        internal const int Reset = 0x01;
        internal const int ChangeI2CAddress = 0x02;
        internal const int SaveI2CAddress = 0x03;
        internal const int PortModeInput = 0x04;
        internal const int PortModePullUp = 0x05;
        internal const int PortModePullDown = 0x06;
        internal const int PortModeOutput = 0x07;
        internal const int DigitalRead = 0x08;
        internal const int DigitalWriteHigh = 0x09;
        internal const int DigitalWriteLow = 0x0A;
        internal const int AnalogWrite = 0x0B;
        internal const int AnalogRead = 0x0C;
        internal const int PwmFreq = 0x0D;

        #endregion
    }
}

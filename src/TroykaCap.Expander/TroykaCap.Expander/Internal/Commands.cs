namespace TroykaCap.Expander.Internal
{
    internal static class Commands
    {
        internal const int Invalid = 0x0;
        internal const int ResetI2CAddress = 0x1;
        internal const int ChangeI2CAddress = 0x2;
        internal const int SaveI2CAddress = 0x3;
        internal const int PortModeInput = 0x4;
        internal const int PortModePullUp = 0x5;
        internal const int PortModePullDown = 0x6;
        internal const int PortModeOutput = 0x7;
        internal const int DigitalRead = 0x8;
        internal const int DigitalWriteHigh = 0x9;
        internal const int DigitalWriteLow = 0xA;
        internal const int AnalogWrite = 0xB;
        internal const int AnalogRead = 0xC;
        internal const int PwmFreq = 0xD;
    }
}

namespace TroykaCap.Expander
{
    public interface IGpioExpander
    {
        #region Digital functions

        ushort DigitalReadPort();

        bool DigitalWritePort(ushort value);

        bool PinMode(ushort pin, PinMode mode);

        bool DigitalWrite(ushort pin, bool value);

        bool DigitalRead(ushort pin);

        #endregion

        #region Analog functions

        bool AnalogWrite(ushort pin, double value);

        double AnalogRead(ushort pin);

        bool PwmFreq(ushort freq);

        #endregion

        #region Shield settings

        bool ChangeAddress(ushort newAddress);

        bool SaveAddress();

        bool Reset();

        #endregion
    }
}
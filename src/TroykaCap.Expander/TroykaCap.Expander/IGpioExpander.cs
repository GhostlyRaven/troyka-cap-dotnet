namespace TroykaCap.Expander
{
    public interface IGpioExpander
    {
        #region Digital functions

        ushort DigitalReadPort();

        void DigitalWritePort(ushort value);

        void PinMode(ushort pin, PinMode mode);

        void DigitalWrite(ushort pin, bool value);

        bool DigitalRead(ushort pin);

        #endregion

        #region Analog functions

        void AnalogWrite(ushort pin, double value);

        double AnalogRead(ushort pin);

        void PwmFreq(ushort freq);

        #endregion

        #region Shield settings

        bool ChangeAddress(ushort newAddress);

        bool SaveAddress();

        bool Reset();

        #endregion
    }
}
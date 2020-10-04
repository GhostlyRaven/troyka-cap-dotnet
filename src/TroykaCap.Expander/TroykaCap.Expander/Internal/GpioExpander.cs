using System;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander.Internal
{
    internal sealed class GpioExpander : IGpioExpander
    {
        #region STM32 addresses

        private const int GpioExpanderReset = 0x01;
        private const int GpioExpanderChangeI2CAddress = 0x02;
        private const int GpioExpanderSaveI2CAddress = 0x03;
        private const int GpioExpanderPortModeInput = 0x04;
        private const int GpioExpanderPortModePullUp = 0x05;
        private const int GpioExpanderPortModePullDown = 0x06;
        private const int GpioExpanderPortModeOutput = 0x07;
        private const int GpioExpanderDigitalRead = 0x08;
        private const int GpioExpanderDigitalWriteHigh = 0x09;
        private const int GpioExpanderDigitalWriteLow = 0x0A;
        private const int GpioExpanderAnalogWrite = 0x0B;
        private const int GpioExpanderAnalogRead = 0x0C;
        private const int GpioExpanderPwmFreq = 0x0D;

        private const int AnalogWriteMultiplier = 255;
        private const double AnalogReadDivisor = 4095.0;

        #endregion

        private readonly II2CDevice _gpioExpander;

        internal GpioExpander(II2CDevice gpioExpander)
        {
            _gpioExpander = gpioExpander;
        }

        #region Digital functions

        public ushort DigitalReadPort()
        {
            ushort data = default;

            try
            {
                data = _gpioExpander.ReadAddressWord(GpioExpanderDigitalRead);
            }
            catch (Exception error)
            {
                OnError(error);

                return data;
            }

            return ReverseUInt16(data);
        }

        public bool DigitalRead(ushort pin)
        {
            return (DigitalReadPort() & GetMask(pin)) > 0;
        }

        public void DigitalWritePort(ushort value)
        {
            ushort data = ReverseUInt16(value);

            try
            {
                _gpioExpander.WriteAddressWord(GpioExpanderDigitalWriteHigh, data);
                _gpioExpander.WriteAddressWord(GpioExpanderDigitalWriteLow, BitwiseDataComplement(data));
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        public void PinMode(ushort pin, PinMode mode)
        {
            ushort data = GetMask(pin);

            data = ReverseUInt16(data);

            try
            {
                _gpioExpander.WriteAddressWord(GetPortModeAddress(mode), data);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        public void DigitalWrite(ushort pin, bool value)
        {
            ushort data = GetMask(pin);

            data = ReverseUInt16(data);

            try
            {
                _gpioExpander.WriteAddressWord(GetHighOrLowAddress(value), data);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        #endregion

        #region Analog functions

        public void AnalogWrite(ushort pin, double value)
        {
            ushort data = (ushort)(value * AnalogWriteMultiplier);

            data = (ushort)((pin & 0xff) | ((data & 0xff) << 8));

            try
            {
                _gpioExpander.WriteAddressWord(GpioExpanderAnalogWrite, data);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        public double AnalogRead(ushort pin)
        {
            return AnalogRead16(pin) / AnalogReadDivisor;
        }

        public void PwmFreq(ushort freq)
        {
            ushort reverseFreq = ReverseUInt16(freq);

            try
            {
                _gpioExpander.WriteAddressWord(GpioExpanderPwmFreq, reverseFreq);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        #endregion

        #region Shield settings

        public void ChangeAddress(ushort newAddress)
        {
            try
            {
                _gpioExpander.WriteAddressWord(GpioExpanderChangeI2CAddress, newAddress);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        public void SaveAddress()
        {
            try
            {
                _gpioExpander.Write(GpioExpanderSaveI2CAddress);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        public void Reset()
        {
            try
            {
                _gpioExpander.Write(GpioExpanderReset);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        #endregion

        #region Unsafe private functions

        private int GetPortModeAddress(PinMode mode)
        {
            return mode switch
            {
                Expander.PinMode.Input => GpioExpanderPortModeInput,
                Expander.PinMode.Output => GpioExpanderPortModeOutput,
                Expander.PinMode.InputPullUp => GpioExpanderPortModePullUp,
                Expander.PinMode.InputPullDown => GpioExpanderPortModePullDown,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, ErrorMessages.Mode)
            };
        }

        private ushort AnalogRead16(ushort pin)
        {
            ushort data = default;

            try
            {
                _gpioExpander.WriteAddressWord(GpioExpanderAnalogRead, pin);

                data = _gpioExpander.ReadAddressWord(GpioExpanderAnalogRead);
            }
            catch (Exception error)
            {
                OnError(error);

                return data;
            }

            return ReverseUInt16(data);
        }

        #endregion

        #region Safe private functions

        private ushort BitwiseDataComplement(ushort data)
        {
            return (ushort)~data;
        }

        private int GetHighOrLowAddress(bool value)
        {
            return value ? GpioExpanderDigitalWriteHigh : GpioExpanderDigitalWriteLow;
        }

        private ushort ReverseUInt16(ushort data)
        {
            return (ushort)(((data & 0xff) << 8) | ((data >> 8) & 0xff));
        }

        private ushort GetMask(ushort pin)
        {
            return (ushort)(0x0001 << pin);
        }

        #endregion

        #region Event functions

        public event EventHandler<ErrorEventArgs> Error;

        private void OnError(ErrorEventArgs args)
        {
            Error?.Invoke(this, args);
        }

        #endregion
    }
}

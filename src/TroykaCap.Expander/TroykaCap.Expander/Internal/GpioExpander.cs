using System;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander.Internal
{
    internal sealed class GpioExpander : IGpioExpander
    {
        private readonly II2CDevice _gpioExpander;

        private const int AnalogWriteMultiplier = 255;
        private const double AnalogReadDivisor = 4095.0;

        public event EventHandler<ErrorEventArgs> Error;

        internal GpioExpander(II2CDevice gpioExpander)
        {
            _gpioExpander = gpioExpander;
        }

        #region Digital functions

        public ushort DigitalReadPort()
        {
            ushort data;

            try
            {
                data = _gpioExpander.ReadAddressWord(Commands.DigitalRead);
            }
            catch (Exception error)
            {
                OnError(error);
                return default;
            }

            return ReverseUInt16(data);
        }

        public bool DigitalRead(ushort pin)
        {
            return (DigitalReadPort() & GetMask(pin)) > 0;
        }

        public void DigitalWritePort(ushort value)
        {
            ushort highData = ReverseUInt16(value);
            ushort lowData = (ushort)~highData;

            try
            {
                _gpioExpander.WriteAddressWord(Commands.DigitalWriteHigh, highData);
                _gpioExpander.WriteAddressWord(Commands.DigitalWriteLow, lowData);
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

            int command = mode switch
            {
                Expander.PinMode.Input => Commands.PortModeInput,
                Expander.PinMode.Output => Commands.PortModeOutput,
                Expander.PinMode.InputPullUp => Commands.PortModePullUp,
                Expander.PinMode.InputPullDown => Commands.PortModePullDown,
                _ => Commands.Invalid
            };

            if (command == Commands.Invalid)
            {
                OnError(new ArgumentOutOfRangeException(nameof(mode), mode, Errors.PinMode));
                return;
            }

            try
            {
                _gpioExpander.WriteAddressWord(command, data);
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

            int command = value ? Commands.DigitalWriteHigh : Commands.DigitalWriteLow;

            try
            {
                _gpioExpander.WriteAddressWord(command, data);
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
                _gpioExpander.WriteAddressWord(Commands.AnalogWrite, data);
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

        public ushort AnalogRead16(ushort pin)
        {
            ushort data;

            try
            {
                _gpioExpander.WriteAddressWord(Commands.AnalogRead, pin);
                data = _gpioExpander.ReadAddressWord(Commands.AnalogRead);
            }
            catch (Exception error)
            {
                OnError(error);
                return default;
            }

            return ReverseUInt16(data);
        }

        public void PwmFreq(ushort freq)
        {
            ushort reverseFreq = ReverseUInt16(freq);

            try
            {
                _gpioExpander.WriteAddressWord(Commands.PwmFreq, reverseFreq);
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
                _gpioExpander.WriteAddressWord(Commands.ChangeI2CAddress, newAddress);
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
                _gpioExpander.Write(Commands.SaveI2CAddress);
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
                _gpioExpander.Write(Commands.ResetI2CAddress);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        #endregion

        #region Private functions

        private void OnError(ErrorEventArgs args)
        {
            Error?.Invoke(this, args);
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
    }
}

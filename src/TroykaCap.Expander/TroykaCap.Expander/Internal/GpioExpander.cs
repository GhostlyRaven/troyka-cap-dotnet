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
            ushort data = default;

            try
            {
                data = _gpioExpander.ReadAddressWord(IOCommands.DigitalRead);
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
            ushort highData = ReverseUInt16(value);
            ushort lowData = (ushort)~highData;

            try
            {
                _gpioExpander.WriteAddressWord(IOCommands.DigitalWriteHigh, highData);
                _gpioExpander.WriteAddressWord(IOCommands.DigitalWriteLow, lowData);
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

            switch (GetPortModeOrError(mode))
            {
                case int command:
                    {
                        try
                        {
                            _gpioExpander.WriteAddressWord(command, data);
                        }
                        catch (Exception error)
                        {
                            OnError(error);
                        }

                        break;
                    }
                case Exception error:
                    {
                        OnError(error);
                        break;
                    }
            }
        }

        public void DigitalWrite(ushort pin, bool value)
        {
            ushort data = GetMask(pin);
            data = ReverseUInt16(data);
            int command = value ? IOCommands.DigitalWriteHigh : IOCommands.DigitalWriteLow;

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
                _gpioExpander.WriteAddressWord(IOCommands.AnalogWrite, data);
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
                _gpioExpander.WriteAddressWord(IOCommands.PwmFreq, reverseFreq);
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
                _gpioExpander.WriteAddressWord(IOCommands.ChangeI2CAddress, newAddress);
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
                _gpioExpander.Write(IOCommands.SaveI2CAddress);
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
                _gpioExpander.Write(IOCommands.Reset);
            }
            catch (Exception error)
            {
                OnError(error);
            }
        }

        #endregion

        #region Unsafe private functions

        private ushort AnalogRead16(ushort pin)
        {
            ushort data = default;

            try
            {
                _gpioExpander.WriteAddressWord(IOCommands.AnalogRead, pin);
                data = _gpioExpander.ReadAddressWord(IOCommands.AnalogRead);
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

        private object GetPortModeOrError(PinMode mode)
        {
            return mode switch
            {
                Expander.PinMode.Input => IOCommands.PortModeInput,
                Expander.PinMode.Output => IOCommands.PortModeOutput,
                Expander.PinMode.InputPullUp => IOCommands.PortModePullUp,
                Expander.PinMode.InputPullDown => IOCommands.PortModePullDown,
                _ => new ArgumentOutOfRangeException(nameof(mode), mode, Errors.PinMode)
            };
        }

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

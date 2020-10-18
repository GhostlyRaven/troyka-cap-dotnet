using System;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander.Internal
{
    internal sealed class GpioExpander : IGpioExpander
    {
        #region STM32 commands

        private const int ResetI2CAddressCommand = 0x1;
        private const int ChangeI2CAddressCommand = 0x2;
        private const int SaveI2CAddressCommand = 0x3;
        private const int PortModeInputCommand = 0x4;
        private const int PortModePullUpCommand = 0x5;
        private const int PortModePullDownCommand = 0x6;
        private const int PortModeOutputCommand = 0x7;
        private const int DigitalReadCommand = 0x8;
        private const int DigitalWriteHighCommand = 0x9;
        private const int DigitalWriteLowCommand = 0xA;
        private const int AnalogWriteCommand = 0xB;
        private const int AnalogReadCommand = 0xC;
        private const int PwmFreqCommand = 0xD;

        #endregion

        #region Consts

        private const int InvalidCommand = -1;
        private const int AnalogWriteMultiplier = 255;
        private const double AnalogReadDivisor = 4095.0;

        #endregion

        private readonly II2CDevice _gpioExpander;

        internal GpioExpander(II2CDevice gpioExpander)
        {
            _gpioExpander = gpioExpander;
        }

        public event EventHandler<ErrorEventArgs> Error;

        #region Digital functions

        public ushort DigitalReadPort()
        {
            ushort data;

            try
            {
                data = _gpioExpander.ReadAddressWord(DigitalReadCommand);
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
                _gpioExpander.WriteAddressWord(DigitalWriteHighCommand, highData);
                _gpioExpander.WriteAddressWord(DigitalWriteLowCommand, lowData);
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
                Expander.PinMode.Input => PortModeInputCommand,
                Expander.PinMode.Output => PortModeOutputCommand,
                Expander.PinMode.InputPullUp => PortModePullUpCommand,
                Expander.PinMode.InputPullDown => PortModePullDownCommand,
                _ => InvalidCommand
            };

            if (command == InvalidCommand)
            {
                OnError(ThrowHelper.ArgumentOutOfRangeException(nameof(mode), mode));
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

            int command = value ? DigitalWriteHighCommand : DigitalWriteLowCommand;

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
                _gpioExpander.WriteAddressWord(AnalogWriteCommand, data);
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
                _gpioExpander.WriteAddressWord(AnalogReadCommand, pin);
                data = _gpioExpander.ReadAddressWord(AnalogReadCommand);
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
                _gpioExpander.WriteAddressWord(PwmFreqCommand, reverseFreq);
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
                _gpioExpander.WriteAddressWord(ChangeI2CAddressCommand, newAddress);
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
                _gpioExpander.Write(SaveI2CAddressCommand);
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
                _gpioExpander.Write(ResetI2CAddressCommand);
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

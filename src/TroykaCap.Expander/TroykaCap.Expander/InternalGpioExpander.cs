using Microsoft.Extensions.Logging;
using TroykaCap.Expander.Extensions;
using Unosquare.RaspberryIO.Abstractions;

namespace TroykaCap.Expander
{
    internal sealed class InternalGpioExpander : IGpioExpander
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

        private readonly ILogger _logger;
        private readonly II2CDevice _gpioExpander;

        internal InternalGpioExpander(II2CDevice gpioExpander, ILogger logger)
        {
            _gpioExpander = gpioExpander;
            _logger = logger;
        }

        #region Digital functions

        public ushort DigitalReadPort()
        {
            return ReverseUInt16(_gpioExpander.SafeReadAddressWord(GpioExpanderDigitalRead, _logger));
        }

        public void DigitalWritePort(ushort value)
        {
            ushort data = ReverseUInt16(value);

            _gpioExpander.SafeWriteAddressWord(GpioExpanderDigitalWriteHigh, data, _logger);
            _gpioExpander.SafeWriteAddressWord(GpioExpanderDigitalWriteLow, (ushort)~data, _logger);
        }

        public void PinMode(ushort pin, PinMode mode)
        {
            ushort data = Mask(pin);

            data = ReverseUInt16(data);

            switch (mode)
            {
                case Expander.PinMode.Input:
                    {
                        _gpioExpander.SafeWriteAddressWord(GpioExpanderPortModeInput, data, _logger);
                        break;
                    }
                case Expander.PinMode.InputPullUp:
                    {
                        _gpioExpander.SafeWriteAddressWord(GpioExpanderPortModePullUp, data, _logger);
                        break;
                    }
                case Expander.PinMode.InputPullDown:
                    {
                        _gpioExpander.SafeWriteAddressWord(GpioExpanderPortModePullDown, data, _logger);
                        break;
                    }
                case Expander.PinMode.Output:
                    {
                        _gpioExpander.SafeWriteAddressWord(GpioExpanderPortModeOutput, data, _logger);
                        break;
                    }
                default:
                    {
                        _logger.LogWarning(LogMessages.WarningForPinMode, nameof(I2CDeviceExtension.SafeWriteAddressWord), mode);
                        break;
                    }
            }
        }

        public void DigitalWrite(ushort pin, bool value)
        {
            ushort data = Mask(pin);

            data = ReverseUInt16(data);

            _gpioExpander.SafeWriteAddressWord(value ? GpioExpanderDigitalWriteHigh : GpioExpanderDigitalWriteLow, data, _logger);
        }

        public bool DigitalRead(ushort pin)
        {
            return (DigitalReadPort() & Mask(pin)) > 0;
        }

        #endregion

        #region Analog functions

        public void AnalogWrite(ushort pin, double value)
        {
            ushort data = (ushort)(value * AnalogWriteMultiplier);

            data = (ushort)((pin & 0xff) | ((data & 0xff) << 8));

            _gpioExpander.SafeWriteAddressWord(GpioExpanderAnalogWrite, data, _logger);
        }

        public double AnalogRead(ushort pin)
        {
            return AnalogRead16(pin) / AnalogReadDivisor;
        }

        public void PwmFreq(ushort freq)
        {
            _gpioExpander.SafeWriteAddressWord(GpioExpanderPwmFreq, ReverseUInt16(freq), _logger);
        }

        #endregion

        #region Shield settings

        public void ChangeAddress(ushort newAddress)
        {
            _gpioExpander.SafeWriteAddressWord(GpioExpanderChangeI2CAddress, newAddress, _logger);
        }

        public void SaveAddress()
        {
            _gpioExpander.SafeWrite(GpioExpanderSaveI2CAddress, _logger);
        }

        public void Reset()
        {
            _gpioExpander.SafeWrite(GpioExpanderReset, _logger);
        }

        #endregion

        #region Private function

        private ushort ReverseUInt16(ushort data)
        {
            return (ushort)(((data & 0xff) << 8) | ((data >> 8) & 0xff));
        }

        private ushort AnalogRead16(ushort pin)
        {
            _gpioExpander.SafeWriteAddressWord(GpioExpanderAnalogRead, pin, _logger);

            return ReverseUInt16(_gpioExpander.SafeReadAddressWord(GpioExpanderAnalogRead, _logger));
        }

        private ushort Mask(ushort pin)
        {
            return (ushort)(0x0001 << pin);
        }

        #endregion
    }
}

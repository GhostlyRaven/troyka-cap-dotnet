using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Abstractions.Native;

namespace TroykaCap.Expander.Extensions
{
    internal static class I2CDeviceExtension
    {
        internal static void SafeWrite(this II2CDevice device, byte data, ILogger logger)
        {
            try
            {
                device.Write(data);
            }
            catch (HardwareException error)
            {
                logger?.LogWarning(error, "{0} method. Unable to write data ({1}) to the I2C bus for transmission to the end device.", nameof(SafeWrite), data);

                return;
            }

            logger?.LogInformation("{0} method. Successfully to write data ({1}) to the I2C bus for transfer to the end device.", nameof(SafeWrite), data);
        }

        internal static ushort SafeReadAddressWord(this II2CDevice device, int address, ILogger logger)
        {
            ushort data = default;

            try
            {
                data = device.ReadAddressWord(address);
            }
            catch (HardwareException error)
            {
                logger?.LogWarning(error, "{0} method. Unable to read data ({1}) from I2C bus from end device ({2}).", nameof(SafeReadAddressWord), data, address);

                return data;
            }

            logger?.LogInformation("{0} method. Successfully to read data ({1}) from the I2C bus from the end device ({2}).", nameof(SafeReadAddressWord), data, address);

            return data;
        }

        internal static void SafeWriteAddressWord(this II2CDevice device, int address, ushort data, ILogger logger)
        {
            try
            {
                device.WriteAddressWord(address, data);
            }
            catch (HardwareException error)
            {
                logger?.LogWarning(error, "{0} method. Unable to write data ({1}) to the I2C bus for transmission to the end device ({2}).", nameof(SafeWriteAddressWord), data, address);

                return;
            }

            logger?.LogInformation("{0} method. Successfully to write data ({1}) to the I2C bus for transfer to the end device ({2}).", nameof(SafeWriteAddressWord), data, address);
        }
    }
}

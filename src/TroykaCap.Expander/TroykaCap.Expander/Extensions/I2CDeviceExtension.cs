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
                logger?.LogWarning(error, Messages.WarningForSafeWrite, nameof(SafeWrite), data);

                return;
            }

            logger?.LogInformation(Messages.InformationForSafeWrite, nameof(SafeWrite), data);
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
                logger?.LogWarning(error, Messages.WarningForSafeReadAddressWord, nameof(SafeReadAddressWord), data, address);

                return data;
            }

            logger?.LogInformation(Messages.InformationForSafeReadAddressWord, nameof(SafeReadAddressWord), data, address);

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
                logger?.LogWarning(error, Messages.WarningForSafeWriteAddressWord, nameof(SafeWriteAddressWord), data, address);

                return;
            }

            logger?.LogInformation(Messages.InformationForSafeWriteAddressWord, nameof(SafeWriteAddressWord), data, address);
        }
    }
}

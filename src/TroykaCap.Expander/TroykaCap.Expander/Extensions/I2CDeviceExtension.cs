using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Abstractions.Native;

namespace TroykaCap.Expander.Extensions
{
    internal static class I2CDeviceExtension
    {
        internal static ushort SafeReadAddressWord(this II2CDevice device, int address)
        {
            if (device is null)
            {
                return default;
            }

            try
            {
                return device.ReadAddressWord(address);
            }
            catch (HardwareException)
            {
                return default;
            }
        }

        internal static bool SafeWrite(this II2CDevice device, byte data)
        {
            if (device is null)
            {
                return false;
            }

            try
            {
                device.Write(data);

                return true;
            }
            catch (HardwareException)
            {
                return false;
            }
        }

        internal static bool SafeWriteAddressWord(this II2CDevice device, int address, ushort data)
        {
            if (device is null)
            {
                return false;
            }

            try
            {
                device.WriteAddressWord(address, data);

                return true;
            }
            catch (HardwareException)
            {
                return false;
            }
        }
    }
}
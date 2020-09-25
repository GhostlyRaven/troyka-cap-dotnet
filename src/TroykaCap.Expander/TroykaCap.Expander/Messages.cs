namespace TroykaCap.Expander
{
    internal static class Messages
    {
        internal const string InformationForSafeReadAddressWord = "{0} method. The reading is successful. Read data: {1}. Internal address: {2}.";
        internal const string WarningForSafeReadAddressWord = "{0} method. The reading is failed. Read data: {1}. Internal address: {2}.";

        internal const string InformationForSafeWriteAddressWord = "{0} method. The writing is successful. Write data: {1}. Internal address: {2}.";
        internal const string WarningForSafeWriteAddressWord = "{0} method. The writing is failed. Write data: {1}. Internal address: {2}.";

        internal const string InformationForSafeWrite = "{0} method. The writing is successful. Write data: {1}.";
        internal const string WarningForSafeWrite = "{0} method. The writing is failed. Write data: {1}.";

        internal const string WarningForPinMode = "{0} method. Invalid pin mode value. Pin mode: {1}.";

        internal const string ArgumentOutOfRangeException = "The I2C bus address must be between 0 and 127.";
        internal const string ArgumentNullException = "The I2C bus can't be null.";
    }
}

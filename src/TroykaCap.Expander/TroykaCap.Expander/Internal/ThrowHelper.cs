using System;

namespace TroykaCap.Expander.Internal
{
    internal static class ThrowHelper
    {
        internal static Exception ArgumentNullException(string paramName)
        {
            return new ArgumentNullException(paramName, "The object can't be null.");
        }

        internal static Exception ArgumentOutOfRangeException(string paramName, object actualValue)
        {
            return new ArgumentOutOfRangeException(paramName, actualValue, "The value is invalid.");
        }
    }
}

using System;

namespace TroykaCap.Expander
{
    /// <summary>
    /// Contains the inner exception GPIO expander.
    /// </summary>
    public sealed class ErrorEventArgs : EventArgs
    {
        private ErrorEventArgs(Exception error)
        {
            Error = error;
            HasValue = error != null;
        }

        /// <summary>
        /// Does the Error variable contain a value other than null.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Returns the inner exception GPIO expander.
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Cast the inner exception GPIO expander to type ErrorEventArgs.
        /// </summary>
        /// <param name="error">Inner exception GPIO expander.</param>
        /// <returns>Object of type ErrorEventArgs.</returns>
        public static implicit operator ErrorEventArgs(Exception error)
        {
            return new ErrorEventArgs(error);
        }
    }
}

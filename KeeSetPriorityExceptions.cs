using System;

namespace KeeSetPriority
{
    internal class KSPException : Exception
    {
        public KSPException() : base()
        {
        }

        public KSPException(string message) : base(message)
        {
        }

        public KSPException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
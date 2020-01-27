using System;

namespace ToolsPack.Samba
{
    public class NetworkDiskException : Exception
    {
        public NetworkDiskException(string message) : base(message)
        {
        }

        public NetworkDiskException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NetworkDiskException(): base()
        {
        }
    }
}
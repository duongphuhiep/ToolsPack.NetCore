using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using ToolsPack.String;

/// <summary>
/// Don't touch this class
/// https://stackoverflow.com/questions/40017663/create-file-get-file-and-delete-with-smb-protocol
/// </summary>
namespace ToolsPack.Samba
{
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
#pragma warning restore CA1051 // Do not declare visible instance fields
    }

#pragma warning disable CA1060 // Move pinvokes to native methods class
    public class NetworkConnection
#pragma warning restore CA1060 // Move pinvokes to native methods class
    {
        private readonly string _networkName;
        private readonly NetworkCredential _credentials;
        private readonly ILogger Log;
        private readonly LogLevel logLevel;

        public NetworkConnection(string networkName, NetworkCredential credentials, ILogger log = null, LogLevel logLevel = LogLevel.Debug)
        {
            Log = log;
            this.logLevel = logLevel;
            _networkName = networkName;
            _credentials = credentials;
        }

        /// <summary>
        /// connect
        /// </summary>
        public void Connect()
        {
            Connect(ResourceScope.GlobalNetwork, ResourceType.Disk, ResourceDisplaytype.Share);
        }

        /// <summary>
        /// throw NetworkDiskException<Win32Exception> if failed
        /// </summary>
        public void Connect(ResourceScope scope, ResourceType type, ResourceDisplaytype display)
        {
            var netResource = new NetResource
            {
                Scope = scope,
                ResourceType = type,
                DisplayType = display,
                RemoteName = _networkName
            };

            var userName = string.IsNullOrEmpty(_credentials.Domain)
                ? _credentials.UserName
                : $@"{_credentials.Domain}\{_credentials.UserName}";

            var ctx = $"{nameof(WNetAddConnection2)}(scope={netResource.Scope}, type={netResource.ResourceType}; display={netResource.DisplayType}, remote={_networkName}, user={userName}, password=***)";

            var connId = StringGenerator.CreateRandomString();

            Log?.Log(logLevel, $"Start open remote disk connection #{connId} {ctx}");
            Stopwatch sw = Stopwatch.StartNew();
            var result = WNetAddConnection2(
                netResource,
                _credentials.Password,
                userName,
                0);

            if (result != 0)
            {
                var win32error = new Win32Exception(result);
                ctx = $"Failed to connect with #{connId} {ctx}. Error {result} - {win32error.Message} / Elapsed: {sw.ElapsedMilliseconds} ms";
                Log?.LogError(ctx);
                throw new NetworkDiskException(ctx, win32error);
            }
            else
            {
                Log?.Log(logLevel, $"Success open remote disk connection #{connId}. Elapsed: {sw.ElapsedMilliseconds} ms");
            }
        }

        /// <summary>
        /// throw NetworkDiskException<Win32Exception> if failed
        /// </summary>
        public void Disconnect(int flag, bool force)
        {
            var ctx = $"{nameof(WNetCancelConnection2)}({_networkName})";
            var connId = StringGenerator.CreateRandomString();

            Log?.Log(logLevel, $"Disconnect remote disk #{connId} {ctx}");
            Stopwatch sw = Stopwatch.StartNew();
            var result = WNetCancelConnection2(_networkName, flag, force);

            if (result != 0)
            {
                var win32error = new Win32Exception(result);
                ctx = $"Failed to Disconnect with #{connId} {ctx}. Error {result} - {win32error.Message} / Elapsed: {sw.ElapsedMilliseconds} ms";
                Log?.LogError(ctx);
                throw new NetworkDiskException(ctx, win32error);
            }
            else
            {
                Log?.Log(logLevel, $"Success disconnect remote disk #{connId}. Elapsed: {sw.ElapsedMilliseconds} ms");
            }
        }

        //overload
        public void Disconnect()
        {
            Disconnect(0, true);
        }

#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);
#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
    }

    public enum ResourceScope
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    };

    public enum ResourceDisplaytype
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    };
}

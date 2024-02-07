using System;

namespace ToolsPack.Samba
{
    /// <summary>
    /// This class is used as Key in the cache dictionary of CifsConnectionManagerFactory
    /// so we must to implement the Equal and HashCode ourself
    /// </summary>
    public class FileStorageSetting : IEquatable<FileStorageSetting>
    {
        public string Domain { get; set; }

        /// <summary>
        /// we try to connect to this place
        /// </summary>
        public string RemoteLocation { get; set; }
        public string BaseLocation { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// it is just a combination of RemoteLocation\BaseLocation
        /// </summary>
        public string BasePath => string.IsNullOrEmpty(BaseLocation) ? RemoteLocation : System.IO.Path.Combine(RemoteLocation, BaseLocation);

        public bool Equals(FileStorageSetting other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Domain == other.Domain && RemoteLocation == other.RemoteLocation && BaseLocation == other.BaseLocation
                 && Login == other.Login && Password == other.Password;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }

            var other = (FileStorageSetting)obj;
            return Equals(other);
        }
        public override int GetHashCode()
        {
            unchecked //Allow overflow result number
            {
                // Choose large primes to avoid hashing collisions on large number
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                //use XOR for higher performence
                var hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Domain) ? Domain.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, RemoteLocation) ? RemoteLocation.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, BaseLocation) ? BaseLocation.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Login) ? Login.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Password) ? Password.GetHashCode() : 0);
                return hash;
            }
        }
    }
}

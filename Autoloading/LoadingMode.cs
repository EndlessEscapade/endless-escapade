using System;

namespace EEMod.Autoloading
{
    [Flags]
    public enum LoadingMode : byte
    {
        /// <summary>
        /// Indicates it will be for client and server.
        /// </summary>
        Both,
        /// <summary>
        /// Indicates it will be only for clients.
        /// </summary>
        Client,
        /// <summary>
        /// Indicates it will only be for servers.
        /// </summary>
        Server
    }
}
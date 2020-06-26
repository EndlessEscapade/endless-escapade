using System;
using Terraria;

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
    interface ILoadModeAttribute
    {
        LoadingMode LoadMode { get; }
    }
    internal static class LoadH
    {
        internal static bool ValidCurrent(LoadingMode mode) => mode == LoadingMode.Both || mode == LoadingMode.Server == Main.dedServ;
    }
}
using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using Terraria.ModLoader;

namespace LinuxMod.Core.Subworlds
{
    public static class Logging
    {
        public static readonly string LogDir = Path.Combine(Program.SavePath, "Logs");
        public static readonly string LogArchiveDir = Path.Combine(LogDir, "Old");

        internal const string side = "client";
        internal static readonly FieldInfo f_fileName = typeof(StackFrame).GetField("strFileName", BindingFlags.Instance | BindingFlags.NonPublic) ?? typeof(StackFrame).GetField("fileName", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly List<string> initWarnings = new List<string>();
        private static readonly HashSet<string> pastExceptions = new HashSet<string>();
        private static readonly HashSet<string> ignoreSources = new HashSet<string> { "MP3Sharp" };

        private static readonly List<string> ignoreContents = new List<string>
        {
            "System.Console.set_OutputEncoding",
            "Terraria.ModLoader.Core.ModCompile",
            "Delegate.CreateDelegateNoSecurityCheck",
            "MethodBase.GetMethodBody",
            "Terraria.Net.Sockets.TcpSocket.Terraria.Net.Sockets.ISocket.AsyncSend",
            "System.Diagnostics.Process.Kill",
            "Terraria.ModLoader.Core.AssemblyManager.CecilAssemblyResolver.Resolve",
            "Terraria.ModLoader.Engine.TMLContentManager.OpenStream"
        };

        private static readonly List<string> ignoreMessages = new List<string>
        {
            "A blocking operation was interrupted by a call to WSACancelBlockingCall",
            "The request was aborted: The request was canceled.",
            "Object name: 'System.Net.Sockets.Socket'.",
            "Object name: 'System.Net.Sockets.NetworkStream'",
            "This operation cannot be performed on a completed asynchronous result object.",
            "Object name: 'SslStream'.",
            "Unable to load DLL 'Microsoft.DiaSymReader.Native.x86.dll'"
        };

        private static readonly List<string> ignoreThrowingMethods = new List<string>
        {
            "at Terraria.Lighting.doColors_Mode",
            "System.Threading.CancellationToken.Throw"
        };

        private static readonly ThreadLocal<bool> handlerActive = new ThreadLocal<bool>(() => false);
        private static readonly Regex statusRegex = new Regex("(.+?)[: \\d]*%$");

        private static readonly Assembly TerrariaAssembly = Assembly.GetExecutingAssembly();

        public static string LogPath { get; private set; }

        internal static ILog Terraria { get; } = LogManager.GetLogger("Terraria");

        internal static ILog tML { get; } = LogManager.GetLogger("tML");

        private static bool CanOpen(string fileName)
        {
            try
            {
                using (new FileStream(fileName, FileMode.Append)) { }

                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        internal static void ResetPastExceptions() => pastExceptions.Clear();

        public static void IgnoreExceptionSource(string source) => ignoreSources.Add(source);

        public static void IgnoreExceptionContents(string source)
        {
            if (!ignoreContents.Contains(source))
            {
                ignoreContents.Add(source);
            }
        }

        internal static void LogStatusChange(string oldStatusText, string newStatusText)
        {
            string value = statusRegex.Match(oldStatusText).Groups[1].Value;
            string value2 = statusRegex.Match(newStatusText).Groups[1].Value;

            if (value2 != value && value2.Length > 0)
            {
                LogManager.GetLogger("StatusText").Info(value2);
            }
        }
    }
}
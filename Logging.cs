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
using Microsoft.Xna.Framework;
using log4net;

namespace EEMod
{
    public static class Logging
    {
        public static readonly string LogDir = Path.Combine(Program.SavePath, "Logs");

        public static readonly string LogArchiveDir = Path.Combine(LogDir, "Old");

        internal const string side = "client";

        private static List<string> initWarnings = new List<string>();

        private static HashSet<string> pastExceptions = new HashSet<string>();

        private static HashSet<string> ignoreSources = new HashSet<string>
    {
        "MP3Sharp"
    };

        private static List<string> ignoreContents = new List<string>
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

        private static List<string> ignoreMessages = new List<string>
    {
        "A blocking operation was interrupted by a call to WSACancelBlockingCall",
        "The request was aborted: The request was canceled.",
        "Object name: 'System.Net.Sockets.Socket'.",
        "Object name: 'System.Net.Sockets.NetworkStream'",
        "This operation cannot be performed on a completed asynchronous result object.",
        "Object name: 'SslStream'.",
        "Unable to load DLL 'Microsoft.DiaSymReader.Native.x86.dll'"
    };

        private static List<string> ignoreThrowingMethods = new List<string>
    {
        "at Terraria.Lighting.doColors_Mode",
        "System.Threading.CancellationToken.Throw"
    };

        private static ThreadLocal<bool> handlerActive = new ThreadLocal<bool>(() => false);

        private static Exception previousException;

        private static Regex statusRegex = new Regex("(.+?)[: \\d]*%$");

        internal static readonly FieldInfo f_fileName = typeof(StackFrame).GetField("strFileName", BindingFlags.Instance | BindingFlags.NonPublic) ?? typeof(StackFrame).GetField("fileName", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly Assembly TerrariaAssembly = Assembly.GetExecutingAssembly();

        public static string LogPath
        {
            get;
            private set;
        }

        internal static ILog Terraria
        {
            get;
        } = LogManager.GetLogger("Terraria");


        internal static ILog tML
        {
            get;
        } = LogManager.GetLogger("tML");






        private static string GetNewLogFile(string baseName)
        {
            Regex pattern = new Regex(baseName + "(\\d*)\\.log$");
            List<string> source = (from s in Directory.GetFiles(LogDir)
                                   where pattern.IsMatch(Path.GetFileName(s))
                                   select s).ToList();
            if (!source.All(CanOpen))
            {
                int num = source.Select(delegate (string s)
                {
                    string value = pattern.Match(Path.GetFileName(s)).Groups[1].Value;
                    return (value.Length == 0) ? 1 : int.Parse(value);
                }).Max();
                return $"{baseName}{num + 1}.log";
            }
            foreach (string item in source.OrderBy(File.GetCreationTime))
            {
                string text = ".old";
                int num2 = 0;
                while (File.Exists(item + text))
                {
                    text = $".old{++num2}";
                }
                try
                {
                    File.Move(item, item + text);
                }
                catch (IOException ex)
                {
                    initWarnings.Add($"Move failed during log initialization: {item} -> {Path.GetFileName(item)}{text}\n{ex}");
                }
            }
            return baseName + ".log";
        }

        private static bool CanOpen(string fileName)
        {
            try
            {
                using (new FileStream(fileName, FileMode.Append))
                {
                }
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }



        internal static void ResetPastExceptions()
        {
            pastExceptions.Clear();
        }

        public static void IgnoreExceptionSource(string source)
        {
            ignoreSources.Add(source);
        }

        public static void IgnoreExceptionContents(string source)
        {
            if (!ignoreContents.Contains(source))
            {
                ignoreContents.Add(source);
            }
        }



        private static void AddChatMessage(string msg, Color color)
        {
            if (!Main.gameMenu)
            {
                float soundVolume = Main.soundVolume;
                Main.soundVolume = 0f;
                Main.NewText(msg, color);
                Main.soundVolume = soundVolume;
            }
        }

        internal static void LogStatusChange(string oldStatusText, string newStatusText)
        {
            string value = statusRegex.Match(oldStatusText).Groups[1].Value;
            string value2 = statusRegex.Match(newStatusText).Groups[1].Value;
            if (value2 != value && value2.Length > 0)
            {
                LogManager.GetLogger("StatusText").Info((object)value2);
            }
        }






        private static void EnablePortablePDBTraces()
        {
            if (FrameworkVersion.Framework == Framework.NetFramework && FrameworkVersion.Version >= new Version(4, 7, 2))
            {
                Type.GetType("System.AppContextSwitches").GetField("_ignorePortablePDBsInStackTraces", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, -1);
            }
        }
    }
}

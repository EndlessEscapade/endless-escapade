using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Audio;
using Terraria.ObjectData;

namespace EEMod.Systems
{
	public static class LayeredMusic
	{
        /*
        public static int PreviousPoint;
        public static bool ShouldJumpToPoint;

        public delegate void orig_FillBuffer(byte[] buffer);
        public delegate void hook_FillBuffer(orig_FillBuffer orig, byte[] buffer);
        public static event hook_FillBuffer FillBuffer
        {
            add
            {
                HookEndpointManager.Add(typeof(MusicStreamingOGG).GetMethod("FillBuffer", BindingFlags.NonPublic | BindingFlags.Instance), value);
            }
            remove
            {
                HookEndpointManager.Remove(typeof(MusicStreamingOGG).GetMethod("FillBuffer", BindingFlags.NonPublic | BindingFlags.Instance), value);
            }
        }
        internal static void OnFillBuffer(orig_FillBuffer orig, byte[] buffer)
        {
            orig(buffer);
            Main.NewText("a");
        }
        */
    }
}

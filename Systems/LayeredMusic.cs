using MonoMod.RuntimeDetour.HookGen;
using NVorbis;
using System;
using System.Reflection;
using Terraria.ModLoader.Audio;

namespace EEMod.Systems
{
    /// <summary>
    /// Simply set ShouldJumpToPoint to true after setting a music and it (should) work properly.
    /// </summary>
	public static class LayeredMusic
	{
        public static long PreviousPoint;
        public static bool ShouldJumpToPoint;

        public delegate void orig_FillBuffer(MusicStreamingOGG self, byte[] buffer);
        public delegate void hook_FillBuffer(orig_FillBuffer orig, MusicStreamingOGG self, byte[] buffer);
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
        internal static void OnFillBuffer(orig_FillBuffer orig, MusicStreamingOGG self, byte[] buffer)
        {
            Type type = typeof(MusicStreamingOGG);
            FieldInfo fieldInfo = type.GetField("reader", BindingFlags.NonPublic | BindingFlags.Instance);
            if (ShouldJumpToPoint && (fieldInfo.GetValue(self) as VorbisReader).DecodedPosition < 210000) //Closest thing to "1st frame", probably a better way to do this
            {
                (fieldInfo.GetValue(self) as VorbisReader).DecodedPosition = PreviousPoint;
                ShouldJumpToPoint = false;
            }
            PreviousPoint = (fieldInfo.GetValue(self) as VorbisReader).DecodedPosition;
            orig(self, buffer);
        }
    }
}

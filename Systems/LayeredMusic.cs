using System;
using System.Reflection;
using NVorbis;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader.Audio;

namespace EEMod.Systems
{
    /// <summary>
    /// Simply set ShouldLayerMusic to true right before setting a music and it should start where the last left off. 
    /// </summary>
	public static class LayeredMusic
	{
        public static long PreviousPoint;
        public static bool ShouldLayerMusic;

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
            //I have no idea how resource intensive this reflection is but hopefully not too much.
            //Will probably change this to an IL edit later ~Exitium.
            Type type = typeof(MusicStreamingOGG);
            FieldInfo fieldInfo = type.GetField("reader", BindingFlags.NonPublic | BindingFlags.Instance);
            if (ShouldLayerMusic && (fieldInfo.GetValue(self) as VorbisReader).DecodedPosition == 0)
            {
                (fieldInfo.GetValue(self) as VorbisReader).DecodedPosition = PreviousPoint;
                ShouldLayerMusic = false;
            }
            PreviousPoint = (fieldInfo.GetValue(self) as VorbisReader).DecodedPosition;
            orig(self, buffer);
        }
    }
}

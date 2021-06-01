using System;
using System.Reflection;
using NVorbis;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader.Audio;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace EEMod.Systems
{
    /// <summary>
    /// Simply set ShouldLayerMusic to true right before setting a music and it should start where the last left off. 
    /// </summary>
	public static class LayeredMusic
	{
        public static long PreviousPoint;
        public static bool ShouldLayerMusic;
        public static void ILFillBuffer(ILContext il)
        {
            ILCursor c = new ILCursor(il).Goto(0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(MusicStreamingOGG).GetField("reader", BindingFlags.NonPublic | BindingFlags.Instance));
            c.EmitDelegate<Action<MusicStreamingOGG, VorbisReader>>((musicStreamingOGG, vorbisReader) =>
            {
                if (ShouldLayerMusic && vorbisReader.DecodedPosition == 0)
                {
                    vorbisReader.DecodedPosition = PreviousPoint;
                    ShouldLayerMusic = false;
                }
                PreviousPoint = vorbisReader.DecodedPosition;
            });
        }
    }
}

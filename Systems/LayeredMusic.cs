using System;
using System.Reflection;
using NVorbis;
using Terraria.ModLoader.Audio;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria;
using System.Collections.Generic;

namespace EEMod.Systems
{
    /// <summary>
    /// Simply set ShouldLayerMusic to true right before setting a music and it should start where the last left off. 
    /// </summary>
	public static class LayeredMusic
	{
        public static long PreviousPoint;
        public static long PreviousTicks;
        public static bool ShouldLayerMusic;
        public static int OldMusic;
        public static Dictionary<int, string> Groups = new Dictionary<int, string>();
        public static void ILFillBuffer(ILContext il)
        {
            ILCursor c = new ILCursor(il).Goto(0);
            c.Emit(OpCodes.Ldarg_0); // push 'this'
            c.Emit(OpCodes.Ldarg_0); // push 'this'
            c.Emit(OpCodes.Ldfld, typeof(MusicStreamingOGG).GetField("reader", BindingFlags.NonPublic | BindingFlags.Instance)); // load the reader field
            c.EmitDelegate<Action<MusicStreamingOGG, VorbisReader>>((musicStreamingOGG, vorbisReader) =>
            {
                if (ShouldLayerMusic && vorbisReader.DecodedPosition == 0 && PreviousTicks <= vorbisReader.TotalTime.Ticks)
                {
                    if (Groups.ContainsKey(Main.curMusic) && Groups.ContainsKey(OldMusic) && Groups[Main.curMusic] == Groups[OldMusic])
                    {
                        vorbisReader.DecodedPosition = PreviousPoint;
                    }
                    OldMusic = Main.curMusic;
                    ShouldLayerMusic = false;
                }
                PreviousTicks = vorbisReader.DecodedTime.Ticks;
                PreviousPoint = vorbisReader.DecodedPosition;
            });
        }
    }
}

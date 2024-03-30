using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using NVorbis;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

// Makes music tracks play subsequently, providing smooth transition between different songs.
[Autoload(Side = ModSide.Client)]
public sealed class LayeredMusic : ILoadable
{
    private static int previousMusic;
    private static long previousSamplePosition;

    void ILoadable.Load(Mod mod) {
        IL_OGGAudioTrack.PrepareBufferToSubmit += PrepareBufferToSubmitPatch;
    }

    void ILoadable.Unload() { }

    private static void PrepareBufferToSubmitPatch(ILContext il) {
        try {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i =>
                    i.MatchCallOrCallvirt(typeof(OGGAudioTrack).GetMethod("ApplyTemporaryBufferTo", BindingFlags.NonPublic | BindingFlags.Static)))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(LayeredMusic)} disabled: Failed to match IL instruction: {nameof(OpCodes.Callvirt)}");
                return;
            }

            c.Index--;

            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldfld, typeof(OGGAudioTrack).GetField("_vorbisReader", BindingFlags.NonPublic | BindingFlags.Instance));

            c.EmitDelegate((VorbisReader reader) => {
                if (Main.curMusic != previousMusic && previousSamplePosition < reader.TotalSamples) {
                    reader.SamplePosition = previousSamplePosition;
                }

                previousMusic = Main.curMusic;
                previousSamplePosition = reader.SamplePosition;
            });
        }
        catch (Exception) {
            MonoModHooks.DumpIL(EndlessEscapade.Instance, il);
        }
    }
}

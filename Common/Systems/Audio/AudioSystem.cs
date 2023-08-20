using System.Collections.Immutable;
using System.Reflection;
using EndlessEscapade.Common.Systems.Audio.Filters;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio;

[Autoload(Side = ModSide.Client)]
public class AudioSystem : ModSystem
{
    private static readonly FieldInfo trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", ReflectionUtils.PrivateInstanceFlags);

    public static readonly ImmutableArray<SoundStyle> IgnoredSounds = ImmutableArray.Create(
        SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab
    );

    public static AudioParameters SoundParameters { get; private set; }
    public static AudioParameters MusicParameters { get; private set; }

    public static void SetParameters(AudioParameters sound, AudioParameters music) {
        SoundParameters = sound;
        MusicParameters = music;
    }

    public static void ResetParameters() {
        SoundParameters = default;
        MusicParameters = default;
    }

    public static void ApplyParameters(SoundEffectInstance instance, AudioParameters parameters) {
        LowPassSystem.ApplyParameters(instance, parameters);

    }

    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }

    public override void Load() {
        if (trackedSoundsField == null) {
            Mod.Logger.Error("Audio effects were disabled: Could not find internal Terraria members.");
            return;
        }

        On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += SoundEnginePlayHook;
        On_SoundEngine.Update += SoundEngineUpdateHook;
    }
    
    private static SlotId SoundEnginePlayHook(On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig, ref SoundStyle style, Vector2? position, SoundUpdateCallback callback) {
        var slot = orig(ref style, position, callback);

        if (IgnoredSounds.Contains(style) || !SoundEngine.TryGetActiveSound(slot, out ActiveSound result) || result.Sound.IsDisposed) {
            return slot;
        }

        ApplyParameters(result.Sound, SoundParameters);

        return slot;
    }

    private static void SoundEngineUpdateHook(On_SoundEngine.orig_Update orig) {
        orig();

        var value = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(SoundEngine.SoundPlayer);

        foreach (var item in value) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (IgnoredSounds.Contains(sound.Style) || !sound.IsPlaying || instance.IsDisposed) {
                continue;
            }

            ApplyParameters(instance, SoundParameters);
        }

        SoundParameters = default;
    }
}

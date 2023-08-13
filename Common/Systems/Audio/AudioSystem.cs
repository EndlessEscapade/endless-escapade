using System.Collections.Immutable;
using System.Reflection;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio;

[Autoload(Side = ModSide.Client)]
public class AudioSystem : ModSystem
{
    private static readonly FieldInfo trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", BindingFlags.Instance | BindingFlags.NonPublic);

    public static readonly ImmutableArray<SoundStyle> IgnoredSounds = ImmutableArray.Create(
        SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat
    );

    public static AudioModifiers SoundModifiers { get; private set; }
    public static AudioModifiers MusicModifiers { get; private set; }

    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }

    public override void OnModLoad() {
        On_SoundPlayer.Update += SoundPlayerUpdateHook;
    }

    public static void SetModifiers(AudioModifiers sound = default, AudioModifiers music = default) {
        SoundModifiers = sound;
        MusicModifiers = music;
    }

    private static void SoundPlayerUpdateHook(On_SoundPlayer.orig_Update orig, SoundPlayer self) {
        var value = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(self);

        foreach (var item in value) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (IgnoredSounds.Contains(sound.Style) || !sound.IsPlaying || instance.IsDisposed) {
                continue;
            }

            LowPassSystem.ApplyModifiers(instance, SoundModifiers);
        }

        orig(self);
    }
}

using EndlessEscapade.Common.Config;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceSystem : ModSystem
{
    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported && ClientSideConfig.Instance.ToggleAmbience;
    }

    public override void PostUpdateEverything() {
        foreach (AmbienceTrack track in ModContent.GetContent<AmbienceTrack>()) {
            track.Update();
        }
    }
}
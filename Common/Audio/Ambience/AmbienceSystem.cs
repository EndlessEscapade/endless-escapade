using EndlessEscapade.Common.Configuration;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio.Ambience;

[Autoload(Side = ModSide.Client)]
public class AmbienceSystem : ModSystem
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
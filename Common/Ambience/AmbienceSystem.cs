using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

// TODO: I hate this. Switch to a data oriented approach later.
[Autoload(Side = ModSide.Client)]
public sealed class AmbienceSystem : ModSystem
{
    public override void PostUpdateEverything() {
        foreach (var track in ModContent.GetContent<AmbienceTrack>()) {
            track.Update();
        }
    }
}

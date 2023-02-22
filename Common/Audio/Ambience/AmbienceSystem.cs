using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceSystem : ModSystem
{
    public override void PostSetupContent() {
        foreach (AmbienceTrack track in ModContent.GetContent<AmbienceTrack>()) {
            track.Setup();
        }
    }

    public override void PostUpdateEverything() {
        foreach (AmbienceTrack track in ModContent.GetContent<AmbienceTrack>()) {
            track.Update();
        }
    }
}
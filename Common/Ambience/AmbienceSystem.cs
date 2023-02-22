using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbienceSystem : ModSystem
{
    public override void PostSetupContent() {
        foreach (AmbienceTrack track in ModContent.GetContent<AmbienceTrack>())
            track.Setup();
    }

    public override void OnModLoad() {
        On.Terraria.Main.UpdateAudio += UpdateAmbience;
    }

    public override void OnModUnload() {
        On.Terraria.Main.UpdateAudio -= UpdateAmbience;
    }

    private static void UpdateAmbience(On.Terraria.Main.orig_UpdateAudio orig, Terraria.Main self) {
        orig(self);

        foreach (AmbienceTrack track in ModContent.GetContent<AmbienceTrack>())
            track.Update();
    }
}
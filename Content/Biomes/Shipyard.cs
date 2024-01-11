using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Biomes;

public class Shipyard : ModBiome
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

    public override int Music => MusicLoader.GetMusicSlot(Mod, $"Assets/Sounds/Music/Shipyard{(Main.dayTime ? "Day" : "Night")}");

    public override bool IsBiomeActive(Player player) {
        var isLeftSide = player.position.X / 16f < Main.maxTilesX / 2f;

        return player.ZoneBeach && isLeftSide;
    }
}

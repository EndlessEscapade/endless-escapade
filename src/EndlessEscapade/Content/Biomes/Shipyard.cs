﻿using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Biomes;

public class Shipyard : ModBiome
{
    public override int Music => MusicLoader.GetMusicSlot(Mod, $"Assets/Sounds/Music/Shipyard{(Main.dayTime ? "Day" : "Night")}");

    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

    public override bool IsBiomeActive(Player player) {
        return player.ZoneBeach && player.position.X / 16f < Main.maxTilesX / 2f;
    }
}

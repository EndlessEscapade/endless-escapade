using System.Collections.Generic;
using EndlessEscapade.Common.Systems.Generation.Loot;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Ocean;

public class RadiantPearl : ModItem, IChestLoot
{
    public IReadOnlyList<int> Frames { get; } = new[] { (int)ChestFrame.Water };

    public int Chance { get; } = 5;
    
    public override void SetDefaults() {
        Item.DefaultToAccessory();
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.findTreasure = true;
    }
}

using System.Collections.Generic;
using EndlessEscapade.Common.Systems.Loot;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Ocean;

public class DolphinFin : ModItem, IChestLoot
{
    public IReadOnlyList<ChestFrame> Frames { get; } = new[] {
        ChestFrame.Water
    };
    
    public int Chance { get; } = 5;
    
    public override void SetDefaults() {
        Item.DefaultToAccessory();
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.accFlipper = true;
        player.ignoreWater = true;
    }
}

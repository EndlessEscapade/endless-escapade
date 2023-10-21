using System.Collections.Generic;
using EndlessEscapade.Common.Systems.Generation.Loot;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Ocean;

[AutoloadEquip(EquipType.Shield)]
public class GreatShell : ModItem, IChestLoot
{
    public IReadOnlyList<int> Frames { get; } = new[] { (int)ChestFrame.Water };

    public int Chance { get; } = 5;

    public override void SetDefaults() {
        Item.DefaultToAccessory();
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.statDefense += 5;
    }
}
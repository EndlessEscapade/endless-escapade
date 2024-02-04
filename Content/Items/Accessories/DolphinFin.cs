using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Accessories;

public class DolphinFin : ModItem
{
    public override void SetDefaults() {
        Item.accessory = true;

        Item.width = 22;
        Item.height = 26;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.ignoreWater = true;
    }
}

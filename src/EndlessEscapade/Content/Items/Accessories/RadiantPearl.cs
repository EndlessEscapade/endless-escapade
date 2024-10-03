using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Accessories;

public class RadiantPearl : ModItem
{
    public override void SetDefaults() {
        Item.accessory = true;

        Item.width = 36;
        Item.height = 36;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        Main.instance.SpelunkerProjectileHelper.AddSpotToCheck(player.Center);
    }
}

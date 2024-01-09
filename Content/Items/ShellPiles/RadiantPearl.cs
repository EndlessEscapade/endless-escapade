using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace EndlessEscapade.Content.Items.ShellPiles;

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

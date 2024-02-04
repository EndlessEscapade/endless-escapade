using EndlessEscapade.Content.Items.Materials;
using EndlessEscapade.Content.Projectiles.Typeless;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tools;

public class MagicConch : ModItem
{
    public override void SetDefaults() {
        Item.fishingPole = 15;

        Item.width = 52;
        Item.height = 44;

        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.UseSound = SoundID.Item1;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override bool? UseItem(Player player)
    {
        SubworldLibrary.SubworldSystem.Enter("EndlessEscapade/Sea");

        return base.UseItem(player);
    }
}

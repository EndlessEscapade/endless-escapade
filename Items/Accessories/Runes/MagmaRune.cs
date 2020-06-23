using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using EEMod.Projectiles.Runes;

namespace EEMod.Items.Accessories.Runes
{
    public class MagmaRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Rune");
            Tooltip.SetDefault("4% increased damage" + "\nTrue melee attacks deals the" + "\ndebuff OnFire for 2 seconds");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 20;
            item.height = 20;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 4;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 32);
            item.shoot = ModContent.ProjectileType<IgnisRune>();
        }
    }
}
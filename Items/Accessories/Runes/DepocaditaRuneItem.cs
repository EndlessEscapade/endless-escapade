using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using EEMod.Projectiles.Runes;

namespace EEMod.Items.Accessories.Runes
{
    public class DepocaditaRuneItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Decapodita Rune");
            Tooltip.SetDefault("3% increased critical strike" + "\nchance while in the Mushroom Biome" + "\n5% increased damage while" + "\nin the Mushroom Biome" + "\n7% decreased velocity while out of" + "\nthe Mushroom Biome");
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
            item.shoot = ModContent.ProjectileType<DepocaditaRune>();
        }
    }
}

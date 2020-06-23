using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using EEMod.Projectiles.Runes;

namespace EEMod.Items.Accessories.Runes
{
    public class StirringSandsRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stirring Sands Rune");
            Tooltip.SetDefault("6% increased attack speed + \nBy pressing [Bind Key] in the air you will summon a sand cyclone");
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
            item.shoot = ModContent.ProjectileType<DesertRune>();
        }
    }
}
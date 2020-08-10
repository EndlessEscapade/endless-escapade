using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles;

namespace EEMod.Items.Weapons.Melee
{
    public class DuelistsBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist's Blade");
            Tooltip.SetDefault("It once belonged to an ancient warrior of times past");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.rare = ItemRarityID.LightRed;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 7f; // 5 and 1/4
            item.useTime = 15;
            item.useAnimation = 15;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.damage = 1007;
            item.width = 128;
            item.height = 128;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<DuelistBladeProj>();
        }
    }
}
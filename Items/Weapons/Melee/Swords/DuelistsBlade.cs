using EEMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class DuelistsBlade : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist's Blade");
            Tooltip.SetDefault("It once belonged to an ancient warrior of times past");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f; // 5 and 1/4
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.damage = 20;
            Item.width = 128;
            Item.height = 128;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<DuelistBladeProj>();
        }
    }
}
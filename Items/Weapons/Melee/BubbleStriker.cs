using EEMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class BubbleStrikerMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Striker");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 180;
            item.useTime = 180;
            item.shootSpeed = 0;
            item.knockBack = 6.5f;
            item.width = 46;
            item.height = 48;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.channel = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<BubbleStrikerProjectile>();
        }
    }
}
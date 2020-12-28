using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using EEMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger
{
    public class ShimmerShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shimmer Shot");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 200;
            item.useTime = 200;
            item.shootSpeed = 4;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item1;
            //item.shoot = ModContent.ProjectileType<Pog>();
        }
    }
}

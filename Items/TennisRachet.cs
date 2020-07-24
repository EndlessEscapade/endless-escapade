using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles;

namespace EEMod.Items
{
    public class TennisRachet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tennis Rachet");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 1f;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.noMelee = true;
            item.ranged = true;
            item.damage = 20;
            item.useTime = 10;
            item.useAnimation = 10;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.autoReuse = true;
            item.knockBack = 6f;
            item.UseSound = SoundID.Item11;
            item.crit = 1;
            item.noUseGraphic = true;
        }

        int yeet;
        float alpha;
        int proj;

        public override void HoldItem(Player player)
        {
            if(player.controlUseItem && yeet == 0)
            {
                proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<TennisRachetProj>(), 0, 0f, player.whoAmI);
                yeet = 1;
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.TennisRackets;

namespace EEMod.Items
{
    public class TennisRacket : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tennis Racket");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 1f;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.noMelee = false;
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

        public int yeet;
        private int proj;

        public override void UpdateInventory(Player player)
        {
            yeet = 0;
        }

        public override void HoldItem(Player player)
        {
            if (player.controlUseItem && yeet == 0 && Main.myPlayer == player.whoAmI)
            {
                proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<TennisRacketProj>(), 0, 0f, player.whoAmI);
                yeet = 1;
                Main.projectile[proj].netUpdate = true;
            }
            if (yeet == 1)
            {
                yeet = 1;
            }
        }
    }
}
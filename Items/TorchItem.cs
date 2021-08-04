using EEMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class TorchItem : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashlight");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldingOut;
            Item.shootSpeed = 1f;
            Item.rare = ItemRarityID.Orange;
            Item.width = 20;
            Item.shoot = ModContent.ProjectileType<Torch>();
            Item.height = 20;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.autoReuse = true;
            Item.knockBack = 6f;
            Item.crit = 1;
        }

        private int yeet;
        private float alpha;
        private int proj = -1;

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
        }

        public override void HoldItem(Player player)
        {
            if(proj != -1) Main.projectile[proj].ai[0] = alpha;
            if (player.controlUseItem && yeet == 0)
            {
                proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Gradient>(), 0, 0f, player.whoAmI);
                yeet = 1;
            }
            if (yeet == 1)
            {
                if (player.controlUseItem)
                {
                    alpha += 0.05f;
                    if (alpha > 1)
                    {
                        alpha = 1;
                    }
                }
                else
                {
                    alpha = 0;
                    yeet = 0;
                    Main.projectile[proj].Kill();
                    proj = -1;
                }
            }
        }
    }
}
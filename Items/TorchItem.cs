using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles;

namespace EEMod.Items
{
    public class TorchItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torch");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 1f;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.shoot = ModContent.ProjectileType<Torch>();
            item.height = 20;
            item.noMelee = true;
            item.ranged = true;
            item.damage = 20;
            item.useTime = 1;
            item.useAnimation = 1;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.autoReuse = true;
            item.knockBack = 6f;
            item.crit = 1;
        }

        int yeet;
        float alpha;
        int proj;
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
        }
        public override void HoldItem(Player player)
        {
            Main.projectile[proj].ai[0] = alpha;
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
                    alpha -= 0.25f;
                    if (alpha < 0)
                    {
                        alpha = 0;
                        yeet = 0;
                        Main.projectile[proj].Kill();
                    }
                }
            }
        }
    }
}

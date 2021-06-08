using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class KelpvineCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpvine Cannon");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.useAnimation = 24;
            item.useTime = 24;

            item.shootSpeed = 8f;
            item.width = 44;
            item.height = 26;

            item.rare = ItemRarityID.Green;

            item.shoot = ModContent.ProjectileType<KelpHookProj>();

            item.autoReuse = false; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item11;
        }

        public override bool CanUseItem(Player player)
        {
            return true; //player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(player.altFunctionUse == 0)
                return true;

            else
            {
                for(int i = 0; i < Main.maxProjectiles; i++)
                {
                    if(Main.projectile[i].type == item.shoot && Main.projectile[i].owner == player.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }

                return false;
            }
        }
    }
}
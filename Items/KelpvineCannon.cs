using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class KelpvineCannon : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpvine Cannon");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.HoldingOut;

            Item.useAnimation = 24;
            Item.useTime = 24;

            Item.shootSpeed = 8f;
            Item.width = 44;
            Item.height = 26;

            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<KelpHookProj>();

            Item.autoReuse = false; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            Item.UseSound = SoundID.Item11;
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
                    if(Main.projectile[i].type == Item.shoot && Main.projectile[i].owner == player.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }

                return false;
            }
        }
    }
}
using EEMod.Buffs.Buffs;
using EEMod.Projectiles.Summons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Summon
{
    public class PrismaticCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Cane");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.summon = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 13;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 38;
            item.height = 36;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<PrismaticCaneProj>();
        }

        public override bool CanUseItem(Player player)
        {
            return Main.LocalPlayer.ownedProjectileCounts[item.shoot] < Main.LocalPlayer.maxMinions;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Helpers.TexToDust("thonk",Main.MouseWorld,5,1,100);
            if (player.altFunctionUse == 0)
            {
                Vector2 comedy = Vector2.Normalize(Main.MouseWorld - player.Center);
                Projectile projectile2 = Projectile.NewProjectileDirect(player.Center, comedy, ModContent.ProjectileType<PrismaticCaneProj>(), 10, 10f, Main.myPlayer);
            }
            if (player.altFunctionUse == 2)
            {

            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
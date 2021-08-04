using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.FishingBobbers;

namespace EEMod.Items.FishingPoles
{
    public class FishingPoleOfTheSevenSeas : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fishing Pole of the Seven Seas");
            ItemID.Sets.CanFishInLava[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodFishingPole);
            Item.fishingPole = 30;
            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<FishingPoleOfTheSevenSeasBobber>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int bobberAmount = 3;
            float spreadAmount = 75f;
            for (int index = 0; index < bobberAmount; ++index)
            {
                float SpeedX = speedX + Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f;
                float SpeedY = speedY + Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f;
                Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, 0, 0f, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
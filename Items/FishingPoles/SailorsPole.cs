using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.FishingPoles
{
    public class SailorsPole : ModItem
    {
        // You can use vanilla textures by using the format: Terraria/Item_<ID>
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Pole");
            //Tooltip.SetDefault("Fires multiple lines at once. Can fish in lava.");
            //Allows the pole to fish in lava
            ItemID.Sets.CanFishInLava[item.type] = true;
        }

        public override void SetDefaults()
        {
            //These are copied through the CloneDefaults method
            //item.useStyle = 1;
            //item.useAnimation = 8;
            //item.useTime = 8;
            //item.width = 24;
            //item.height = 28;
            //item.UseSound = SoundID.Item1;
            item.CloneDefaults(ItemID.WoodFishingPole);
            item.fishingPole = 30;
            item.shootSpeed = 14f;
            item.shoot = ModContent.ProjectileType<SailorsPoleBobber>();
        }

        //Overrides the default shooting method to fire multiple bobbers
        //NOTE: This will allow the fishing rod to summon multiple Duke Fishrons with multiple Truffle Worms in the inventory
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int bobberAmount = 1;
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
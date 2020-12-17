using EEMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Ranger
{
    public class BubbleBlitzer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Blitzer");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.ranged = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 10;
            item.useTime = 25;
            item.useAnimation = 25;
            item.width = 20;
            item.height = 20;
            item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 20f;
			item.useAmmo = AmmoID.Bullet;
            item.rare = ItemRarityID.Orange;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
        }
        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
        public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useTime = 8;
                item.useAnimation = 8;
                item.autoReuse = true;
                item.shootSpeed = 4f;
			}
			else
			{
                item.useTime = 25;
                item.useAnimation = 25;
				item.autoReuse = false;
                item.shootSpeed = 20f;
			}
			return true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<BlitzBubble>();
                knockBack = 0;
                Vector2 direction = new Vector2(speedX,speedY).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.85f, 1.15f);
                speedX = direction.X;
                speedY = direction.Y;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
    }
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Ranger.Guns
{
    public class BubbleBlitzer : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Blitzer");
        }

        public override void SetDefaults()
        {
            // Item.melee = false;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 12;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.width = 20;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 20f;
			Item.useAmmo = AmmoID.Bullet;
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
        }
        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
        public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 8;
                Item.useAnimation = 8;
                Item.autoReuse = true;
                Item.shootSpeed = 4f;
			}
			else
			{
                Item.useTime = 25;
                Item.useAnimation = 25;
				// Item.autoReuse = false;
                Item.shootSpeed = 20f;
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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

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
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 12;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.width = 20;
            Item.height = 20;
            Item.shoot = ModContent.ProjectileType<BlitzBubble>();
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

                return true;
            }
			else
			{
                Item.useTime = 25;
                Item.useAnimation = 25;
				Item.autoReuse = false;
                Item.shootSpeed = 20f;

                return true;
            }
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 direction = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.85f, 1.15f);

                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_ItemUse(player, Item), position, direction, ModContent.ProjectileType<BlitzBubble>(), damage, knockback);

                return false;
            }
            else
            {
                return true;
            }
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
    }
}
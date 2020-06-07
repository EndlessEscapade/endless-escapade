using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InteritosMod.Projectiles.Mage;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Items.Weapons.Mage
{
    public class DalantiniumKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Knives");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.magic = true;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 21);
            item.mana = 7;
            item.shootSpeed = 7f;
            item.useTime = 22;
            item.useAnimation = 22;
            item.rare = ItemRarityID.Green;
            item.width = 20;
            item.height = 20;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<DalantiniumFang>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -2);
        }

        /* public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numProj = 3;
			float rotation = MathHelper.ToRadians(25);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numProj; i++)
			{
				Vector2 pertSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, pertSpeed.X, pertSpeed.Y, type, damage, knockBack, player.whoAmI);
			}

			return false;
		} */

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 5);
            recipe.AddIngredient(ItemID.ThrowingKnife, 20);
            recipe.AddTile(TileID.Anvils);
        }
    }
}
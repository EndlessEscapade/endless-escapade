using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class SaharaSceptoid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sahara Sceptoid");
            Tooltip.SetDefault("Shoots a beam that splits on impact with a solid surface");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.magic = true;
            item.mana = 7;
            item.width = 46;
            item.height = 46;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = 10000;
            item.UseSound = SoundID.Item73;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Crystal");
            item.shootSpeed = 6f;
            item.rare = 3;
            item.crit = 5;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX *= 2f;
            speedY *= 2f;
            return true;
        }
        public override void AddRecipes()
        {
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.Diamond, 1);
                recipe.AddIngredient(mod.ItemType("MummifiedRag"), 7);
                recipe.AddIngredient(ItemID.Sandstone, 15);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}

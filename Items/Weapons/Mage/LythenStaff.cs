using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using InteritosMod.Items.Placeables.Ores;
using InteritosMod.Projectiles.Mage;

namespace InteritosMod.Items.Weapons.Mage
{
    public class LythenStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Staff");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.width = 32;
            item.height = 32;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 0;
            item.rare = ItemRarityID.Green;
            item.autoReuse = false;
            item.crit = 4;
            item.noMelee = true;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<LythenStaffProjectile>();
            item.shootSpeed = 16f;
            item.mana = 2;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.HoldingOut;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, -4);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LythenBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

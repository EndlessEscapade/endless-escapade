using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class LythenWarhammer : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidebreaker");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.shootSpeed = 0f;
            Item.knockBack = 6.5f;
            Item.width = 54;
            Item.height = 60;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.melee = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<LythenWarhammerProjectile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LythenBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<LythenWarhammerProjectile>()] >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
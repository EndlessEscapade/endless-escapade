using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class LythenWarhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidebreaker");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 320;
            item.useAnimation = 320;
            item.shootSpeed = 0f;
            item.knockBack = 6.5f;
            item.width = 54;
            item.height = 60;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.channel = true;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<LythenWarhammerProjectile>();
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
            if(player.ownedProjectileCounts[ModContent.ProjectileType<LythenWarhammerProjectile>()] >= 1)
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
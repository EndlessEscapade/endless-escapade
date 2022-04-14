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
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.shootSpeed = 0f;
            Item.knockBack = 6.5f;
            Item.width = 54;
            Item.height = 60;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            // Item.autoReuse = false;
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<LythenWarhammerProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LythenBar>(), 10).AddTile(TileID.Anvils).Register();
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
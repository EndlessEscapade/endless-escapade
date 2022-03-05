using EEMod.Items.Materials;
using EEMod.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace EEMod.Items.Placeables.Ores
{
    public class LythenBar : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Bar");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 7));
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 40;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            //Item.createTile = ModContent.TileType<LythenBarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LythenOre>(), 3).AddIngredient(ModContent.ItemType<HydrosScales>(), 1).AddTile(TileID.Furnaces).Register();
        }

        public override void PostUpdate()
        {

        }
    }
}
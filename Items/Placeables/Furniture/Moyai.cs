using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Foliage.ThermalVents;
using EEMod.Tiles.Foliage.Aquamarine;
using EEMod.Tiles.Foliage.KelpForest;
using EEMod.Tiles.Furniture.NautilusPuzzle;
using EEMod.Tiles.Foliage.Halocline;
using EEMod.Tiles.Furniture.Chests;
using EEMod.Tiles.Furniture.Shipyard;

namespace EEMod.Items.Placeables.Furniture
{
    public class Moyai : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moyai");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.White;

            Item.createTile = ModContent.TileType<FigureheadTile>();
        }
    }
}
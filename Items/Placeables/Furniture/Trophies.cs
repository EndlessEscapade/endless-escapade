using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;

namespace EEMod.Items.Placeables.Furniture
{
    public class HydrosTrophy : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.createTile = ModContent.TileType<TrophiesTile>();
            item.placeStyle = 0;
        }
    }
    public class OmenTrophy : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.createTile = ModContent.TileType<TrophiesTile>();
            item.placeStyle = 1;
        }
    }
    public class AkumoTrophy : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.createTile = ModContent.TileType<TrophiesTile>();
            item.placeStyle = 2;
        }
    }
    public class KrakenTrophy : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.createTile = ModContent.TileType<TrophiesTile>();
            item.placeStyle = 3;
        }
    }
    public class TalosTrophy : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.createTile = ModContent.TileType<TrophiesTile>();
            item.placeStyle = 4;
        }
    }
    public class CoralGolemTrophy : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;
            item.createTile = ModContent.TileType<TrophiesTile>();
            item.placeStyle = 5;
        }
    }
}
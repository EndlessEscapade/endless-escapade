using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public abstract class ThropyItem : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
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

    public class HydrosTrophy : ThropyItem { }

    public class OmenTrophy : ThropyItem { }

    public class AkumoTrophy : ThropyItem { }

    public class KrakenTrophy : ThropyItem { }

    public class TalosTrophy : ThropyItem { }

    public class CoralGolemTrophy : ThropyItem { }
}
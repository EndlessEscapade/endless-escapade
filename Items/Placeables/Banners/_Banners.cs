using EEMod.Tiles.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Placeables.Banners
{
    public abstract class BannerItem : EEItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = TileType<BannerTile>();
            Item.placeStyle = 0;
        }
    }

    public class ClamBanner : BannerItem { }

    public class LunaJellyBanner : BannerItem { }

    public class ManoWarBanner : BannerItem { }

    public class SeaSlugBanner : BannerItem { }

    public class ToxicPufferBanner : BannerItem { }

    public class GiantSquidBanner : BannerItem { }

    public class SmallClamBanner : BannerItem { }

    public class SeahorseBanner : BannerItem { }

    public class CoconutCrabBanner : BannerItem { }

    public class CoconutSpiderBanner : BannerItem { }
}
using EEMod.Tiles.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Placeables.Banners
{
    public abstract class BannerItem : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.createTile = TileType<BannerTile>();
            item.placeStyle = 0;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Banners
{
    public class BannerTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;
            DisableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 18;
            string item;
            switch (style)
            {
                case 0:
                    item = "ClamBanner";
                    break;

                case 1:
                    item = "LunaJellyBanner";
                    break;

                case 2:
                    item = "ManoWarBanner";
                    break;

                case 3:
                    item = "SeaSlugBanner";
                    break;

                case 4:
                    item = "ToxicPufferBanner";
                    break;

                case 5:
                    item = "GiantSquidBanner";
                    break;

                case 6:
                    item = "SmallClamBanner";
                    break;

                case 7:
                    item = "SeahorseBanner";
                    break;

                case 8:
                    item = "CoconutSpiderBanner";
                    break;

                case 9:
                    item = "CoconutCrabBanner";
                    break;

                default:
                    return;
            }
            Item.NewItem(i * 16, j * 16, 16, 48, Mod.Find<ModItem>(item).Type);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player player = Main.LocalPlayer;
                int style = Framing.GetTileSafely(i, j).frameX / 18;
                string type;
                switch (style)
                {
                    case 0:
                        type = "Clam";
                        break;

                    case 1:
                        type = "LunaJelly";
                        break;

                    case 2:
                        type = "ManoWar";
                        break;

                    case 3:
                        type = "SeaSlug";
                        break;

                    case 4:
                        type = "ToxicPuffer";
                        break;

                    case 5:
                        type = "GiantSquid";
                        break;

                    case 6:
                        type = "SmallClam";
                        break;

                    case 7:
                        type = "Seahorse";
                        break;

                    case 8:
                        type = "CoconutSpider";
                        break;

                    case 9:
                        type = "CoconutCrab";
                        break;

                    default:
                        return;
                }
                //player.Banner[Mod.Find<ModNPC>(type).Type] = true;
                //player.hasBanner = true;
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}
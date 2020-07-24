using EEMod.Items.Banners;
using EEMod.NPCs.CoralReefs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Banners
{
    public class BannerTile : ModTile
	{
		public override void SetDefaults()
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
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			int itemtype;
            switch (style)
            {
				case 0: itemtype = ModContent.ItemType<ClamBanner>(); break;
				case 1: itemtype = ModContent.ItemType<LunaJellyBanner>(); break;
				case 2: itemtype = ModContent.ItemType<ManoWarBanner>(); break;
				case 3: itemtype = ModContent.ItemType<SeaSlugBanner>(); break;
				case 4: itemtype = ModContent.ItemType<ToxicPufferBanner>(); break;
				case 5: itemtype = ModContent.ItemType<GiantSquidBanner>(); break;
				case 6: itemtype = ModContent.ItemType<SmallClamBanner>(); break;
                default:
					return;
            }
			Item.NewItem(i * 16, j * 16, 16, 48, itemtype);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				int type;
                switch (style)
                {
					case 0: type = ModContent.NPCType<Clam>(); break;
					case 1: type = ModContent.NPCType<LunaJelly>(); break;
					case 2: type = ModContent.NPCType<ManoWar>(); break;
					case 3: type = ModContent.NPCType<SeaSlug>(); break;
					case 4: type = ModContent.NPCType<ToxicPuffer>(); break;
					case 5: type = ModContent.NPCType<GiantSquid>(); break;
					default:
						return;
                }
				player.NPCBannerBuff[type] = true;
				player.hasBanner = true;
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

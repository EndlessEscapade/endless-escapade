using EEMod.Items.Materials;
using EEMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Placeables.Furniture;
using EEMod.EEWorld;
using EEMod.UI.States;
using EEMod;

namespace EEMod.Tiles.Furniture
{
    public class LunaJellyBottleTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Luna Jelly Bottle");
            AddMapEntry(new Color(255, 168, 28), name);
            dustType = 11;
            disableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<LunaJellyBottle>());
        }

        private int frameCounter;
        private int frame;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            frameCounter++;
            if (frameCounter >= 12) 
            { 
                frame++; 
                frameCounter = 0; 
            }

            if (frame >= 2) frame = 0;

            Tile tile = Framing.GetTileSafely(i, j);

            Texture2D tex = mod.GetTexture("Tiles/Furniture/LunaJellyBottleTile");
            Main.spriteBatch.Draw(tex, new Rectangle((i * 16) - (int)Main.screenPosition.X + (int)Helpers.GetTileDrawZero().X, (j * 16) - (int)Main.screenPosition.Y + (int)Helpers.GetTileDrawZero().Y, 18, 18), new Rectangle(tile.frameX, tile.frameY + (frame * 54), 18, 18), Lighting.GetColor(i, j));

            return false;
        }
    }
}
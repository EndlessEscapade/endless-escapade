/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Foliage.ThermalVents
{
    public class ThermalVent6x6 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                16
            };
            animationFrameHeight = 96;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Thermal Vent");
            AddMapEntry(new Color(0, 100, 200), name);
            dustType = DustID.Dirt;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Tiles/Foliage/ThermalVents/ThermalVent6x6Vent"), new Vector2(i * 16, j * 16) - Main.screenPosition, new Rectangle(tile.frameX, tile.frameY + (frame2 * 96), 16, 16), Color.White);

            if (tile.frameX == 48 && tile.frameY == 32 && frame2 >= 4 && !sus)
            {
                int sussy = NPC.NewNPC(i * 16, j * 16, ModContent.NPCType<Amogus>());
                Main.npc[sussy].velocity = new Vector2(2, -5);
                sus = true;
            }
        }

        private int frame2;
        private bool sus = false;
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if(frameCounter >= 4 && frame < 6)
            {
                frame++;
                frameCounter = 0;
            }

            frameCounter++;

            frame2 = frame;
        }
    }
}*/
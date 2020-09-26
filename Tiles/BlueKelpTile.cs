using EEMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles
{
    public class BlueKelpTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileBlendAll[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = false;
            AddMapEntry(new Color(68, 89, 195));
            //Main.tileCut[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<Kelp>();
            soundStyle = SoundID.Grass;
            mineResist = 0f;
            minPick = 0;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 0, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<GemsandTile>(), ModContent.TileType<BlueKelpTile>(), ModContent.TileType<LightGemsandTile>() };
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.addTile(Type);
            animationFrameHeight = 18;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            if (WorldGen.InWorld(i, j))
            {
                Tile tile = Framing.GetTileSafely(i, j + 1);
                if (!tile.active()
                    || tile.type != ModContent.TileType<BlueKelpTile>()
                    && tile.type != ModContent.TileType<GemsandTile>()
                    && tile.type != ModContent.TileType<LightGemsandTile>()
                    && tile.type != ModContent.TileType<DarkGemsandTile>())
                {
                    WorldGen.KillTile(i, j);
                }
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j - 1);
            if (!tile.active() && Main.rand.Next(4) == 0)
            {
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<BlueKelpTile>());
                NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<BlueKelpTile>(), 0, 0, -1, -1);
            }
        }

        int b = Main.rand.Next(0, 9);
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 6)
            {
                b++;
                if (b >= 8)
                {
                    b = 0;
                }
                frame = b;
                frameCounter = 0;
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
        }
    }
}
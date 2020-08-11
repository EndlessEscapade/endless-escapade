using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using EEMod.Tiles;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Tiles
{
    public class KelpTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileBlendAll[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = false;
            AddMapEntry(new Color(68, 89, 195));
            Main.tileCut[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<Kelp>();
            soundStyle = 1;
            mineResist = 0f;
            minPick = 0;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 0, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<GemsandTile>(), ModContent.TileType<KelpTile>(), ModContent.TileType<LightGemsandTile>() };
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.addTile(Type);
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            if (WorldGen.InWorld(i, j))
            {
                Tile tile = Framing.GetTileSafely(i, j + 1);
                if (!tile.active()
                    || tile.type != ModContent.TileType<KelpTile>()
                    && tile.type != ModContent.TileType<GemsandTile>()
                    && tile.type != ModContent.TileType<LightGemsandTile>()
                    && tile.type != ModContent.TileType<DarkGemsandTile>())
                    WorldGen.KillTile(i, j);
            }
        }
        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j - 1);
            if (!tile.active() && Main.rand.Next(4) == 0)
            {
                WorldGen.PlaceObject(i, j - 1, ModContent.TileType<KelpTile>());
                NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<KelpTile>(), 0, 0, -1, -1);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            
            return true;
        }
    }
}

using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using EEMod.Tiles;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;

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
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorValidTiles = new int[] {ModContent.TileType<GemsandTile>(), ModContent.TileType<KelpTile>(), ModContent.TileType<LightGemsandTile>() };
            TileObjectData.newTile.AnchorTop = default;
            TileObjectData.addTile(Type);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

        }
    }
}

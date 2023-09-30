using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EndlessEscapade.Content.Tiles.Shipyard;

public class WoodFigurehead : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileBlockLight[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
        TileObjectData.newTile.Origin = Point16.Zero;
        TileObjectData.newTile.DrawYOffset = -2;
        TileObjectData.newTile.CoordinateHeights[0] = 18;
        TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(164, 102, 65));

        HitSound = SoundID.Dig;
        DustType = DustID.WoodFurniture;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}

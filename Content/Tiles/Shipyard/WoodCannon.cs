using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EndlessEscapade.Content.Tiles.Shipyard;

public class WoodCannon : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;        
        Main.tileBlockLight[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = Point16.Zero;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(47, 55, 69));

        MineResist = 1f;

        HitSound = SoundID.Dig;
        DustType = DustID.Stone;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) { num = fail ? 1 : 3; }
}

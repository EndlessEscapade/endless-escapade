using EndlessEscapade.Common.Tiles;
using EndlessEscapade.Content.Items.Reefs.Depths;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Depths;

public class DarkGemsandTile : CompositeTileBase
{
    public override int AtlasWidth { get; } = 3;

    public override int AtlasHeight { get; } = 3;

    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileID.Sets.Conversion.Sand[Type] = true;

        AddMapEntry(new Color(71, 106, 183));

        HitSound = SoundID.Dig;
        DustType = DustID.BlueMoss;

        ItemDrop = ModContent.ItemType<DarkGemsandItem>();
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}
using EndlessEscapade.Common.Tiles;
using EndlessEscapade.Content.Items.Reefs.Kelp;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Tiles.Reefs.Kelp;

public class KelpRockTile : CompositeTileBase
{
    public override int AtlasWidth { get; } = 3;

    public override int AtlasHeight { get; } = 3;

    public override void SetStaticDefaults() {
        Main.tileMergeDirt[Type] = false;
        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileID.Sets.Conversion.Stone[Type] = true;

        AddMapEntry(new Color(99, 136, 132));

        MineResist = 1f;

        HitSound = SoundID.Tink;
        DustType = DustID.JunglePlants;

        ItemDrop = ModContent.ItemType<KelpRockItem>();
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}
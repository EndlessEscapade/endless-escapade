using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Tiles.Furniture.Coral
{
    public class HangingCoralTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            TileObjectData.newSubTile.LavaDeath = false;
            TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Hanging Coral");
            AddMapEntry(new Color(30, 150, 200), name);
            dustType = DustID.Stone;
            adjTiles = new int[] { TileID.HangingLanterns };
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}

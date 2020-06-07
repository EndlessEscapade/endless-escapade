using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Tiles.Ores
{
    public class LythenOreTile : ModTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 410; // Metal Detector value
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 1100; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lythen");
            AddMapEntry(new Color(152, 171, 198), name);

            dustType = 84;
            drop = ModContent.ItemType<LythenOre>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 30;
        }

        public override bool CanExplode(int i, int j)
        {
            return NPC.downedBoss1;
        }
    }
}

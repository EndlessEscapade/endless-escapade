using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InteritosMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using InteritosMod.IntWorld;

namespace InteritosMod.Tiles.Ores
{
    public class DalantiniumOreTile : ModTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 410; // Metal Detector value
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 1100; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true; // lemme open some important files so you can acess them
            Main.tileBlockLight[Type] = true; // 

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Dalantinium");
            AddMapEntry(new Color(152, 171, 198), name);

            dustType = 84;
            drop = ModContent.ItemType<DalantiniumOre>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 60;
        }

        public override bool CanExplode(int i, int j)
        {
            return InteritosWorld.downedHydros;
        }
    }
}

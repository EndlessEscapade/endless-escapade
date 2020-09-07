using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture
{
    public class TrophiesTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            dustType = 7;
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Trophy");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int item = 0;
            switch (frameX / 54)
            {
                case 0:
                    item = ModContent.ItemType<HydrosTrophy>();
                    break;

                case 1:
                    item = ModContent.ItemType<OmenTrophy>();
                    break;

                case 2:
                    item = ModContent.ItemType<AkumoTrophy>();
                    break;

                case 3:
                    item = ModContent.ItemType<KrakenTrophy>();
                    break;

                case 4:
                    item = ModContent.ItemType<TalosTrophy>();
                    break;

                case 5:
                    item = ModContent.ItemType<CoralGolemTrophy>();
                    break;
            }
            Item.NewItem(i * 16, j * 16, 48, 48, item);
        }
    }
}
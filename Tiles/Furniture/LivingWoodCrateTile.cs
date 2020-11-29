using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles.Furniture
{
    public class LivingWoodCrateTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Living Wood Crate");
            AddMapEntry(new Color(255, 168, 28), name);
            dustType = 11;
            disableSmartCursor = true;
        }
    }
}
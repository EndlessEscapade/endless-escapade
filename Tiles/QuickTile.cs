using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Tiles
{
    public abstract class QuickTile : ModTile
    {
        public abstract int Width
        {
            get;
            set;
        }
        public abstract int Height
        {
            get;
            set;
        }
        public abstract bool isSolid { get; set; }
        public virtual string NameOfTile => GetType().Name;
        public virtual int Padding => 2;
        public virtual Color MapColour => new Color(60, 60, 60);
        public virtual bool isHanging => false;
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileSolid[Type] = isSolid;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.Width = Width;
            TileObjectData.newTile.Height = Height;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = isHanging ? new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1) : AnchorData.Empty;
            TileObjectData.newTile.AnchorBottom = isHanging ? AnchorData.Empty : new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            int[] coordHeights = new int[Height];
            for (int i = 0; i < Height; i++)
            {
                coordHeights[i] = 16;
            }
            TileObjectData.newTile.CoordinateHeights = coordHeights;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = Padding;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(NameOfTile);
            AddMapEntry(MapColour, name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
        }
    }
}
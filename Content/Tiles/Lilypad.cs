using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EndlessEscapade.Content.Tiles;

public class Lilypad : ModTile
{
    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileCut[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.UsesCustomCanPlace = true;

        TileObjectData.newTile.CoordinatePadding = 2;
        
        TileObjectData.newTile.Width = 1;
        TileObjectData.newTile.CoordinateWidth = 26;

        TileObjectData.newTile.Height = 1;
        TileObjectData.newTile.CoordinateHeights = new[] {
            30
        };
        
        TileObjectData.newTile.DrawYOffset = 2;

        TileObjectData.newTile.WaterDeath = false;
        TileObjectData.newTile.LavaDeath = true;
        
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        
        TileObjectData.newTile.Origin = Point16.Zero;

        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Height, 0);
        TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.EmptyTile, TileObjectData.newTile.Height, 0);
        
        TileObjectData.addTile(Type);
        
        AddMapEntry(new Color(135, 196, 26));

        DustType = DustID.Grass;
        HitSound = SoundID.Grass;
    }
}

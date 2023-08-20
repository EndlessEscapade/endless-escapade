using EndlessEscapade.Common.Systems.Shipyard;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EndlessEscapade.Content.Tiles.Shipyard;

public class Wheel : ModTile
{
    public override string HighlightTexture => Texture + "_Highlight";

    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        TileID.Sets.HasOutlines[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
        TileObjectData.newTile.Height = 3;

        for (var i = 0; i < TileObjectData.newTile.Height; i++) {
            TileObjectData.newTile.CoordinateHeights[i] = 16;
        }

        TileObjectData.newTile.Origin = Point16.Zero;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.Direction = TileObjectDirection.None;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(119, 71, 59));

        HitSound = SoundID.Dig;
        DustType = DustID.WoodFurniture;
    }

    public override bool RightClick(int i, int j) {
        return true;
    }

    public override void MouseOver(int i, int j) {
        var player = Main.LocalPlayer;
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ModContent.ItemType<Items.Shipyard.Wheel>();
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
        return true;
    }
}

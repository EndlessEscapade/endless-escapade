using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding.Biomes;

public sealed class Iceboat : MicroBiome
{
    public override bool Place(Point origin, StructureMap structures) {
        var mod = EndlessEscapade.Instance;

        var ruinsDims = Point16.Zero;
        var iceboatDims = Point16.Zero;

        if (!Generator.GetDimensions("Content/Structures/IceboatRuins", mod, ref ruinsDims) || !Generator.GetDimensions("Content/Structures/Iceboat", mod, ref iceboatDims)) {
            return false;
        }

        var offset = WorldGen.genRand.Next(150, 200);

        var ruinsOrigin = origin - new Point(ruinsDims.X / 2, 0);
        var iceboatOrigin = origin - new Point(iceboatDims.X / 2, iceboatDims.Y / 2 - offset);

        if (!CanPlaceStructure(ruinsOrigin, ruinsDims, structures) || !CanPlaceStructure(iceboatOrigin, iceboatDims, structures)) {
            return false;
        }

        var leftAdjacentTile = Framing.GetTileSafely(ruinsOrigin.X - 1, ruinsOrigin.Y - 1);
        var rightAdjacentTile = Framing.GetTileSafely(ruinsOrigin.X + ruinsDims.X, ruinsOrigin.Y - 1);

        if (leftAdjacentTile.HasTile || rightAdjacentTile.HasTile) {
            return false;
        }

        if (!Generator.GenerateStructure("Content/Structures/IceboatRuins", new Point16(ruinsOrigin), mod) || !Generator.GenerateStructure("Content/Structures/Iceboat", new Point16(iceboatOrigin), mod)) {
            return false;
        }

        var shaftShapeData = new ShapeData();

        WorldUtils.Gen(new Point(iceboatOrigin.X + iceboatDims.X / 2, ruinsOrigin.Y + ruinsDims.Y), new Shapes.Rectangle(1, iceboatOrigin.Y - ruinsOrigin.Y - ruinsDims.Y), Actions.Chain(new Modifiers.Blotches(2, 0.2), new Actions.ClearTile().Output(shaftShapeData), new Modifiers.Expand(1), new Modifiers.OnlyTiles(TileID.SnowBlock), new Actions.SetTile(TileID.IceBlock).Output(shaftShapeData), new Actions.PlaceWall(WallID.IceUnsafe)));
        WorldUtils.Gen(new Point(iceboatOrigin.X + iceboatDims.X / 2, ruinsOrigin.Y + ruinsDims.Y), new ModShapes.All(shaftShapeData), new Actions.SetFrames(true));

        structures.AddProtectedStructure(new Rectangle(ruinsOrigin.X, ruinsOrigin.Y, ruinsDims.X, ruinsDims.Y));
        structures.AddProtectedStructure(new Rectangle(iceboatOrigin.X, iceboatOrigin.Y, iceboatDims.X, iceboatDims.Y));

        return true;
    }

    private bool CanPlaceStructure(Point origin, Point16 dims, StructureMap structures) {
        if (!structures.CanPlace(new Rectangle(origin.X, origin.Y, dims.X, dims.Y))) {
            return false;
        }

        var areaPercentage = dims.X * dims.Y / 2f;
        var tileLookup = new Dictionary<ushort, int>();

        WorldUtils.Gen(origin, new Shapes.Rectangle(dims.X, dims.Y), new Actions.TileScanner(TileID.SnowBlock, TileID.IceBlock).Output(tileLookup));

        return tileLookup[TileID.SnowBlock] + tileLookup[TileID.IceBlock] >= areaPercentage;
    }
}

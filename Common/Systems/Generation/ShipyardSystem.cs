using EndlessEscapade.Common.Systems.Generation.Actions;
using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Content.Tiles.Shipyard;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.Generation;

public sealed class ShipyardSystem : ModSystem
{
    public static int X { get; private set; }
    public static int Y { get; private set; }

    public static bool Repaired { get; private set; }

    public override void SaveWorldData(TagCompound tag) {
        tag[nameof(X)] = X;
        tag[nameof(Y)] = Y;
        tag[nameof(Repaired)] = Repaired;
    }

    public override void LoadWorldData(TagCompound tag) {
        X = tag.GetInt(nameof(X));
        Y = tag.GetInt(nameof(Y));
        Repaired = tag.GetBool(nameof(Repaired));
    }

    public override void ClearWorld() {
        X = 0;
        Y = 0;
        Repaired = false;
    }

    public override void Load() {
        Sailor.OnBoatRepair += GenerateDefaultBoat;
    }

    public override void PostWorldGen() {
        var foundOcean = false;
        var foundBeach = false;

        var x = 0;
        var y = (int)(Main.worldSurface * 0.35f);

        while (!foundOcean) {
            var tile = Framing.GetTileSafely(x, y);

            if (tile.LiquidAmount >= 255 && tile.LiquidType == LiquidID.Water) {
                foundOcean = true;
                break;
            }

            y++;
        }

        while (!foundBeach) {
            if (WorldGen.SolidTile(x, y) && WorldGen.TileType(x, y) == TileID.Sand) {
                foundBeach = true;
                break;
            }

            x++;
        }

        const int sailboatDistance = 100;

        GenerateShipyard(x, y);
        GenerateBrokenBoat(x - sailboatDistance, y);
    }

    // TODO: Turn into a single attachment method via enumerator, or something similar.
    public static bool PlaceWheel<T>() where T : ModTile {
        return WorldGen.PlaceObject(X + 12, Y + 25, ModContent.TileType<T>());
    }

    public static bool PlaceCannon<T>() where T : ModTile {
        return WorldGen.PlaceObject(X + 31, Y + 26, ModContent.TileType<T>());
    }

    public static bool PlaceFigurehead<T>() where T : ModTile {
        return WorldGen.PlaceObject(X + 4, Y + 30, ModContent.TileType<T>());
    }

    private static void GenerateShipyard(int x, int y) {
        const string path = "Assets/Structures/Shipyard";

        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions(path, mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y - dims.Y / 3);
        var origin = new Point16(x, y) - offset;

        if (!Generator.GenerateStructure(path, origin, mod)) {
            return;
        }

        ExtendPillar(origin.X + 4, origin.Y + 39);
        ExtendPillar(origin.X + 5, origin.Y + 39);

        ExtendPillar(origin.X + 20, origin.Y + 39);
        ExtendPillar(origin.X + 21, origin.Y + 39);

        ExtendPillar(origin.X + 36, origin.Y + 39);
        ExtendPillar(origin.X + 37, origin.Y + 39);

        const int roomOffsetX = 60;
        const int roomOffsetY = 10;

        var sailorX = (int)((origin.X + roomOffsetX) * 16f);
        var sailorY = (int)((origin.Y + roomOffsetY) * 16f);

        var index = NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
        var sailor = Main.npc[index];

        sailor.UpdateHomeTileState(false, (int)(sailorX / 16f), (int)(sailorY / 16f));
    }

    private static void ExtendPillar(int x, int y) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, TileID.LivingWood, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }

    private static void GenerateBrokenBoat(int x, int y) {
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/BrokenSailboat", mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var origin = new Point16(x, y) - offset;

        if (!Generator.GenerateStructure("Assets/Structures/BrokenSailboat", origin, mod)) {
            return;
        }

        X = origin.X;
        Y = origin.Y;

        NetMessage.SendData(MessageID.WorldData);
    }

    private static void GenerateDefaultBoat() {
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/BrokenSailboat", mod, ref dims)) {
            return;
        }

        WorldUtils.Gen(
            new Point(X, Y),
            new Shapes.Rectangle(dims.X, dims.Y),
            Terraria.WorldBuilding.Actions.Chain(
                new Terraria.WorldBuilding.Actions.ClearTile(),
                new Terraria.WorldBuilding.Actions.ClearWall()
            )
        );

        // Shifts the position back to the original origin.
        X += dims.X / 2;
        Y += dims.Y / 2;

        if (!Generator.GetDimensions("Assets/Structures/Sailboat", mod, ref dims)) {
            return;
        }

        // Shifts the position to the new origin, which is approximately 85% upwards from the original origin.
        X -= dims.X / 2;
        Y -= dims.Y - dims.Y / 7;

        Repaired = true;

        NetMessage.SendData(MessageID.WorldData);

        if (!Generator.GenerateStructure("Assets/Structures/Sailboat", new Point16(X, Y), mod)) {
            return;
        }

        WorldUtils.Gen(new Point(X, Y), new Shapes.Rectangle(dims.X, dims.Y), new Reframe());
    }
}

using EndlessEscapade.Content.NPCs.Shipyard;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.WorldBuilding;

public sealed class ShipyardSystem : ModSystem
{
    public const int SailboatDistance = 100;

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

        GenerateShipyard(x, y);
        GenerateBrokenBoat(x - SailboatDistance, y);
    }

    private void GenerateShipyard(int x, int y) {
        const string ShipyardStructurePath = "Assets/Structures/Shipyard";

        var dims = Point16.Zero;

        if (!Generator.GetDimensions(ShipyardStructurePath, Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y - dims.Y / 3);
        var origin = new Point16(x, y) - offset;

        if (!Generator.GenerateStructure(ShipyardStructurePath, origin, Mod)) {
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

    private void ExtendPillar(int x, int y) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, TileID.LivingWood, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }

    private void GenerateBrokenBoat(int x, int y) {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/BrokenSailboat", Mod, ref dims)) {
            return;
        }

        var offset = new Point16(dims.X / 2, dims.Y / 2);
        var origin = new Point16(x, y) - offset;

        if (!Generator.GenerateStructure("Assets/Structures/BrokenSailboat", origin, Mod)) {
            return;
        }

        NetMessage.SendData(MessageID.WorldData);
    }

    /*
    private void GenerateDefaultBoat() {
        var dims = Point16.Zero;

        if (!Generator.GetDimensions("Assets/Structures/BrokenSailboat", Mod, ref dims)) {
            return;
        }

        WorldUtils.Gen(
            new Point(X, Y),
            new Shapes.Rectangle(dims.X, dims.Y),
            Actions.Chain(
                new Actions.ClearTile(),
                new Actions.ClearWall()
            )
        );

        // Shifts the position back to the original origin.
        X += dims.X / 2;
        Y += dims.Y / 2;

        if (!Generator.GetDimensions("Assets/Structures/Sailboat", Mod, ref dims)) {
            return;
        }

        // Shifts the position to the new origin, which is approximately 85% upwards from the original.
        X -= dims.X / 2;
        Y -= dims.Y - dims.Y / 7;

        Repaired = true;

        NetMessage.SendData(MessageID.WorldData);

        if (!Generator.GenerateStructure("Assets/Structures/Sailboat", new Point16(X, Y), Mod)) {
            return;
        }

        WorldUtils.Gen(new Point(X, Y), new Shapes.Rectangle(dims.X, dims.Y), new Reframe());
    }
    */
}

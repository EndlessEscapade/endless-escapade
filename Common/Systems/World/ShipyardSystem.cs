using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Content.Tiles.Shipyard;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Systems.World;

public class ShipyardSystem : ModSystem
{
    private const int BoatWidth = 46;
    private const int BoatHeight = 37;

    public static int BoatX { get; private set; }
    public static int BoatY { get; private set; }

    public static bool BoatFixed { get; private set; }

    public override void SaveWorldData(TagCompound tag) {
        tag[nameof(BoatX)] = BoatX;
        tag[nameof(BoatY)] = BoatY;

        tag[nameof(BoatFixed)] = BoatFixed;
    }

    public override void LoadWorldData(TagCompound tag) {
        BoatX = tag.GetInt(nameof(BoatX));
        BoatY = tag.GetInt(nameof(BoatY));

        BoatFixed = tag.GetBool(nameof(BoatFixed));
    }

    public override void Load() {
        Sailor.OnBoatRepair += PrepareDefaultBoat;
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

        y--;

        while (!foundBeach) {
            var tile = Framing.GetTileSafely(x, y);

            if (WorldGen.SolidTile(tile) && tile.HasTile && tile.TileType == TileID.Sand) {
                foundBeach = true;
                break;
            }

            x++;
        }

        const int sailboatDistance = 100;

        GenerateShipyard(x, y);
        GenerateBrokenBoat(x - sailboatDistance, y);
    }

    public static void GenerateHull(string path) {
        var mod = EndlessEscapade.Instance;

        var offset = GetAttachmentOffset(AttachmentType.Hull);
        var origin = new Point16(BoatX, BoatY) + offset;

        Generator.GenerateStructure(path, origin, mod);
    }

    public static void GenerateSmallSail(string path) {
        var mod = EndlessEscapade.Instance;

        var offset = GetAttachmentOffset(AttachmentType.SmallSail);
        var origin = new Point16(BoatX, BoatY) + offset;

        Generator.GenerateStructure(path, origin, mod);
    }

    public static void GenerateLargeSail(string path) {
        var mod = EndlessEscapade.Instance;

        var offset = GetAttachmentOffset(AttachmentType.LargeSail);
        var origin = new Point16(BoatX, BoatY) + offset;

        Generator.GenerateStructure(path, origin, mod);
    }

    public static void GenerateCannon(int type) {
        var mod = EndlessEscapade.Instance;

        var offset = GetAttachmentOffset(AttachmentType.Cannon);
        var origin = new Point16(BoatX, BoatY) + offset;

        WorldGen.PlaceTile(origin.X, origin.Y, type, true, true);
    }

    public static void GenerateWheel(int type) {
        var mod = EndlessEscapade.Instance;

        var offset = GetAttachmentOffset(AttachmentType.Wheel);
        var origin = new Point16(BoatX, BoatY) + offset;

        WorldGen.PlaceTile(origin.X, origin.Y, type, true, true);
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

        NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
    }

    private static void ExtendPillar(int x, int y) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, TileID.LivingWood, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }

    private static void GenerateBrokenBoat(int x, int y) {
        const string path = "Assets/Structures/Boats/Default/Broken";

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

        BoatX = origin.X;
        BoatY = origin.Y;
    }

    private static void PrepareDefaultBoat() {
        const string path = "Assets/Structures/Boats/Default/Broken";

        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions(path, mod, ref dims)) {
            return;
        }

        ClearArea(BoatX, BoatY, dims.X + 1, dims.Y + 1);

        BoatX += dims.X / 2;
        BoatY += dims.Y / 2;

        BoatX -= BoatWidth / 2;
        BoatY -= BoatHeight - BoatHeight / 3;

        BoatFixed = true;

        NetMessage.SendData(MessageID.WorldData);
    }

    private static void GenerateDefaultBoat() {
        var mod = EndlessEscapade.Instance;

        GenerateHull("Assets/Structures/Boats/Default/Hull");

        GenerateCannon(ModContent.TileType<Cannon>());
        GenerateWheel(ModContent.TileType<Wheel>());

        GenerateSmallSail("Assets/Structures/Boats/Default/SmallSail");
        GenerateLargeSail("Assets/Structures/Boats/Default/LargeSail");

        ReframeArea(BoatX, BoatY, BoatWidth, BoatHeight);
    }

    private static void ClearArea(int x, int y, int width, int height) {
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                if (!WorldGen.InWorld(x + i, y + j)) {
                    continue;
                }

                var tile = Framing.GetTileSafely(x + i, y + j);

                var oldType = tile.LiquidType;
                var oldAmount = tile.LiquidAmount;

                tile.ClearEverything();

                tile.LiquidType = oldType;
                tile.LiquidAmount = oldAmount;
            }
        }
    }

    private static void ReframeArea(int x, int y, int width, int height) {
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                if (!WorldGen.InWorld(x + i, y + j)) {
                    continue;
                }

                WorldGen.Reframe(x + i, y + j, true);
            }
        }
    }

    private static Point16 GetAttachmentOffset(AttachmentType type) {
        return type switch {
            AttachmentType.Hull => new Point16(0, 17),
            AttachmentType.Wheel => new Point16(12, 24),
            AttachmentType.Cannon => new Point16(31, 25),
            AttachmentType.SmallSail => new Point16(7, 9),
            AttachmentType.LargeSail => new Point16(21, 0)
        };
    }

    private enum AttachmentType
    {
        Hull,
        Wheel,
        Cannon,
        SmallSail,
        LargeSail,
        Figurehead
    }
}

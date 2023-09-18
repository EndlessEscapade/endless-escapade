using EndlessEscapade.Common.Systems.Shipyard.Attachments;
using EndlessEscapade.Common.Systems.World.Actions;
using EndlessEscapade.Content.NPCs.Shipyard;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Cannon = EndlessEscapade.Content.Tiles.Shipyard.Cannon;
using Wheel = EndlessEscapade.Content.Tiles.Shipyard.Wheel;

namespace EndlessEscapade.Common.Systems.Shipyard;

public class ShipyardSystem : ModSystem
{
    public const int ShipWidth = 46;
    public const int ShipHeight = 37;

    public static int ShipX { get; private set; }
    public static int ShipY { get; private set; }

    public static bool ShipFixed { get; private set; }

    public override void SaveWorldData(TagCompound tag) {
        tag[nameof(ShipX)] = ShipX;
        tag[nameof(ShipY)] = ShipY;
        tag[nameof(ShipFixed)] = ShipFixed;
    }

    public override void LoadWorldData(TagCompound tag) {
        ShipX = tag.GetInt(nameof(ShipX));
        ShipY = tag.GetInt(nameof(ShipY));
        ShipFixed = tag.GetBool(nameof(ShipFixed));
    }

    public override void ClearWorld() {
        ShipX = 0;
        ShipY = 0;
        ShipFixed = false;
    }

    public override void Load() { Sailor.OnBoatRepair += GenerateDefaultBoat; }

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

    public static bool GenerateAttachment<T>(T attachment) where T : IAttachment { return attachment.Generate(ShipX, ShipY); }

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

        ShipX = origin.X;
        ShipY = origin.Y;

        NetMessage.SendData(MessageID.WorldData);
    }

    private static void GenerateDefaultBoat() {
        const string path = "Assets/Structures/Boats/Default/Broken";

        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;

        if (!Generator.GetDimensions(path, mod, ref dims)) {
            return;
        }

        WorldUtils.Gen(
            new Point(ShipX, ShipY),
            new Shapes.Rectangle(ShipWidth, ShipHeight),
            Actions.Chain(
                new Actions.ClearTile(),
                new Actions.ClearWall()
            )
        );

        ShipX += dims.X / 2;
        ShipY += dims.Y / 2;

        ShipX -= ShipWidth / 2;
        ShipY -= ShipHeight - ShipHeight / 3;

        ShipFixed = true;

        GenerateAttachment(new Hull("Assets/Structures/Boats/Default/Hull"));
        GenerateAttachment(new SmallSail("Assets/Structures/Boats/Default/SmallSail"));
        GenerateAttachment(new LargeSail("Assets/Structures/Boats/Default/LargeSail"));
        GenerateAttachment(new Attachments.Cannon(ModContent.TileType<Cannon>()));
        GenerateAttachment(new Attachments.Wheel(ModContent.TileType<Wheel>()));

        NetMessage.SendData(MessageID.WorldData);

        WorldUtils.Gen(new Point(ShipX, ShipY), new Shapes.Rectangle(ShipWidth, ShipHeight), new Reframe());
    }
}

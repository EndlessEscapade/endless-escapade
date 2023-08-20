using System.IO;
using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.World;

public class ShipyardSystem : ModSystem
{
    private const int BoatWidth = 46;
    private const int BoatHeight = 37;
    
    public static bool BoatFixed { get; private set; }
    
    public static int BoatX { get; private set; }
    public static int BoatY { get; private set; }
    
    public override void NetSend(BinaryWriter writer) {
        writer.Write(BoatFixed);
        
        writer.Write(BoatX);
        writer.Write(BoatY);
    }

    public override void NetReceive(BinaryReader reader) {
        BoatFixed = reader.ReadBoolean();
        
        BoatX = reader.ReadInt32();
        BoatY = reader.ReadInt32();
    }

    public override void Load() {
        Sailor.OnBoatRepair += Recalculate;
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

    private static void GenerateShipyard(int x, int y) {
        void ExtendPillar(int xOffset, int yOffset) {
            var pillarX = x + xOffset;
            var pillarY = y + yOffset;

            while (WorldGen.InWorld(pillarX, pillarY) && !WorldGen.SolidTile(pillarX, pillarY)) {
                WorldGen.PlaceTile(pillarX, pillarY, TileID.LivingWood, true, true);
                WorldGen.SlopeTile(pillarX, pillarY);

                pillarY++;
            }
        }
        
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
        
        ExtendPillar(4 - offset.X, 39 - offset.Y);
        ExtendPillar(4 - offset.X + 1, 39 - offset.Y);

        ExtendPillar(20 - offset.X, 39 - offset.Y);
        ExtendPillar(20 - offset.X + 1, 39 - offset.Y);

        ExtendPillar(36 - offset.X, 39 - offset.Y);
        ExtendPillar(36 - offset.X + 1, 39 - offset.Y);

        const int roomOffsetX = 60;
        const int roomOffsetY = 10;

        var sailorX = (int)((origin.X + roomOffsetX) * 16f);
        var sailorY = (int)((origin.Y + roomOffsetY) * 16f);

        NPC.NewNPC(new EntitySource_WorldGen(), sailorX, sailorY, ModContent.NPCType<Sailor>());
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
    
    private static void Recalculate() {
        const string path = "Assets/Structures/Boats/Default/Broken";
        
        var mod = EndlessEscapade.Instance;
        var dims = Point16.Zero;
        
        if (!Generator.GetDimensions(path, mod, ref dims)) {
            return;
        }

        ClearArea(BoatX - 2, BoatY - 2, dims.X + 2, dims.Y + 2);
    }

    private static void ClearArea(int x, int y, int width, int height) {
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                Framing.GetTileSafely(x + i, y + j).ClearEverything();
            }
        }
    }
    
    private static void ReframeArea(int x, int y, int width, int height) {
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                WorldGen.Reframe(x + i, y + j, true);
            }
        }
    }

    private static Point16 GetAttachmentOffset(AttachmentType type) {
        return type switch {
            AttachmentType.Hull => new Point16(0, 17),
            AttachmentType.Wheel => new Point16(13, 26),
            AttachmentType.Cannon => new Point16(32, 26),
            AttachmentType.SailSmall => new Point16(7, 9),
            AttachmentType.SailLarge => new Point16(21, 0)
        };
    }
    
    public enum AttachmentType
    {
        Hull,
        Wheel,
        Cannon,
        SailSmall,
        SailLarge,
        Figurehead
    }
}

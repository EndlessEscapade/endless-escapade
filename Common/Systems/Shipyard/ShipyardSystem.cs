using System;
using System.Collections.Generic;
using EndlessEscapade.Common.Systems.Shipyard.Attachments;
using EndlessEscapade.Content.NPCs.Shipyard;
using EndlessEscapade.Utilities;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Systems.Shipyard;

public class ShipyardSystem : ModSystem
{
    public const int BoatWidth = 46;
    public const int BoatHeight = 37;

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

    public static bool GenerateAttachment<T>(T attachment) where T : IAttachment {
        return attachment.Generate(BoatX, BoatY);
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

        WorldGenUtils.ClearArea(BoatX, BoatY, dims.X + 1, dims.Y + 1, true);

        BoatX += dims.X / 2;
        BoatY += dims.Y / 2;

        BoatX -= BoatWidth / 2;
        BoatY -= BoatHeight - BoatHeight / 3;

        BoatFixed = true;

        NetMessage.SendData(MessageID.WorldData);
    }

    private static void GenerateDefaultBoat() {
        var mod = EndlessEscapade.Instance;

        var validTiles = new ushort[] { TileID.Platforms, TileID.WoodBlock, TileID.LivingWood };
        var validWalls = new ushort[] { WallID.Wood, WallID.LivingWood, WallID.Sail };

        GenerateAttachment(new Hull("Assets/Structures/Boats/Default/Hull", validTiles, validWalls));
        GenerateAttachment(new SmallSail("Assets/Structures/Boats/Default/SmallSail", validTiles, validWalls));
        GenerateAttachment(new LargeSail("Assets/Structures/Boats/Default/LargeSail", validTiles, validWalls));
        
        GenerateAttachment(new Cannon(ModContent.TileType<Content.Tiles.Shipyard.Cannon>()));
        GenerateAttachment(new Wheel(ModContent.TileType<Content.Tiles.Shipyard.Wheel>()));

        WorldGenUtils.ReframeArea(BoatX, BoatY, BoatWidth, BoatHeight);
    }
}

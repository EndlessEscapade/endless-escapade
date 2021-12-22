using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using EEMod.ID;
using EEMod.Tiles;
using EEMod.VerletIntegration;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.WorldBuilding;
using System.Diagnostics;

namespace EEMod.Systems.Subworlds.EESubworlds
{
    public class GoblinFort : Subworld
    {
        public override Point Dimensions => new Point(600, 600);

        public override Point SpawnTile => new Point(200, 100);

        public override string Name => "Suk-Mah Outpost";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            var rand = WorldGen.genRand;

            EEMod.progressMessage = "Generating Goblin Fort";

            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            FillRegion(400, 275, new Vector2(0, 325), TileID.Stone);

            FillRegion(100, 50, new Vector2(0, 275), TileID.Sand);
            FillRegion(400, 50, new Vector2(100, 275), TileID.Dirt);
            FillRegion(100, 50, new Vector2(500, 275), TileID.Sand);

            Main.worldSurface = 300;

            EEMod.progressMessage = "Successful!";
            //EEMod.isSaving = false;

            Main.spawnTileX = SpawnTile.X;
            Main.spawnTileY = SpawnTile.Y;

            EEMod.progressMessage = null;
        }

        internal override void PlayerUpdate(Player player)
        {
           
        }
    }
}
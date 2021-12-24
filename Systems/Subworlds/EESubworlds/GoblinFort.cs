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
        public override Point Dimensions => new Point(600, 800);

        public override Point SpawnTile => new Point(200, 100);

        public override int surfaceLevel => 500;

        public override string Name => "Suk-Mah Outpost";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            var rand = WorldGen.genRand;

            EEMod.progressMessage = "Generating Goblin Fort";

            Main.worldSurface = 400;

            //

            FillRegion(600, 475, new Vector2(0, 325), TileID.Stone);

            for(int i = 0; i < 100; i++)
            {
                for(int j = 0; j < 50; j++)
                {
                    if(j < (((i / 15f) * (i / 15f)) + 10f))
                    {
                        WorldGen.PlaceTile((i) + 0, (50 - j) + 274, TileID.Sand);
                    }
                }
            }

            //FillRegion(100, 50, new Vector2(0, 275), TileID.Sand);
            FillRegion(400, 50, new Vector2(100, 275), TileID.Dirt);
            //FillRegion(100, 50, new Vector2(500, 275), TileID.Sand);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (j < (((i / 15f) * (i / 15f)) + 10f))
                    {
                        WorldGen.PlaceTile((599 - i) + 0, (50 - j) + 274, TileID.Sand);
                    }
                }
            }

            FillRegionWithWater(600, 50, new Vector2(0, 280));

            //

            EEMod.progressMessage = null;
        }

        internal override void PlayerUpdate(Player player)
        {
           
        }
    }
}
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.Events;
using EEMod.Tiles.Ores;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using EEMod.EEWorld;
using EEMod.Projectiles;

namespace EEMod.EEWorld
{
    public partial class EEWorld : ModWorld
    {
        public static bool GenkaiMode;

        public static bool downedGallagar;
        public static bool downedForerunner;
        public static bool downedSoS;
        public static bool downedFlare;
        public static bool downedAssimilator;
        public static bool downedAkumo;
        public static bool downedHydros;
        public static bool downedStagrel;
        public static bool downedBeheader;

        private static List<Point> BiomeCenters;
        public static int CoralReefsTiles = 0;
        public static Vector2 yes;
        public static Vector2 ree;
        public static IList<Vector2> EntracesPosses = new List<Vector2>();

        public override void Initialize()
        {
            eocFlag = NPC.downedBoss1;
            ree = new Vector2(100, TileCheck(100) - 22);
            if (EntracesPosses.Count > 0)
                yes = EntracesPosses[0];
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Interitos Mod Ores", EEModOres));
            }
            int MicroBiomes = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
            /*if (LivingTreesIndex != -1)
            {
                tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress)
                {
                    progress.Message = "Generating structures";
                    for (int l = 0; l < 30; l++)
                    {
                        int posX = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int posY = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, Main.maxTilesY);
                        PlaceRuins(posX, posY, ruinsShape);
                    }
                }));
            }*/
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteVector2(ree);
            writer.WriteVector2(yes);
        }
        public override void NetReceive(BinaryReader reader)
        {
            ree = reader.ReadVector2();
            yes = reader.ReadVector2();
        }
        public override void PostWorldGen()
        {
            DoAndAssignShrineValues();
            DoAndAssignShipValues();
        }
        public override void PostUpdate()
        {
            if (NPC.downedBoss1)
            {
                if (!eocFlag)
                {
                    eocFlag = true;
                    Main.NewText("You hear a strong wind erupting from the desert...", 228, 171, 72);
                    StartSandstorm();
                }
            }
            int lastNoOfShipTiles = missingShipTiles.Count;
            ShipComplete();
            if (missingShipTiles.Count != lastNoOfShipTiles)
            {
                for(int i = 0; i<200; i++)
                {
                  if(Main.projectile[i].type == ModContent.ProjectileType<WhiteBlock>())
                    {
                        Main.projectile[i].Kill();
                    }
                }
                foreach (Vector2 tile in missingShipTiles)
                {
                    Projectile.NewProjectile(tile * 16 + new Vector2(8,8) + new Vector2(-3*16,-6*16), Vector2.Zero, ModContent.ProjectileType<WhiteBlock>(), 0, 0);
                }
            }
        }
        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("EntracesPosses"))
            {
                EntracesPosses = tag.GetList<Vector2>("EntracesPosses");
            }
            if (tag.ContainsKey("yes") && tag.ContainsKey("yes").GetType().Name == "Vector2")
            {
                yes = tag.Get<Vector2>("yes");
            }
            if (tag.ContainsKey("ree") && tag.ContainsKey("ree").GetType().Name == "Vector2")
            {
                ree = tag.Get<Vector2>("ree");
            }
            var downed = new List<string>();
            if (eocFlag) downed.Add("eocFlag");

            IList<string> flags = tag.GetList<string>("boolFlags");

            // Game modes
            GenkaiMode = flags.Contains("GenkaiMode");

            // Downed bosses
            downedGallagar = flags.Contains("downedGallagar");
            downedForerunner = flags.Contains("downedForerunner");
            downedSoS = flags.Contains("downedSoS");
            downedFlare = flags.Contains("downedFlare");
            downedAssimilator = flags.Contains("downedAssimilator");
            downedAkumo = flags.Contains("downedAkumo");
            downedHydros = flags.Contains("downedHydros");
            downedStagrel = flags.Contains("downedStagrel");
            downedBeheader = flags.Contains("downedBeheader");
        }
        public override TagCompound Save()
        {
            return new TagCompound {
            {"EntracesPosses",EntracesPosses},{"yes", yes },{"ree", ree }
        };
            /*List<string> boolflags = new List<string>();

            // Game modes
            if (GenkaiMode)
                boolflags.Add("GenkaiMode");

            // Downed bosses
            if (downedGallagar)
                boolflags.Add("downedGallagar");
            if (downedForerunner)
                boolflags.Add("downedForerunner");
            if (downedSoS)
                boolflags.Add("downedSoS");
            if (downedFlare)
                boolflags.Add("downedFlare");
            if (downedAssimilator)
                boolflags.Add("downedAssimilator");
            if (downedAkumo)
                boolflags.Add("downedAkumo");
            if (downedHydros)
                boolflags.Add("downedHydros");
            if (downedStagrel)
                boolflags.Add("downedStagrel");
            if (downedBeheader)
                boolflags.Add("downedBeheader");


            return new TagCompound
            {
                ["SaveVersion"] = new Version(0, 3, 0, 0).ToString(),
                ["boolFlags"] = boolflags
            };*/
        }
        public override void ResetNearbyTileEffects()
        {
            CoralReefsTiles = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            CoralReefsTiles = tileCounts[ModContent.TileType<GemsandstoneTile>()] + tileCounts[ModContent.TileType<HardenedGemsandTile>()];
        }

        public static int customBiome = 0;
        public static bool eocFlag;


        public static bool shipComplete;

        public static List<Vector2> missingShipTiles = new List<Vector2>();

        public static void ShipComplete()
        {

            missingShipTiles.Clear();
            int ShipTilePosX =(int)(ree.X);
            int ShipTilePosY = (int)(ree.Y);

            for (int i = ShipTilePosX; i< ShipTilePosX + ShipTiles.GetLength(1); i++)
            {
                for (int j = ShipTilePosY; j < ShipTilePosY + ShipTiles.GetLength(0); j++)
                {
                    Tile tile = Framing.GetTileSafely(i-3, j-6);
                    int expectedType = 0;
                    switch (ShipTiles[j - ShipTilePosY,i-ShipTilePosX])
                    {
                        case 1:
                            expectedType = TileID.WoodBlock;
                            break;
                        case 2:
                            expectedType = TileID.RichMahogany;
                            break;
                        case 3:
                            expectedType = TileID.GoldCoinPile;
                            break;
                        case 4:
                            expectedType = TileID.Platforms;
                            break;
                        case 5:
                            expectedType = TileID.WoodenBeam;
                            break;
                        case 6:
                            expectedType = TileID.SilkRope;
                            break;
                        default:
                            expectedType = -1;
                            break;
                    }
                    if (tile.type != expectedType && expectedType != -1)
                    {
                        missingShipTiles.Add(new Vector2(i, j));

                    }
                    if (expectedType == -1 && tile.active())
                    {
                        missingShipTiles.Add(new Vector2(i, j));
                    }
                }
            }

            if (missingShipTiles.Count == 0)
                shipComplete = true;
            else
                shipComplete = false;
        }
    }
}

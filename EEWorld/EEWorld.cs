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
using EEMod.Tiles;
using EEMod.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using EEMod.ID;

namespace EEMod.EEWorld
{
    public partial class EEWorld : ModWorld
    {
        //public static bool GenkaiMode;

        public int minionsKilled;
        public static EEWorld instance => ModContent.GetInstance<EEWorld>();
        public static bool downedTalos;
        public static bool downedCoralGolem;
        public static bool downedAkumo;
        public static bool downedHydros;
        public static bool downedOmen;
        public static bool downedKraken;
        public static bool omenPath;

        // private static List<Point> BiomeCenters;
        public static int CoralReefsTiles = 0;
        public static Vector2 yes;
        public static Vector2 ree;
        public static IList<Vector2> EntracesPosses = new List<Vector2>();

        public static Vector2[] PylonBegin = new Vector2[100];
        public static Vector2[] PylonEnd = new Vector2[100];
        public static List<Vector2> ChainConnections = new List<Vector2>();
        public override void Initialize()
        {
            eocFlag = NPC.downedBoss1;
            if (EntracesPosses.Count > 0)
                yes = EntracesPosses[0];
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                for (int i = 42; i < Main.maxTilesX-42; i++)
                {
                    for (int j = 42; j < Main.maxTilesY-42; j++)
                    {
                        if (TileCheck2(i,j) != 0 && WorldGen.genRand.NextBool(2))
                        {
                            if (ChainConnections.Count == 0)
                            {
                                ChainConnections.Add(new Vector2(i, j));
                            }
                            else
                            {
                                Vector2 lastPos = ChainConnections[ChainConnections.Count - 1];
                                if(Vector2.Distance(lastPos,new Vector2(i,j)) > 20 && Vector2.Distance(lastPos, new Vector2(i, j)) < 50)
                                {
                                    ChainConnections.Add(new Vector2(i, j));
                                }
                            }
                        }
                    }
                }
                
            }
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
            Main.spriteBatch.Begin();
            for(int i = 1; i< ChainConnections.Count - 2; i++)
            {
                Vector2 ChainConneccPos = ChainConnections[i] * 16;
                Vector2 LastChainConneccPos = ChainConnections[i-1] * 16;
                Helpers.DrawBezier(Main.spriteBatch,TextureCache.Vine, "", Color.White, ChainConneccPos, LastChainConneccPos, LastChainConneccPos, LastChainConneccPos, 0.01f, 0);
            }
            Main.spriteBatch.End();
            if (NPC.downedBoss1)
            {
                if (!eocFlag)
                {
                    eocFlag = true;
                    Main.NewText("You hear a strong wind erupting from the desert...", 228, 171, 72);
                    StartSandstorm();
                }
            }

        }

        public static Vector2 SubWorldSpecificCoralBoatPos;
        public static Vector2 SubWorldSpecificVolcanoInsidePos = new Vector2(198, 189);

        public override void ResetNearbyTileEffects()
        {
            CoralReefsTiles = 0;
        }
        public override void TileCountsAvailable(int[] tileCounts)
        {
            CoralReefsTiles = tileCounts[ModContent.TileType<DarkGemsandTile>()] + tileCounts[ModContent.TileType<GemsandTile>()] + tileCounts[ModContent.TileType<LightGemsandTile>()];
        }

        public static int customBiome = 0;
        public static bool eocFlag;


        public static bool shipComplete;

        public static List<Vector2> missingShipTiles = new List<Vector2>();
        public static List<Texture2D> missingShipTilesItems = new List<Texture2D>();
        public static List<Vector2> missingShipTilesRespectedPos = new List<Vector2>();
        public static void ShipComplete()
        {
            missingShipTiles.Clear();
            missingShipTilesItems.Clear();
            missingShipTilesRespectedPos.Clear();
            int? nullableReeX = (int)ree.X;
            int? nullableReeY = (int)ree.Y;
            int ShipTilePosX = nullableReeX ?? 100;
            int ShipTilePosY = nullableReeY ?? TileCheckWater(100) - 22;
            if (ree == Vector2.Zero)
            {
                ShipTilePosX = 100;
                ShipTilePosY = TileCheckWater(100) - 22;
            }
            for (int i = ShipTilePosX; i < ShipTilePosX + ShipTiles.GetLength(1); i++)
            {
                for (int j = ShipTilePosY; j < ShipTilePosY + ShipTiles.GetLength(0); j++)
                {
                    if (WorldGen.InWorld(i - 3, i - 6))
                    {
                        Tile tile = Framing.GetTileSafely(i - 3, j - 6);
                        int expectedType;
                        switch (ShipTiles[j - ShipTilePosY, i - ShipTilePosX])
                        {
                            case 1:
                                expectedType = TileID.WoodBlock;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(Main.itemTexture[ItemID.Wood]);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                            case 2:
                                expectedType = TileID.RichMahogany;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(Main.itemTexture[ItemID.RichMahogany]);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                            case 3:
                                expectedType = TileID.GoldCoinPile;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(Main.itemTexture[ItemID.GoldCoin]);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                            case 4:
                                expectedType = TileID.Platforms;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(Main.itemTexture[ItemID.WoodPlatform]);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                            case 5:
                                expectedType = TileID.WoodenBeam;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(Main.itemTexture[ItemID.WoodenBeam]);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                            case 6:
                                expectedType = TileID.SilkRope;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(Main.itemTexture[ItemID.SilkRope]);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                            default:
                                expectedType = 0;
                                if (tile.type != expectedType)
                                {
                                    missingShipTilesItems.Add(TextureCache.Empty);
                                    missingShipTilesRespectedPos.Add(new Vector2(i, j));
                                }
                                break;
                        }
                        if (tile.type != expectedType)
                        {
                            missingShipTiles.Add(new Vector2(i, j));
                        }
                    }
                }
            }

            if (missingShipTiles.Count == 0)
                shipComplete = true;
            else
                shipComplete = false;
        }

        public static bool HydrosCheck()
        {
            if (instance.minionsKilled >= 5)
                return true;
            else
                return false;
        }
    }
}

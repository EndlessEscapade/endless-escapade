using EEMod.Autoloading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using EEMod.ID;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using Terraria.GameContent.Events;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Terraria.DataStructures;
using EEMod.Tiles.EmptyTileArrays;
using System.Linq;
using EEMod.VerletIntegration;
using EEMod.Prim;
using EEMod.Systems.Subworlds.EESubworlds;
using Terraria.UI;
using EEMod.NPCs.Friendly;

namespace EEMod.EEWorld
{
    public partial class EEWorld : ModSystem
    {
        public int minionsKilled;
        public static EEWorld instance => ModContent.GetInstance<EEWorld>();

        // private static List<Point> BiomeCenters;
        public static Vector2 yes;

        public static Vector2 ree;

        [FieldInit(FieldInitType.SubType, typeof(List<Vector2>))]
        public static IList<Vector2> EntracesPosses = new List<Vector2>();

        [FieldInit(FieldInitType.ArrayIntialization, 6)]
        public static byte[] LightStates = new byte[6];

        [FieldInit(FieldInitType.ArrayIntialization, 100)]
        public static Vector2[] PylonBegin = new Vector2[100];

        [FieldInit(FieldInitType.ArrayIntialization, 100)]
        public static Vector2[] PylonEnd = new Vector2[100];

        [FieldInit(FieldInitType.ArrayIntialization, 10000)]
        public static Vector2[] sinDis = new Vector2[10000];

        //public Vector2[] sinDis = new Vector2[10000];

        public override void OnWorldLoad()
        {
            //ModContent.GetInstance<EEMod>().TVH.Clear();
            if (sinDis != null)
            {
                for (int i = 0; i < sinDis.Length; i++)
                {
                    // When an array is created all elements get initialized to the default value, and the default value of structs isn't null
                    sinDis[i].X = WorldGen.genRand.NextFloat(0, 0.03f);
                }
            }
            eocFlag = NPC.downedBoss1;
            if (EntracesPosses != null)
            {
                if (EntracesPosses.Count > 0)
                {
                    yes = EntracesPosses[0];
                }
            }
            Main.LocalPlayer.GetModPlayer<EEPlayer>().isInSubworld = Main.ActiveWorldFileData.Path.Contains($@"{Main.SavePath}\Worlds\{Main.LocalPlayer.GetModPlayer<EEPlayer>().baseWorldName}Subworlds");

            builtShip = ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/builtboat.lcs");

            //EESubWorlds.placedShipTether = false;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                //tasks.Insert(ShiniesIndex + 1, new PassLegacy("Endless Escapade Ores", EEModOres));
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
            for (int i = 0; i < LightStates.Length; i++)
            {
                writer.Write(LightStates[i]);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            ree = reader.ReadVector2();
            yes = reader.ReadVector2();
            for (int i = 0; i < LightStates.Length; i++)
            {
                LightStates[i] = reader.ReadByte();
            }
        }

        public override void PostWorldGen()
        {
            DoAndAssignShipyardValues();

            for (int i = 0; i < sinDis.Length; i++)
            {
                sinDis[i].X = Main.rand.NextFloat(0, 0.03f);
            }
        }

        public override void PostUpdateEverything()
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
        }

        public static Vector2 SubWorldSpecificCoralBoatPos;
        public static Vector2 SubWorldSpecificVolcanoInsidePos = new Vector2(198, 189);

        public static int customBiome = 0;
        public static bool eocFlag;

        public static bool shipComplete;

        public static List<Vector2> missingShipTiles = new List<Vector2>();
        public static List<Texture2D> missingShipTilesItems = new List<Texture2D>();
        public static List<Vector2> missingShipTilesRespectedPos = new List<Vector2>();

        public static void ShipComplete()
        {
            /*if (ree == Vector2.Zero)
            {
                ShipTilePosX = 100;
                ShipTilePosY = TileCheckWater(100) - 22;
            }
            for (int i = ShipTilePosX; i < ShipTilePosX + ShipTiles.GetLength(1); i++)
            {
                for (int j = ShipTilePosY; j < ShipTilePosY + ShipTiles.GetLength(0); j++)
                {
                    if (WorldGen.InWorld(i - 3, i - 6, 32))
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
                                    missingShipTilesItems.Add(ModContent.GetInstance<EEMod>().GetTexture("Empty"));
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
            }*/

            //int? nullableReeX = (int)ree.X; // not a null value
            //int? nullableReeY = (int)ree.Y;
            Vector2 revisedRee = new Vector2(ree.X == 0 ? 100 : ree.X,
                                             ree.Y == 0 ? TileCheckWater(100) - 22 : ree.Y);

            int ShipTilePosX = (int)revisedRee.X;// ?? 100;
            int ShipTilePosY = (int)revisedRee.Y;// ?? TileCheckWater(100) - 22;

            bool hasSteeringWheel = false;
            int numberOfTiles = 0;
            for (int i = ShipTilePosX; i < ShipTilePosX + ShipTiles.GetLength(1); i++)
            {
                for (int j = ShipTilePosY; j < ShipTilePosY + ShipTiles.GetLength(0); j++)
                {
                    if (Framing.GetTileSafely(i, j).IsActive == true)
                    {
                        numberOfTiles++;
                    }
                    if (Framing.GetTileSafely(i, j).type == ModContent.TileType<WoodenShipsWheelTile>())
                    {
                        hasSteeringWheel = true;
                    }
                }
            }

            if (hasSteeringWheel && numberOfTiles > 100)
            {
                shipComplete = true;
            }
            else
            {
                shipComplete = false;
            }


        }

        public static bool HydrosCheck()
        {
            if (instance.minionsKilled >= 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IList<Vector2> Vines = new List<Vector2>();

        public override void LoadWorldData(TagCompound tag)
        {
            tag.TryGetListRef("EntracesPosses", ref EntracesPosses);
            tag.TryGetRef("CoralBoatPos", ref CoralReefs.CoralBoatPos);
            tag.TryGetRef("SubWorldSpecificVolcanoInsidePos", ref SubWorldSpecificVolcanoInsidePos);
            tag.TryGetRef("yes", ref yes);
            tag.TryGetRef("ree", ref ree);
            tag.TryGetRef("SpirePosition", ref CoralReefs.SpirePosition);
            tag.TryGetListRef("CoralReefVineLocations", ref CoralReefs.CoralReefVineLocations);
            tag.TryGetListRef("AquamarineZiplineLocations", ref CoralReefs.AquamarineZiplineLocations);
            tag.TryGetListRef("ThinCrystalBambooLocations", ref CoralReefs.ThinCrystalBambooLocations);
            tag.TryGetListRef("BulbousTreePosition", ref CoralReefs.BulbousTreePosition);
            tag.TryGetListRef("WebPositions", ref CoralReefs.WebPositions);


            IList<Vector2> positions = new List<Vector2>();
            IList<Vector2> sizes = new List<Vector2>();
            IList<int> ids = new List<int>();

            tag.TryGetListRef("CoralMinibiomesPositions", ref positions);
            tag.TryGetListRef("CoralMinibiomesSizes", ref sizes);
            tag.TryGetListRef("CoralMinibiomesIds", ref ids);

            tag.TryGetRef("ShipCoords", ref shipCoords);

            for (int i = 0; i < positions.Count; i++)
            { 
                if(ids[i] == (int)MinibiomeID.None)
                {
                    continue;
                }
                else if (ids[i] == (int)MinibiomeID.AquamarineCaverns)
                {
                    CoralReefs.Minibiomes.Add(new AquamarineCaverns()
                    {
                        Position = positions[i].ToPoint(),
                        Size = sizes[i].ToPoint(),
                        EnsureNoise = false
                    });

                    continue;
                }
                else if (ids[i] == (int)MinibiomeID.GlowshroomGrotto)
                {
                    CoralReefs.Minibiomes.Add(new GlowshroomGrotto()
                    {
                        Position = positions[i].ToPoint(),
                        Size = sizes[i].ToPoint(),
                        EnsureNoise = false
                    });

                    continue;
                }
                else if (ids[i] == (int)MinibiomeID.KelpForest)
                {
                    CoralReefs.Minibiomes.Add(new KelpForest()
                    {
                        Position = positions[i].ToPoint(),
                        Size = sizes[i].ToPoint(),
                        EnsureNoise = false
                    });

                    continue;
                }
                else if (ids[i] == (int)MinibiomeID.ThermalVents)
                {
                    CoralReefs.Minibiomes.Add(new ThermalVents()
                    {
                        Position = positions[i].ToPoint(),
                        Size = sizes[i].ToPoint(),
                        EnsureNoise = false
                    });

                    continue;
                }
                else
                {
                    Main.NewText("something went wrong");
                }
            }



            //tag.TryGetListRef("CoralReefMinibiomes", ref CoralReefs.Minibiomes);

            /*if (tag.TryGetListRef("WebPositions", ref EESubWorlds.WebPositions))
            {
                if (EESubWorlds.WebPositions.Count != 0)
                {
                    foreach (Vector2 vec in EESubWorlds.WebPositions)
                    {
                        for (int i = 0; i < 12; i++)
                            PrimSystem.primitives.CreateTrail(new WebPrimTrail(null, vec * 16, i));
                    }
                }
            }*/

            if (tag.TryGetListRef("SwingableVines", ref VerletHelpers.SwingableVines))
            {
                //TODO: Confirm moving this here didn't break anything
                VerletHelpers.SwingableVines.Clear();
                if (VerletHelpers.SwingableVines.Count != 0)
                {
                    foreach (Vector2 vec in VerletHelpers.SwingableVines)
                    {
                        VerletHelpers.AddStickChainNoAdd(ref ModContent.GetInstance<EEMod>().verlet, vec, Main.rand.Next(5, 10), 27);
                    }
                }
            }
            if (tag.TryGetList<Vector2>("EmptyTileVectorMain", out IList<Vector2> vecMains) && tag.TryGetList<Vector2>("EmptyTileVectorSub", out IList<Vector2> list2))
            {
                EmptyTileEntities.Instance.EmptyTilePairsCache = vecMains.Zip(list2, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
            }
            if (tag.TryGetList<Vector2>("EmptyTileVectorEntities", out var VecMains) && tag.TryGetList<EmptyTileEntity>("EmptyTileEntities", out var VecSubs))
            {
                EmptyTileEntities.Instance.EmptyTileEntityPairsCache = VecMains.Zip(VecSubs, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
                IList<EmptyTileEntity> emptyTileEntities = VecSubs;
                EmptyTileEntities.Instance.ETES.Clear();
                foreach (EmptyTileEntity ETEnt in emptyTileEntities)
                {
                    EmptyTileEntities.Instance.ETES.Add(ETEnt);
                }
            }
            if (tag.TryGetListRef<Vector2>("CoralCrystalPosition", ref CoralReefs.CoralCrystalPosition))
            {
                // for (int i = 0; i < EESubWorlds.CoralCrystalPosition.Count; i++)
                //    EmptyTileEntityCache.AddPair(new Crystal(EESubWorlds.CoralCrystalPosition[i]), EESubWorlds.CoralCrystalPosition[i], EmptyTileArrays.CoralCrystal);
            }
            tag.TryGetByteArrayRef("LightStates", ref LightStates);
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                tag["CoralBoatPos"] = CoralReefs.CoralBoatPos;
                tag["CoralReefVineLocations"] = CoralReefs.CoralReefVineLocations;
                tag["AquamarineZiplineLocations"] = CoralReefs.AquamarineZiplineLocations;
                tag["ThinCrystalBambooLocations"] = CoralReefs.ThinCrystalBambooLocations;
                tag["BulbousTreePosition"] = CoralReefs.BulbousTreePosition;
                tag["WebPositions"] = CoralReefs.WebPositions;

                List<Vector2> positions = new List<Vector2>();
                List<Vector2> sizes = new List<Vector2>();
                List<int> ids = new List<int>();

                foreach (CoralReefMinibiome mb in CoralReefs.Minibiomes)
                {
                    positions.Add(mb.Position.ToVector2());
                    sizes.Add(mb.Size.ToVector2());
                    ids.Add((int)mb.id);
                }

                tag["CoralMinibiomesSize"] = positions;
                tag["CoralMinibiomesId"] = sizes;
                tag["CoralMinibiomesPosition"] = ids;

                //tag["CoralReefMinibiomes"] = CoralReefs.Minibiomes;

                tag["SwingableVines"] = VerletHelpers.SwingableVines;
                tag["LightStates"] = LightStates;
                tag["CoralCrystalPosition"] = CoralReefs.CoralCrystalPosition;
                tag["SpirePosition"] = CoralReefs.SpirePosition;
                tag["EmptyTileVectorMain"] = EmptyTileEntities.Instance.EmptyTilePairsCache.Keys.ToList();
                tag["EmptyTileVectorSub"] = EmptyTileEntities.Instance.EmptyTilePairsCache.Values.ToList();
                tag["EmptyTileVectorEntities"] = EmptyTileEntities.Instance.EmptyTileEntityPairsCache.Keys.ToList();
                tag["EmptyTileEntities"] = EmptyTileEntities.Instance.EmptyTileEntityPairsCache.Values.ToList();


            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoInside)
            {
                tag["SubWorldSpecificVolcanoInsidePos"] = SubWorldSpecificVolcanoInsidePos;
            }
            tag["EntracesPosses"] = EntracesPosses;
            tag["yes"] = yes;
            tag["ree"] = ree;
            tag["ShipCoords"] = shipCoords;
        }
    }
}
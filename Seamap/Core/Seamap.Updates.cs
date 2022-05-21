using EEMod.Seamap.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEMod;
using Terraria.Audio;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using EEMod.Systems.Noise;
using EEMod.Seamap.Content.Islands;

namespace EEMod.Seamap.Core
{
    public partial class Seamap
    {
        public static void UpdateSeamap()
        {
            for (int i = 0; i < SeamapObjects.SeamapEntities.Length; i++)
            {
                if (SeamapObjects.SeamapEntities[i] != null)
                {
                    SeamapObjects.SeamapEntities[i].Update();
                    //SeamapObjects.SeamapEntities[i].UpdateComponents();
                }
            }

            permaWindVector += windVector;

            if (Main.GameUpdateCount % 600 == 0 && SeamapObjects.localship != null)
            {
                SpawnSeagullFlock(Main.rand.Next(4, 8));
            }

            windRot += ((float)Math.Sin(Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount / 1600f) * Main.rand.NextFloat(1f) / 120f);

            windVector = Vector2.UnitY.RotatedBy(windRot);
            windVector.Y *= 0.6f;
        }

        public static float windRot;
        public static Vector2 windVector;

        public static bool firstOpened = true;

        public static void InitializeSeamap()
        {
            SeamapObjects.InitObjects(new Vector2(seamapWidth - 450, seamapWidth - 500));

            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamap = false;
            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().quickOpeningFloat = 5f;

            Main.LocalPlayer.GetModPlayer<SeamapPlayer>().myLastBoatPos = Vector2.Zero;

            SeamapObjects.NewSeamapObject(new MainIsland(new Vector2(seamapWidth - 402 - 30, seamapHeight - 118 - 200 - 30)));

            for (int i = 0; i < 20; i++)
            {
                PlaceRockCluster(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(300, seamapHeight - 800)));
            }

            for (int i = 0; i < 40; i++)
            {
                PlaceRock(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(300, seamapHeight - 800)));
            }

            #region Tropical water generation
            SeamapObjects.NewSeamapObject(new CoralReefsIsland(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));

            SeamapObjects.NewSeamapObject(new VolcanoIsland(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));

            SeamapObjects.NewSeamapObject(new TropicalIsland1(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));
            #endregion

            #region Moderate water generation
            SeamapObjects.NewSeamapObject(new TropicalIsland2(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));

            //SeamapObjects.NewSeamapObject(new TropicalIslandAlt(new Vector2(3500, 3500)));

            SeamapObjects.NewSeamapObject(new MoyaiIsland(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));

            SeamapObjects.NewSeamapObject(new JadeIsles(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));

            SeamapObjects.NewSeamapObject(new Iceberg2(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));
            #endregion

            //SeamapObjects.NewSeamapObject(new GoblinFort(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(2000, seamapHeight - 300))));
            PlaceGoblinFort(new Vector2(seamapWidth - 500, seamapHeight - 800));

            SpawnSeagullFlock(Main.rand.Next(4, 8));
            SpawnSeagullFlock(Main.rand.Next(4, 8));
            SpawnSeagullFlock(Main.rand.Next(4, 8));
        }

        public static void PlaceRock(Vector2 position, int type = -1)
        {
            switch (type == -1 ? Main.rand.Next(6) : type)
            {
                case 0:
                    SeamapObjects.NewSeamapObject(new Rock1(position));
                    break;
                case 1:
                    SeamapObjects.NewSeamapObject(new Rock6(position));
                    break;
                case 2:
                    SeamapObjects.NewSeamapObject(new Rock3(position));
                    break;
                case 3:
                    SeamapObjects.NewSeamapObject(new Rock4(position));
                    break;
                case 4:
                    SeamapObjects.NewSeamapObject(new Rock5(position));
                    break;
                case 5:
                    SeamapObjects.NewSeamapObject(new Rock2(position));
                    break;
            }
        }

        public static void PlaceRockCluster(Vector2 center)
        {
            float val = Main.rand.NextFloat(6.28f);

            for (int i = 0; i < 3; i++)
            {
                float val2 = val + ((i * 6.28f) / 3f);

                PlaceRock(new Vector2((float)Math.Cos(val2) * 50, (float)Math.Sin(val2) * 25) + center, Main.rand.Next(4));
            }
        }

        public static void SpawnSeagullFlock(int amount)
        {
            Vector2 Pos = new Vector2(Main.rand.Next(Main.screenWidth) + Main.screenPosition.X, Main.screenPosition.Y - 100);
            List<Vector2> PosBuffer = new List<Vector2>();
            for (int i = 0; i < amount; i++)
            {
                Pos += new Vector2(Main.rand.Next(-30, 30), Main.rand.Next(-30, 30));

                SeamapObject seagull = new Seagull(Pos, new Vector2(0, Main.rand.NextFloat(0.5f, 1)));

                seagull.scale = Main.rand.NextFloat(0.5f, 1f);
                //seagull.alpha = Main.rand.NextFloat(.2f, .8f);
                //seagull.flash = Main.rand.NextFloat(0, 100);

                int boidCheck = 0;
                for (int j = 0; j < PosBuffer.Count; j++)
                {
                    if (Vector2.DistanceSquared(Pos, PosBuffer[j]) < 10 * 10)
                    {
                        boidCheck++;
                    }
                }
                if (boidCheck == 0)
                {
                    PosBuffer.Add(Pos);
                    SeamapObjects.NewSeamapObject(seagull);
                }
            }
        }

        public static void PlaceGoblinFort(Vector2 pos)
        {
            float val = Main.rand.NextFloat(6.28f);

            for (int i = 0; i < 11; i++)
            {
                float val2 = val + ((i * 6.28f) / 14f);

                PlaceRock(new Vector2((float)Math.Cos(val2) * Main.rand.Next(190, 211), (float)Math.Sin(val2) * Main.rand.Next(140, 161)) + pos, Main.rand.Next(4));
            }

            SeamapObjects.NewSeamapObject(new GoblinFortIsland(pos));
        }
    }
}


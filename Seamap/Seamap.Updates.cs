using EEMod.Seamap.SeamapAssets;
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

namespace EEMod.Seamap.SeamapContent
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
                }
            }

            if(Main.GameUpdateCount % 180 == 0 && Main.rand.NextBool(3))
            {
                SpawnSeagullFlock(Main.rand.Next(4, 8));
            }

            if (Main.GameUpdateCount % 180 == 0 && Main.rand.NextBool(3))
            {
                SpawnClouds();
            }

            if (firstOpened)
            {
                Seamap.SpawnClouds();
                Seamap.SpawnClouds();
                Seamap.SpawnClouds();

                Seamap.SpawnSeagullFlock(Main.rand.Next(4, 8));
                Seamap.SpawnSeagullFlock(Main.rand.Next(4, 8));

                firstOpened = false;
            }
        }

        public static bool firstOpened = true;

        public static void InitializeSeamap()
        {
            SeamapObjects.InitObjects(new Vector2(seamapWidth - 450, seamapWidth - 100));

            SeamapObjects.NewSeamapObject(new MainIsland(new Vector2(seamapWidth - 402, seamapHeight - 118 - 500)));

            //SeamapObjects.NewSeamapObject(new RedDutchman(new Vector2(seamapWidth - 500, seamapHeight - 500), Vector2.Zero));

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

            SeamapObjects.NewSeamapObject(new MoyaiIsland(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));

            SeamapObjects.NewSeamapObject(new JadeIsles(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(800, seamapHeight - 800))));
            #endregion

            //SeamapObjects.NewSeamapObject(new GoblinFort(new Vector2(Main.rand.Next(300, seamapWidth - 300), Main.rand.Next(2000, seamapHeight - 300))));
            SeamapObjects.NewSeamapObject(new GoblinFort(new Vector2(seamapWidth - 500, seamapHeight - 500)));

            for (int iterations = 0; iterations < 5; iterations++)
            {
                for (int i = 0; i < 50; i++)
                {
                    BorderCloud newCloud = new BorderCloud(new Vector2(i * (seamapWidth / 50f), (seamapHeight - 500) - (30 * (4 - iterations))) + new Vector2(Main.rand.Next(-8, 9), Main.rand.Next(-8, 9)), Vector2.Zero);

                    newCloud.lerpToBlack = 1 - (0.33f * iterations);

                    SeamapObjects.NewSeamapObject(newCloud);
                }
            }

            for (int iterations = 0; iterations < 5; iterations++)
            {
                for (int i = 0; i < 50; i++)
                {
                    BorderCloud newCloud = new BorderCloud(new Vector2(i * (seamapWidth / 50f), -80 + (30 * (4 - iterations))) + new Vector2(Main.rand.Next(-8, 9), Main.rand.Next(-8, 9)), Vector2.Zero);

                    newCloud.lerpToBlack = 1 - (0.33f * iterations);

                    SeamapObjects.NewSeamapObject(newCloud);
                }
            }

            for (int iterations = 0; iterations < 5; iterations++)
            {
                for (int i = 0; i < 120; i++)
                {
                    BorderCloud newCloud = new BorderCloud(new Vector2(-50 + (30 * (4 - iterations)), i * (seamapHeight / 120f)) + new Vector2(Main.rand.Next(-8, 9), Main.rand.Next(-8, 9)), Vector2.Zero);

                    newCloud.lerpToBlack = 1 - (0.33f * iterations);

                    SeamapObjects.NewSeamapObject(newCloud);
                }
            }

            for (int iterations = 0; iterations < 5; iterations++)
            {
                for (int i = 0; i < 120; i++)
                {
                    BorderCloud newCloud = new BorderCloud(new Vector2(-80 + (30 * (4 - iterations)) + (seamapWidth - 100), i * (seamapHeight / 120f)) + new Vector2(Main.rand.Next(-8, 9), Main.rand.Next(-8, 9)), Vector2.Zero);

                    newCloud.lerpToBlack = 0.33f * iterations;

                    SeamapObjects.NewSeamapObject(newCloud);
                }
            }
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
                Pos += new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20));

                SeamapObject seagull = new Seagull(Pos, new Vector2(0, Main.rand.NextFloat(0.5f, 1)));

                seagull.scale = Main.rand.NextFloat(0.5f, 1f);
                //seagull.alpha = Main.rand.NextFloat(.2f, .8f);
                //seagull.flash = Main.rand.NextFloat(0, 100);

                int boidCheck = 0;
                for (int j = 0; j < PosBuffer.Count; j++)
                {
                    if (Vector2.DistanceSquared(Pos, PosBuffer[j]) < 5 * 5)
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

        public static void SpawnClouds()
        {
            Vector2 Pos = new Vector2(Main.screenWidth + Main.screenPosition.X + 200, Main.rand.Next(Main.screenHeight) + Main.screenPosition.Y);

            switch (Main.rand.Next(3))
            {
                case 0:
                    SeamapObjects.NewSeamapObject(new AirCloud1(Pos, Vector2.Zero));
                    break;
                case 1:
                    SeamapObjects.NewSeamapObject(new AirCloud2(Pos, Vector2.Zero));
                    break;
                case 2:
                    SeamapObjects.NewSeamapObject(new AirCloud3(Pos, Vector2.Zero));
                    break;
            }
        }
    }
}


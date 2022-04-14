using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.VerletIntegration
{
    public static class VerletHelpers
    {
        struct VineInfo
        {
            public Vector2 position;
            public int numberOfChains;
            public float lengthOfChains;
            public string glowmaskPath;
            public string texturePath;
            public Texture2D glowmask => EEMod.Instance.Assets.Request<Texture2D>(glowmaskPath).Value;
            public Texture2D texture => EEMod.Instance.Assets.Request<Texture2D>(texturePath).Value;
            public VineInfo(Vector2 position, int numberOfChains, float lengthOfChains, string texturePath, string glowmaskPath)
            {
                this.position = position;
                this.numberOfChains = numberOfChains;
                this.lengthOfChains = lengthOfChains;
                this.glowmaskPath = glowmaskPath;
                this.texturePath = texturePath;
            }
        }
        public static HashSet<int> EndPointChains = new HashSet<int>();
        public static IList<Vector2> SwingableVines = new List<Vector2>();

        public static void AddStickChain(ref Verlet verlet, Vector2 position, int numberOfChains, float lengthOfChains)
        {
            int SpacialTolerance = 0;

            foreach (Vector2 pos in SwingableVines) 
                if(Vector2.DistanceSquared(pos,position) < 100)
                { 
                    SpacialTolerance++; 
                }

            if (SpacialTolerance > 0) return;

            Point p = position.ToTileCoordinates();

            if (Framing.GetTileSafely(p).HasTile)
            {
                Vector2 TP = (position + new Vector2(0, lengthOfChains * (numberOfChains - 1))) / 16;
                if (!Framing.GetTileSafely((int)TP.X, (int)TP.Y).HasTile)
                {
                    for (int i = 0; i < numberOfChains; i++)
                    {
                        EEMod eemood = EEMod.Instance;
                        int a = verlet.CreateVerletPoint(position + new Vector2(0, lengthOfChains * i), i == 0 ? true : false);
                        if (i == 0)
                        {
                            if(!SwingableVines.Contains(position))
                            SwingableVines.Add(position);
                        }
                        if (i > 1)
                        {
                            int vineRand = Main.rand.Next(0, 6);
                            if (vineRand != 0 && vineRand != 3)
                                verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand).Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand + "Glow").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand + "Map").Value);
                            else
                                verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand).Value);
                        }
                        if (i == 1)
                        {
                            int vineRand = Main.rand.Next(0, 3);
                            if (vineRand != 0)
                                verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand).Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand + "Glow").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand + "Map").Value);
                            else
                                verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand).Value);
                        }

                        if (i == numberOfChains - 1)
                        {
                            verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineCap").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineCapGlow").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineCapMap").Value);
                            EndPointChains.Add(a);
                        }
                    }
                }
            }
        }

        public static void AddStickChainNoAdd(ref Verlet verlet, Vector2 position, int numberOfChains, float lengthOfChains)
        {
            int SpacialTolerance = 0;

            foreach (Vector2 pos in SwingableVines) 
                if (Vector2.DistanceSquared(pos, position) < 30) 
                { 
                    SpacialTolerance++; 
                }

            if (SpacialTolerance > 1) return;

            Point p = position.ToTileCoordinates();
            if (Framing.GetTileSafely(p).HasTile)
            {
                for (int i = 0; i < numberOfChains; i++)
                {
                    EEMod eemood = EEMod.Instance;
                    int a = verlet.CreateVerletPoint(position + new Vector2(0, lengthOfChains * i), i == 0 ? true : false);
                    if (i > 1)
                    {
                        int vineRand = Main.rand.Next(0, 6);
                        if (vineRand != 0 && vineRand != 3)
                            verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand).Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand + "Glow").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand + "Map").Value);
                        else
                            verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/Vine" + vineRand).Value);
                    }
                    if (i == 1)
                    {
                        int vineRand = Main.rand.Next(0, 3);
                        if (vineRand != 0)
                            verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand).Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand + "Glow").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand + "Map").Value);
                        else
                            verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineBase" + vineRand).Value);
                    }

                    if (i == numberOfChains - 1)
                    {
                        verlet.BindPoints(a, a - 1, true, default, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineCap").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineCapGlow").Value, eemood.Assets.Request<Texture2D>("Textures/Vines/HangingVines/VineCapMap").Value);
                        EndPointChains.Add(a);
                    }
                }
            }
        }

        public static void LoadVines()
        {

        }

    }
}

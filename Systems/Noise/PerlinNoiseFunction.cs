using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Utilities;

namespace EEMod.Systems.Noise
{
    public class PerlinNoiseFunction
    {
        public float[,] perlin;
        public float[,] perlin2;
        public float[,] perlinValues;
        public int[,] perlinBinary;
        static Vector2 one = new Vector2(1, 1), onem1 = new Vector2(1, -1), m1m1 = new Vector2(-1, -1), m1one = new Vector2(-1, 1);

        private static float DotProduct(ref Vector2 a, ref Vector2 b) => a.X * b.X + a.Y * b.Y;

        private static float FadeFunction(float input) => (float)((6 * input * input * input * input * input) - (15 * input * input * input * input) + (10 * input * input * input));

        public PerlinNoiseFunction(int sizeX, int sizeY, int widthOfCell, int heightOfCell, float Threshold, UnifiedRandom random = null)
        {
            UnifiedRandom rand = random ?? Main.rand;
            perlinBinary = new int[sizeX, sizeY];
            perlin = new float[sizeX, sizeY];
            perlin2 = new float[sizeX, sizeY];
            perlinValues = new float[sizeX, sizeY];

            //Vector2[] VectorTables = { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) };
            int numberOfIterationsX = sizeX / widthOfCell;
            int numberOfIterationsY = sizeY / heightOfCell;
            Vector2[,] Grad = new Vector2[numberOfIterationsX + 1, numberOfIterationsY + 1];

            int i, j, k, l;

            for (i = 0; i < numberOfIterationsX + 1; i++)
            {
                for (j = 0; j < numberOfIterationsY + 1; j++)
                {
                    //Grad[i, j] = VectorTables[Main.rand.Next(4)];
                    switch (rand.Next(4))
                    {
                        case 0: Grad[i, j] = one; break;
                        case 1: Grad[i, j] = onem1; break;
                        case 2: Grad[i, j] = m1m1; break;
                        case 3: Grad[i, j] = m1one; break;
                    }
                }
            }

            for (i = 0; i < widthOfCell; i++)
            {
                for (j = 0; j < heightOfCell; j++)
                {
                    for (k = 0; k < numberOfIterationsX; k++)
                    {
                        for (l = 0; l < numberOfIterationsY; l++)
                        {
                            float iNew = FadeFunction(i / (float)widthOfCell);
                            float jNew = FadeFunction(j / (float)heightOfCell);
                            float iOld = i / (float)widthOfCell;
                            float jOld = j / (float)heightOfCell;

                            //Vector2[] DistanceFromEachPoint = { new Vector2(iOld, jOld), new Vector2(iOld - 1, jOld), new Vector2(iOld, jOld - 1), new Vector2(iOld - 1, jOld - 1) };
                            Vector2 disFromPoint0 = new Vector2(iOld, jOld);
                            Vector2 disFromPoint1 = new Vector2(iOld - 1, jOld);
                            Vector2 disFromPoint2 = new Vector2(iOld, jOld - 1);
                            Vector2 disFromPoint3 = new Vector2(iOld - 1, jOld - 1);

                            //float[] DP = { DotProduct(disFromPoint0, Grad[k, l]), DotProduct(disFromPoint1, Grad[k + 1, l]), DotProduct(disFromPoint2, Grad[k, l + 1]), DotProduct(disFromPoint3, Grad[k + 1, l + 1]) };
                            float DP0 = DotProduct(ref disFromPoint0, ref Grad[k, l]);
                            float DP1 = DotProduct(ref disFromPoint1, ref Grad[k + 1, l]);
                            float DP2 = DotProduct(ref disFromPoint2, ref Grad[k, l + 1]);
                            float DP3 = DotProduct(ref disFromPoint3, ref Grad[k + 1, l + 1]);

                            //float[] LerpOne = { DP[0] + (DP[1] - DP[0]) * iNew, DP[2] + (DP[3] - DP[2]) * iNew };
                            float LerpOne0 = DP0 + (DP1 - DP0) * iNew;
                            float LerpOne1 = DP2 + (DP3 - DP2) * iNew;

                            float LerpTwo = LerpOne0 + (LerpOne1 - LerpOne0) * jNew;
                            //Vector2 index = new Vector2(i + (k * widthOfCell), j + (l * heightOfCell));

                            int x = i + (k * widthOfCell); // (int)index.X;
                            int y = j + (l * heightOfCell); // (int)index.Y;

                            perlinBinary[x, y] = (perlin[x, y] = (LerpTwo + 1) * 0.5f) < Threshold ? 1 : 0;

                            perlinValues[x, y] = (perlin[x, y] = (LerpTwo + 1) * 0.5f);

                            perlin2[x, y] = LerpTwo;

                            /*if (perlin[(int)index.X, (int)index.Y] < Threshold)
                            {
                                perlinBinary[(int)index.X, (int)index.Y] = 1;
                            }
                            else
                            {
                                perlinBinary[(int)index.X, (int)index.Y] = 0;
                            }*/
                        }
                    }
                }
            }
        }
    }
}
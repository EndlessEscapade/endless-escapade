using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace EEMod
{
    public class PerlinNoiseFunction
    {
        public float[,] perlin;
        public float[,] perlin2;
        public int[,] perlinBinary;

        private float DotProduct(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

        private float FadeFunction(float input) => (float)((6 * Math.Pow(input, 5)) - (15 * Math.Pow(input, 4)) + (10 * Math.Pow(input, 3)));

        public PerlinNoiseFunction(int sizeX, int sizeY, int widthOfCell, int heightOfCell, float Threshold)
        {
            perlinBinary = new int[sizeX, sizeY];
            perlin = new float[sizeX, sizeY];
            perlin2 = new float[sizeX, sizeY];

            Vector2[] VectorTables = { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) };
            int numberOfIterationsX = sizeX / widthOfCell;
            int numberOfIterationsY = sizeY / heightOfCell;
            Vector2[,] Grad = new Vector2[numberOfIterationsX + 1, numberOfIterationsY + 1];

            for (int i = 0; i < numberOfIterationsX + 1; i++)
            {
                for (int j = 0; j < numberOfIterationsY + 1; j++)
                {
                    Grad[i, j] = VectorTables[Main.rand.Next(4)];
                }
            }

            for (int i = 0; i < widthOfCell; i++)
            {
                for (int j = 0; j < heightOfCell; j++)
                {
                    for (int k = 0; k < numberOfIterationsX; k++)
                    {
                        for (int l = 0; l < numberOfIterationsY; l++)
                        {
                            float iNew = FadeFunction(i / (float)widthOfCell);
                            float jNew = FadeFunction(j / (float)heightOfCell);
                            float iOld = i / (float)widthOfCell;
                            float jOld = j / (float)heightOfCell;
                            Vector2[] DistanceFromEachPoint = { new Vector2(iOld, jOld), new Vector2(iOld - 1, jOld), new Vector2(iOld, jOld - 1), new Vector2(iOld - 1, jOld - 1) };
                            float[] DP = { DotProduct(DistanceFromEachPoint[0], Grad[k, l]), DotProduct(DistanceFromEachPoint[1], Grad[k + 1, l]), DotProduct(DistanceFromEachPoint[2], Grad[k, l + 1]), DotProduct(DistanceFromEachPoint[3], Grad[k + 1, l + 1]) };
                            float[] LerpOne = { DP[0] + (DP[1] - DP[0]) * iNew, DP[2] + (DP[3] - DP[2]) * iNew };
                            float LerpTwo = LerpOne[0] + (LerpOne[1] - LerpOne[0]) * jNew;
                            Vector2 index = new Vector2(i + (k * widthOfCell), j + (l * heightOfCell));

                            perlin[(int)index.X, (int)index.Y] = (LerpTwo + 1) * 0.5f;
                            perlin2[(int)index.X, (int)index.Y] = LerpTwo;

                            if (perlin[(int)index.X, (int)index.Y] < Threshold)
                            {
                                perlinBinary[(int)index.X, (int)index.Y] = 1;
                            }
                            else
                            {
                                perlinBinary[(int)index.X, (int)index.Y] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
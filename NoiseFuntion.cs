using Microsoft.Xna.Framework;
using System.Drawing;
using Terraria;
using Terraria.Graphics.Shaders;

namespace EEMod
{
    public class NoiseFunction
    {
        Vector2 sampleSize;
        float division;
        Bitmap myBitmap = new Bitmap($@"{Main.SavePath}\Mod Sources\EEMod\noise2.png");
        public Vector2 middle;
        public NoiseFunction(Vector2 sampleSize, float division)
        {
            this.sampleSize = sampleSize;
            this.division = division;
            middle = sampleSize * 0.5f;
        }

        public int[,] ArrayOfSamples()
        {
            int[,] Array = new int[(int)sampleSize.X, (int)sampleSize.Y];
            for (int i = 0; i < sampleSize.X; i++)
            {
                for (int j = 0; j < sampleSize.Y; j++)
                {
                    bool gradientFunction = myBitmap.GetPixel(i, j).R > (int)(division * 255) ? true : false;
                    Array[i, j] = gradientFunction ? 1 : 0;
                }
            }
            return Array;
        }
    }
}

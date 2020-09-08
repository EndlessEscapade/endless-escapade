using Microsoft.Xna.Framework;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class NoiseFunction
    {
        private Vector2 _sampleSize;
        private readonly float _division;
        private readonly Bitmap _myBitmap; //= new Bitmap($@"{Main.SavePath}\Mod Sources\EEMod\noise2.png");

        public Vector2 middle;

        public NoiseFunction(Vector2 sampleSize, float division)
        {
            _sampleSize = sampleSize;
            _division = division;
            middle = sampleSize * 0.5f;
            using (MemoryStream ms = new MemoryStream())
            {
                ImageIO.RawToPng(EEMod.instance.GetFileStream("noise2.rawimg"), ms); // tmod stores images as .rawimg
                _myBitmap = new Bitmap(ms);
                ms.Close();
            }
        }

        public int[,] ArrayOfSamples()
        {
            int[,] Array = new int[(int)_sampleSize.X, (int)_sampleSize.Y];

            for (int i = 0; i < _sampleSize.X; i++)
            {
                for (int j = 0; j < _sampleSize.Y; j++)
                {
                    Array[i, j] = _myBitmap.GetPixel(i, j).R > (int)(_division * 255) ? 1 : 0;
                }
            }

            return Array;
        }
    }
}
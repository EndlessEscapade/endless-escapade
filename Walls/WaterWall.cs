using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Walls
{
    public class WaterWall : ModWall
	{
		public override void SetDefaults()
		{
            AddMapEntry(new Color(0, 0, 200));
            Main.wallHouse[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
        public override void AnimateWall(ref byte frame, ref byte frameCounter)
        {
            if (frame >= 9)
            {
                frame = 0;
                frameCounter = 0;
            }
            frameCounter++;
            if (frameCounter >= 10)
            {
                frameCounter = 0;
                frame++;
            }
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}

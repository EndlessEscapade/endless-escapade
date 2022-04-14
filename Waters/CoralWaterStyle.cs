using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Waters
{
    public class CoralWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle()
            => ModContent.GetInstance<CoralWaterfallStyle>().Slot;

        public override int GetDropletGore()
            => -1;

        public override int GetSplashDust()
            => DustID.BlueCrystalShard;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1.09f;
            g = 1.09f;
            b = 1.09f;
        }
    }
}
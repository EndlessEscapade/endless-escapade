using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Waters
{
    public class CoralWaterStyle : ModWaterStyle
    {
        /*public override bool ChooseWaterStyle()
            => Main.worldName == "CoralReefs";*/

        public override int ChooseWaterfallStyle()
            //=> Mod.GetWaterfallStyleSlot("CoralWaterfallStyle");
            => 0;

        public override int GetSplashDust()
            => DustID.BlueCrystalShard;

        public override int GetDropletGore()
            //=> Mod.GetGoreSlot("Gores/ExampleDroplet");
            => 0;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1.09f;
            g = 1.09f;
            b = 1.09f;
        }
    }
}
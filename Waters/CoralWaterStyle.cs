using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Waters
{
    public class CoralWaterStyle : ModWaterStyle
    {
        public override bool ChooseWaterStyle()
            => Main.worldName == "CoralReefs";

        public override int ChooseWaterfallStyle()
            => mod.GetWaterfallStyleSlot("CoralWaterfallStyle");

        public override int GetSplashDust()
            => DustID.BlueCrystalShard;

        public override int GetDropletGore()
            => mod.GetGoreSlot("Gores/ExampleDroplet");

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }
    }
}
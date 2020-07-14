
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Waters
{
    public class CoralWaterStyle : ModWaterStyle
    {
        public override bool ChooseWaterStyle()
            => Main.bgStyle == mod.GetSurfaceBgStyleSlot("");

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

        public override Color BiomeHairColor()
            => Color.White;
    }
}

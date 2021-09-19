using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Backgrounds
{
    public class EEBestiaryBiome : ModBiome
    {
        public override string BestiaryIcon => "icon_small";
        public override string BackgroundPath => "Backgrounds/EEBestiaryBackground";

        public virtual Color? BackgroundColor => null;
    }
}
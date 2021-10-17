using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Waters;
using EEMod.ID;

namespace EEMod.Backgrounds
{
    public class CoralReefs : ModBiome
    {
        public override string BestiaryIcon => "icon_small";
        public override string BackgroundPath => "Backgrounds/EEBestiaryBackground";

        public override Color? BackgroundColor => null;

        public override ModWaterStyle WaterStyle => ModContent.GetInstance<CoralWaterStyle>();

        public override bool IsBiomeActive(Player player) => Main.ActiveWorldFileData.Name == KeyID.CoralReefs;

        public override int Music => MusicLoader.GetMusicSlot("EEMod/Sounds/Music/SurfaceReefs");
    }
}
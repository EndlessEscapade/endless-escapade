using Terraria;
using Terraria.ModLoader;

namespace EEMod.Backgrounds
{
    public class CoralReefsUnderground : ModUgBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.LocalPlayer.GetModPlayer<EEPlayer>().ZoneCoralReefs;
        }

        public override void FillTextureArray(int[] textureSlots)
        {
            textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/CoralReefsUnderground");
            textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/CoralReefsUnderground");
            textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/CoralReefsUnderground");
            textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/CoralReefsUnderground");
        }
    }
}
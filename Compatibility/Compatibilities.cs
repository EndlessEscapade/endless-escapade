using Terraria.ModLoader;
using EEMod.Autoloading;

namespace EEMod.Compatibility
{
    /// <summary>
    /// For mods that need constant access in code, mods like bosschecklist aren't needed here since it only needs 1 access.
    /// </summary>
	public static partial class Compatibilities
    {
        public static Mod EEMusic;
        public static Mod CalamityMod;
        public static Mod SpiritMod;
        public static Mod OrchidMod;

        [LoadingMethod]
        public static void LoadModFields()
        {
            EEMusic = ModLoader.GetMod("EEModMusic");
            CalamityMod = ModLoader.GetMod("CalamityMod");
            SpiritMod = ModLoader.GetMod("SpiritMod");
            OrchidMod = ModLoader.GetMod("OrchidMod");
        }

        [UnloadingMethod]
        public static void Unload()
        {
            EEMusic = null;
            CalamityMod = null;
            SpiritMod = null;
            OrchidMod = null;
        }
    }
}
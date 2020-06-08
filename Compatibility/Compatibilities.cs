using Terraria.ModLoader;
using EEMod.Autoloading;
using System;

namespace EEMod.Compatibility
{
    /// <summary>
    /// For mods that need constant access in code, mods like bosschecklist aren't needed here since it only needs 1 access.
    /// </summary>
	public static partial class Compatibilities
    {
        public static Mod InteritosMusic;
        public static Mod CalamityMod;
        public static Mod SpiritMod;
        public static Mod OrchidMod;

        [Loading]
        public static void LoadModFields()
        {
            InteritosMusic = ModLoader.GetMod("InteritosModMusic");
            CalamityMod = ModLoader.GetMod("CalamityMod");
            SpiritMod = ModLoader.GetMod("SpiritMod");
            OrchidMod = ModLoader.GetMod("OrchidMod");
        }

        [Unloading]
        public static void Unload()
        {
            InteritosMusic = null;
            CalamityMod = null;
            SpiritMod = null;
            OrchidMod = null;
        }
    }
}
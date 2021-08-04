using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class ThermalHealing : EEBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Thermal Healing");
            Description.SetDefault("The warmth of the thermal vents heals you");
            Main.buffNoSave[Type] = true;
        }
    }
}
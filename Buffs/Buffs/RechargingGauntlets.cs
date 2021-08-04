using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class RechargingGauntlets : EEBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Recharging Gauntlets");
            Description.SetDefault("Your Granite Gauntlets are recharging");
            Main.buffNoSave[Type] = true;
        }
    }
}
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class BabyHydrosBuff : EEBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Hydros");
            Description.SetDefault("The Baby Hydros fights for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
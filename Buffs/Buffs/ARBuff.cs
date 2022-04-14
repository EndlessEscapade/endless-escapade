using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class ARBuff : EEBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquatic Reinforcment");
            Description.SetDefault("Pog");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
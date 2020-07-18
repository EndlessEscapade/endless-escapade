using Terraria;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Buffs.Buffs
{
    public class ARBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Aquatic Reinforcment");
            Description.SetDefault("Pog");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}

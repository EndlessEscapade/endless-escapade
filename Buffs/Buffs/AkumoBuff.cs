using Terraria;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Buffs.Buffs
{
    public class AkumoBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Baby Akumo");
            Description.SetDefault("The Baby Akumo fights for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}

using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class ClamBuff : EEBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clam");
            Description.SetDefault("The small clam fights for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class LobsterbikeBuff : EEBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lobsterbike");
            Description.SetDefault("Only for REAL men B)");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.Lobsterbike>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
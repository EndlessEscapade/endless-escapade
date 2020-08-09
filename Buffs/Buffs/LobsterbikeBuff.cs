using Terraria;
using Terraria.ModLoader;
using EEMod.Mounts;

namespace EEMod.Buffs.Buffs
{
    public class LobsterbikeBuff : ModBuff
    {
        public override void SetDefaults()
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

using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class SurfboardBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Surfboard");
            Description.SetDefault("Surf's up, duuuude!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.Surfboard>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
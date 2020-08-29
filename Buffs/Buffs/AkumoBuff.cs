using Terraria;
using Terraria.ModLoader;
using EEMod.Projectiles.Summons;

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

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AkumoMinion>()] > 0)
                player.buffTime[buffIndex] = 18000;
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}

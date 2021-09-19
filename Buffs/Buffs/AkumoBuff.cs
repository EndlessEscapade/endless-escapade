using EEMod.Items.Weapons.Summon.Minions;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class AkumoBuff : EEBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Akumo");
            Description.SetDefault("The Baby Akumo fights for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AkumoMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
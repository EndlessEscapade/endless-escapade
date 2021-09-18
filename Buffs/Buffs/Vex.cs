using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class Vex : EEBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Vex");
            Description.SetDefault("+5% damage and +5% crit chance");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetCritChance(DamageClass.Ranged) += 5;
        }
    }
}
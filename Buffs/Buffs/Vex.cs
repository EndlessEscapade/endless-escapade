using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Buffs
{
    public class Vex : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Vex");
            Description.SetDefault("+5% damage and +5% crit chance");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.magicCrit += 5;
            player.meleeCrit += 5;
            player.rangedCrit += 5;
        }
    }
}
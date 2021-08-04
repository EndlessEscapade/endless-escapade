using Terraria;
using Terraria.ModLoader;

namespace EEMod.Buffs.Debuffs
{
    public class WaterPressure : EEBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Water Pressure");
            Description.SetDefault("The water pressure is crushing in");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLife -= 2;
        }
    }
}
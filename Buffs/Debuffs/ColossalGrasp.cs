using Terraria;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Buffs.Debuffs
{
    public class ColossalGrasp : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Colossal Grasp");
            Description.SetDefault("The Kraken takes hold of you");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 25;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 25;
        }
    }
}

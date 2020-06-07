using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InteritosMod.Buffs.Debuffs
{
    public class MagmaticFlames : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Magmatic Flames");
            Description.SetDefault("The blood of the Precursors is burning you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // player.GetModPlayer<InteritosPlayer>().magmaticFlames = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // npc.GetGlobalNPC<InteritosPlayer>().magmaticFlames = true;
        }
    }
}

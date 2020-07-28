using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class Cococritter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cococritter");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {

            npc.width = 32;
            npc.height = 58;
            npc.damage = 12;
            npc.defense = 4;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath33;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.alpha = 20;
            npc.behindTiles = true;
        }
        public override void FindFrame(int frameHeight)
        {

        }
        public override void AI()
        {
            
        }
    }
}


using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class BigChungus : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Chungus");
        }

        public override void SetDefaults()
        {
            npc.width = 360;
            npc.height = 400;
            npc.damage = 12;
            npc.defense = 4;
            npc.lifeMax = 1000000000;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath33;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.alpha = 20;
            npc.spriteDirection = -1;
        }
    }
}


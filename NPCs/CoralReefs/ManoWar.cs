using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InteritosMod.NPCs.CoralReefs
{
    public class ManoWar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Man o War");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 18;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 20;

            npc.lifeMax = 38;
            npc.defense = 2;

            npc.buffImmune[BuffID.Confused] = true;

            npc.width = 22;
            npc.height = 50;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }

        private int variation = Main.rand.Next(3);
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 50 * variation;
        }
    }
}
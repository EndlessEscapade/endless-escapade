using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InteritosMod.NPCs.CoralReefs
{
    public class LunaJelly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luna Jelly");
            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.friendly = true;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 20;

            npc.lifeMax = 5;

            npc.width = 18;
            npc.height = 30;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }

        private int variation = Main.rand.Next(3);
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 30 * variation;
        }
    }
}
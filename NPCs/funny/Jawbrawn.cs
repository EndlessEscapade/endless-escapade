using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.funny
{
    public class Jawbrawn : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jawbrawn");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 5)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 4)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.friendly = false;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 80000;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.height = 164;
            npc.width = 114;
            npc.noGravity = true;
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.rotation = (target.Center - npc.Center).ToRotation() - MathHelper.PiOver2;
        }
    }
}
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.EEWorld;
using Terraria.ID;

namespace EEMod.NPCs.CoralReefs
{
    internal class Nautilus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 5)
            {
                npc.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 4)
                {
                    frameNumber = 0;
                }
                npc.frame.Y = frameNumber * 84;
            }
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 34;
            npc.height = 134;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
            npc.spriteDirection = -1;
        }

        public override void AI()
        {
            npc.ai[2]++;
            if(npc.ai[2] >= 360)
            {
                npc.ai[2] = 0;
                npc.ai[0] = Main.rand.Next(0, 2);
            }
            Vector2 closestPlayer = new Vector2();
            for (int i = 0; i < Main.player.Length; i++)
                if (Vector2.Distance(Main.player[i].Center, npc.Center) <= Vector2.Distance(closestPlayer, npc.Center))
                    closestPlayer = Main.player[i].Center;
            if (npc.ai[0] == 0)
            {
                if (Vector2.Distance(closestPlayer, npc.Center) <= 640)
                {
                    if (npc.ai[1] < 8)
                        npc.ai[1] *= 1.02f;
                    if (npc.ai[1] < 1)
                        npc.ai[1] = 1;
                }
                else
                    npc.ai[1] *= 0.95f;
                npc.velocity = Vector2.Normalize(closestPlayer - npc.Center) * npc.ai[1];
            }
            if (npc.ai[0] == 1)
            {
                npc.ai[1] *= 0.95f;
                npc.velocity = Vector2.Normalize(closestPlayer - npc.Center) * npc.ai[1];
            }
            npc.rotation = npc.velocity.ToRotation() + MathHelper.Pi;
        }
    }
}
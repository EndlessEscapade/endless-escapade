using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class GiantSquid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Squid");
            Main.npcFrameCount[npc.type] = 3;
        }

        // private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 3)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 20;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 34;
            npc.height = 134;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.ai[0]++;
            if (npc.ai[0] >= 100)
            {
                npc.velocity += Vector2.Normalize(target.Center - npc.Center) * 10;
                npc.ai[0] = 0;
            }
            npc.velocity *= 0.99f;

            npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver2;

            if (npc.life <= npc.lifeMax / 2)
            {
                if ((Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) / 2 >= 3)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.Smoke, newColor: Color.Black);
                }
            }
        }
    }
}
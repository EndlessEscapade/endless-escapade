using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class GiantSquid : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Squid");
            Main.npcFrameCount[NPC.type] = 3;
        }

        // private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 6)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 3)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 20;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 34;
            NPC.height = 134;

            NPC.noGravity = true;

            NPC.lavaImmune = false;
            NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            NPC.ai[0]++;
            if (NPC.ai[0] >= 100)
            {
                NPC.velocity += Vector2.Normalize(target.Center - NPC.Center) * 10;
                NPC.ai[0] = 0;
            }
            NPC.velocity *= 0.99f;

            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

            if (NPC.life <= NPC.lifeMax / 2)
            {
                if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) / 2 >= 3)
                {
                    Dust.NewDust(NPC.Center, 0, 0, DustID.Smoke, newColor: Color.Black);
                }
            }
        }
    }
}
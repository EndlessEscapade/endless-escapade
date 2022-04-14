using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.LowerReefs
{
    public class Lionfish : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lionfish");
            Main.npcFrameCount[NPC.type] = 8;
        }

        //int frameHeight = 130;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[2] == 1)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter == 3)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0;
                }
                if (NPC.frame.Y >= frameHeight * 8)
                {
                    NPC.frame.Y = 0;
                    return;
                }
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 34;
            NPC.height = 134;

            NPC.noGravity = true;

            NPC.buffImmune[BuffID.Confused] = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            NPC.TargetClosest();
            NPC.ai[0]++;
            if (NPC.ai[0] % 120 == 0)
            {
                NPC.ai[2] = 1;
                NPC.velocity = Vector2.Normalize(Main.player[NPC.target].position - NPC.Center) * 4;
            }
            if ((NPC.ai[0] + 96) % 120 == 0)
            {
                NPC.ai[2] = 0;
            }

            if (Main.player[NPC.target].position.X < NPC.Center.X)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
            }

            NPC.velocity *= 0.99f;
            NPC.rotation = NPC.velocity.X / 16;
        }
    }
}
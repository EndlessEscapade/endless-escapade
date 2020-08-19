using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    public class Lionfish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lionfish");
            Main.npcFrameCount[npc.type] = 8;
        }

        //int frameHeight = 130;
        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[2] == 1)
            {
                npc.frameCounter++;
                if (npc.frameCounter == 3)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y >= frameHeight * 8)
                {
                    npc.frame.Y = 0;
                    return;
                }
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
        }

        public override void AI()
        {
            npc.TargetClosest();
            npc.ai[0]++;
            if (npc.ai[0] % 120 == 0)
            {
                npc.ai[2] = 1;
                npc.velocity = Vector2.Normalize(Main.player[npc.target].position - npc.Center) * 4;
            }
            if ((npc.ai[0] + 96) % 120 == 0)
                npc.ai[2] = 0;
            if (Main.player[npc.target].position.X < npc.Center.X)
                npc.spriteDirection = 1;
            else
                npc.spriteDirection = -1;
            npc.velocity *= 0.99f;
            npc.rotation = npc.velocity.X / 16;
        }
    }
}
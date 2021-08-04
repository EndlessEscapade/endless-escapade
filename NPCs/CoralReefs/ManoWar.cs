using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class ManoWar : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Man o War");
            Main.npcFrameCount[NPC.type] = 3;
            //bannerItem = ModContent.ItemType<Items.Banners.ManoWarBanner>();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 127;

            NPC.lifeMax = 38;
            NPC.defense = 2;

            NPC.buffImmune[BuffID.Confused] = true;

            NPC.width = 22;
            NPC.height = 50;

            NPC.noGravity = true;

            NPC.lavaImmune = false;
            NPC.noTileCollide = false;
        }

        private readonly int variation = Main.rand.Next(3);

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 50 * variation;
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, 0.2f, 0.8f, 1.4f);
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
        }
    }
}
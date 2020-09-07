using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class ManoWar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Man o War");
            Main.npcFrameCount[npc.type] = 3;
            bannerItem = ModContent.ItemType<Items.Banners.ManoWarBanner>();
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 127;

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

        public override void AI()
        {
            Lighting.AddLight(npc.Center, 0.2f, 0.8f, 1.4f);
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.ai[0]++;
            if (npc.ai[0] >= 100)
            {
                npc.velocity += Vector2.Normalize(target.Center - npc.Center) * 10;
                npc.ai[0] = 0;
            }
            npc.velocity *= 0.99f;

            npc.rotation = npc.velocity.ToRotation() + MathHelper.Pi / 2;
        }
    }
}
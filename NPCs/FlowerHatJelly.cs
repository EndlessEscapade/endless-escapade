using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class FlowerHatJelly : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flower Hat Jelly");
            //bannerItem = ModContent.ItemType<Items.Banners.LunaJellyBanner>();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.friendly = true;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            //npc.alpha = 127;

            NPC.lifeMax = 5;

            NPC.width = 18;
            NPC.height = 30;

            NPC.noGravity = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, 0.2f, 0.4f, 1.4f);
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            NPC.ai[0]++;
            if (target.Center.Y > NPC.Center.Y)
            {
                if (NPC.velocity.Y < 2)
                {
                    NPC.velocity.Y *= 1.01f;
                }

                if (NPC.velocity.Y <= 0)
                {
                    NPC.velocity.Y += 0.5f;
                }
            }
            else
            {
                if (NPC.ai[0] >= 120)
                {
                    NPC.velocity.Y -= 2;
                    NPC.velocity.X += Helpers.Clamp((target.Center.X - NPC.Center.X) / 10, -2, 2);
                    NPC.ai[0] = 0;
                }
                NPC.velocity *= 0.97f;
            }
        }
    }
}
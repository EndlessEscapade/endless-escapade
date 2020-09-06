using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.CoralReefs
{
    public class FlowerHatJelly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flower Hat Jelly");
            //bannerItem = ModContent.ItemType<Items.Banners.LunaJellyBanner>();
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.friendly = true;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            //npc.alpha = 127;

            npc.lifeMax = 5;

            npc.width = 18;
            npc.height = 30;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(npc.Center, 0.2f, 0.4f, 1.4f);
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.ai[0]++;
            if (target.Center.Y > npc.Center.Y)
            {
                if (npc.velocity.Y < 2)
                    npc.velocity.Y *= 1.01f;
                if (npc.velocity.Y <= 0)
                    npc.velocity.Y += 0.5f;
            }
            else
            {
                if (npc.ai[0] >= 120)
                {
                    npc.velocity.Y -= 2;
                    npc.velocity.X += Helpers.Clamp((target.Center.X - npc.Center.X) / 10, -2, 2);
                    npc.ai[0] = 0;
                }
                npc.velocity *= 0.97f;
            }
        }
    }
}
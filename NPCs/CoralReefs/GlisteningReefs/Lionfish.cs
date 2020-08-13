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

        float speed = 0.5f;
        public override void AI()
        {
            Vector2 closestPlayer = new Vector2();
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Vector2.DistanceSquared(Main.player[i].Center, npc.Center) <= Vector2.DistanceSquared(closestPlayer, npc.Center))
                {
                    closestPlayer = Main.player[i].Center;
                }
            }
            if (Vector2.DistanceSquared(closestPlayer, npc.Center) <= 640 * 640)
            {
                npc.velocity = Vector2.Normalize(closestPlayer - npc.Center) * speed;
            }
            else
            {
                npc.ai[0]++;
                if (npc.ai[0] >= 180)
                {
                    npc.velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * speed;
                    npc.ai[0] = 0;
                }
            }
            if (closestPlayer.X < npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }
        }
    }
}
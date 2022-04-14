using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace EEMod.NPCs.TropicalIslands
{
    public class Cococritter : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coco Critter");
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.lifeMax = 5;
            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            NPC.height = 29;
            NPC.width = 24;
        }

        public override void AI()
        {
            Animate(4, false);
            NPC.velocity.X = NPC.ai[1];
            if (NPC.ai[0] == 0)
            {
                NPC.ai[1] = 1;
            }

            NPC.ai[0]++;
            if (NPC.ai[0] % 180 == 0 && Helpers.OnGround(NPC))
            {
                NPC.velocity.Y -= 5;
                if (Helpers.isCollidingWithWall(NPC))
                {
                    if (NPC.ai[1] == -1)
                    {
                        NPC.ai[1] = 1;
                    }
                    else
                    {
                        NPC.ai[1] = -1;
                    }
                }
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            item.stack = 2;
        }

        public void Animate(int delay, bool flip)
        {
            Player player = Main.player[NPC.target];
            if (flip)
            {
                if (player.Center.X - NPC.Center.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.spriteDirection = -1;
                }
            }
            if (NPC.frameCounter++ > delay)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = NPC.frame.Y + (TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type]);
            }
            if (NPC.frame.Y >= TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] * (Main.npcFrameCount[NPC.type] - 1))
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void FindFrame(int frameHeight)
        {
        }
    }
}
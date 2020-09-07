using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class Cococritter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coco Critter");
            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 5;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.height = 29;
            npc.width = 24;
        }

        public override void AI()
        {
            Animate(4, false);
            npc.velocity.X = npc.ai[1];
            if (npc.ai[0] == 0)
                npc.ai[1] = 1;
            npc.ai[0]++;
            if (npc.ai[0] % 180 == 0 && Helpers.OnGround(npc))
            {
                npc.velocity.Y -= 5;
                if (Helpers.isCollidingWithWall(npc))
                {
                    if (npc.ai[1] == -1)
                    {
                        npc.ai[1] = 1;
                    }
                    else
                    {
                        npc.ai[1] = -1;
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

            try
            {
                var npcCenter = npc.Center.ToTileCoordinates();
                Tile tile = Main.tile[npcCenter.X, npcCenter.Y];
                if (!WorldGen.SolidTile(npcCenter.X, npcCenter.Y) && tile.liquid == 0)
                {
                    tile.liquid = (byte)Main.rand.Next(50, 150);
                    tile.lava(true);
                    tile.honey(false);
                    WorldGen.SquareTileFrame(npcCenter.X, npcCenter.Y, true);
                }
            }
            catch
            {
                return;
            }
        }

        public void Animate(int delay, bool flip)
        {
            Player player = Main.player[npc.target];
            if (flip)
            {
                if (player.Center.X - npc.Center.X > 0)
                {
                    npc.spriteDirection = 1;
                }
                else
                    npc.spriteDirection = -1;
            }
            if (npc.frameCounter++ > delay)
            {
                npc.frameCounter = 0;
                npc.frame.Y = npc.frame.Y + (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]);
            }
            if (npc.frame.Y >= (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]) * (Main.npcFrameCount[npc.type] - 1))
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void FindFrame(int frameHeight)
        {
        }
    }
}
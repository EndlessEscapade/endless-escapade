using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Grebyser : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grebyser");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 32;
            npc.height = 32;

            npc.noGravity = false;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            npc.velocity.X = npc.ai[1];
            if (npc.ai[0] == 0)
            {
                npc.ai[1] = 1;
            }

            npc.ai[0]++;
            if (npc.ai[0] % 180 == 0 && Helpers.OnGround(npc))
            {
                if (npc.ai[0] >= 600)
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

            npc.ai[2]++;
            if (npc.ai[2] >= 300)
            {
                Projectile.NewProjectile(npc.Center + new Vector2(0, -16), new Vector2(0, -5), ProjectileID.GeyserTrap, 20, 2f);
                npc.ai[2] = 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 5)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 3)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(TextureCache.GrebyserGlow, npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
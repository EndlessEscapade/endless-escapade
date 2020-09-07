using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class MechanicalEel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Eel");
            //Main.npcFrameCount[npc.type] = 6;
        }

        /*public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 6)
            {
                npc.frame.Y = 0;
                return;
            }
        }*/

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 178;
            npc.height = 40;

            npc.noGravity = true;

            npc.spriteDirection = 1;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        private Vector2 oldPlayerPos = new Vector2();

        public override void AI()
        {
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.ai[0]++;
            if (npc.ai[1] == 1)
            {
                Vector2 origin = npc.Center;
                float radius = 96;
                int numLocations = 30;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i + Main.rand.Next(0, 60))) * (radius + Main.rand.Next(-2, 3));
                    Dust dust = Dust.NewDustPerfect(position, 111);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                    dust.noLight = false;
                    dust.fadeIn = 1f;
                }
                npc.velocity *= 0.96f;
                npc.rotation = (target.Center - npc.Center).ToRotation(); // a vector keeps the rotation when normalized, so normalizing it is not needed

                if (npc.ai[0] >= 300)
                {
                    npc.ai[1] = 0;
                    npc.ai[0] = 100;
                }

                if (Vector2.DistanceSquared(target.Center, npc.Center) <= 640 * 640)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 position = Vector2.Lerp(target.Center, npc.Center, i / 50f);
                        Dust dust = Dust.NewDustPerfect(position, 111);
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                        dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                    target.AddBuff(BuffID.Electrified, 61);
                }
            }
            if (npc.ai[1] == 0)
            {
                npc.velocity *= 0.99f;
                if (npc.ai[0] >= 100)
                {
                    npc.velocity += Vector2.Normalize(target.Center - npc.Center) * 10;
                    npc.ai[0] = 0;
                    if (Main.rand.NextBool(3))
                        npc.ai[1] = 1;
                }
                npc.rotation = npc.velocity.ToRotation();
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(TextureCache.MechanicalEelGlow, npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
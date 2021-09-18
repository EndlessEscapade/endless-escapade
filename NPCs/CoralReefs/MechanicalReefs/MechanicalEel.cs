using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class MechanicalEel : EENPC
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
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 178;
            NPC.height = 40;

            NPC.noGravity = true;

            NPC.spriteDirection = 1;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        //private Vector2 oldPlayerPos = new Vector2(); //unused?

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            NPC.ai[0]++;
            if (NPC.ai[1] == 1)
            {
                Vector2 origin = NPC.Center;
                float radius = 96;
                int numLocations = 30;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i + Main.rand.Next(0, 60))) * (radius + Main.rand.Next(-2, 3));
                    Dust dust = Dust.NewDustPerfect(position, 111);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                    // dust.noLight = false;
                    dust.fadeIn = 1f;
                }
                NPC.velocity *= 0.96f;
                NPC.rotation = (target.Center - NPC.Center).ToRotation(); // a vector keeps the rotation when normalized, so normalizing it is not needed

                if (NPC.ai[0] >= 300)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[0] = 100;
                }

                if (Vector2.DistanceSquared(target.Center, NPC.Center) <= 640 * 640)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 position = Vector2.Lerp(target.Center, NPC.Center, i / 50f);
                        Dust dust = Dust.NewDustPerfect(position, 111);
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                        // dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                    target.AddBuff(BuffID.Electrified, 61);
                }
            }
            if (NPC.ai[1] == 0)
            {
                NPC.velocity *= 0.99f;
                if (NPC.ai[0] >= 100)
                {
                    NPC.velocity += Vector2.Normalize(target.Center - NPC.Center) * 10;
                    NPC.ai[0] = 0;
                    if (Main.rand.NextBool(3))
                    {
                        NPC.ai[1] = 1;
                    }
                }
                NPC.rotation = NPC.velocity.ToRotation();
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/CoralReefs/MechanicalReefs/MechanicalEelGlow").Value, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    internal class Nautilus : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 4)
                {
                    frameNumber = 0;
                }
                NPC.frame.Y = frameNumber * 84;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 34;
            NPC.height = 134;

            NPC.noGravity = true;

            NPC.buffImmune[BuffID.Confused] = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            NPC.ai[0]++;
            NPC.velocity *= 0.98f;
            NPC.rotation = NPC.velocity.X / 32f;
            if (NPC.ai[0] % 100 > 60 && NPC.ai[1] == 0)
            {
                Helpers.Move(NPC, player, 13, 30, Vector2.Zero, true, -1);
            }
            if (NPC.ai[0] % 450 == 0)
            {
                if (NPC.life < NPC.lifeMax * 0.5f)
                {
                    NPC.ai[1] = Main.rand.Next(1, 3);
                }
                else
                {
                    NPC.ai[1] = 1;
                }

                NPC.ai[0] = 0;
                NPC.netUpdate = true;
            }
            if (NPC.ai[1] == 1)
            {
                if (NPC.ai[0] <= 200)
                {
                    NPC.velocity.X = (float)Math.Sin(NPC.ai[0] / 10) * 10;
                    NPC.velocity.Y = (float)Math.Cos(NPC.ai[0] / 10) * 10;
                    Dust.NewDustPerfect(NPC.Center, 113, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)));
                }
                else
                {
                    if (NPC.ai[0] < 300)
                    {
                        Dust.NewDustPerfect(NPC.Center, 113, Vector2.Zero + new Vector2((float)Math.Sin(NPC.ai[0] / 10), (float)Math.Cos(NPC.ai[0] / 10)));
                        if (NPC.ai[0] < 215)
                        {
                            Helpers.Move(NPC, player, 60, 200, Vector2.Zero, true, -1);
                        }
                    }
                }
                if (NPC.ai[0] >= 400)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                }
            }
            if (NPC.ai[1] == 2)
            {
                float rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDustPerfect(NPC.Center + new Vector2(-(float)Math.Sin(NPC.ai[0] / 30) * 100, 0).RotatedBy(rotation), 113, new Vector2((float)Math.Sin(rotation) * 0.4f, (float)Math.Cos(rotation) * 0.4f), 255 * (int)(NPC.velocity.X / 10f));
                }

                if (NPC.ai[0] % 10 == 0)
                {
                    NPC.life += 2;
                    NPC.HealEffect(2);
                }
                if (NPC.lifeMax - NPC.life < 20)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (NPC.ai[0] % 100 > 60 || NPC.ai[1] == 1)
            {
                AfterImage.DrawAfterimage(spriteBatch, Terraria.GameContent.TextureAssets.Npc[NPC.type].Value, 0, NPC, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }

            return true;
        }
    }
}
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.EEWorld;
using System;
using Terraria.ID;
namespace EEMod.NPCs.CoralReefs
{
    internal class Nautilus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 5)
            {
                npc.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 4)
                {
                    frameNumber = 0;
                }
                npc.frame.Y = frameNumber * 84;
            }
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
        }
        private void Move(Player player, float sped, float TR, Vector2 addOn)
        {
            Vector2 moveTo = player.Center + addOn;
            float speed = sped;
            Vector2 move = moveTo - npc.Center;
            float magnitude = move.Length(); // (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TR;

            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            npc.velocity = move;
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            npc.ai[0]++;
            npc.velocity *= 0.98f;
            npc.rotation = npc.velocity.X / 32f;
            if(npc.ai[0] % 100 > 60 && npc.ai[1] == 0)
            {
                Helpers.Move(npc,player, 13, 30, Vector2.Zero,true,-1);
            }
            if(npc.ai[0] % 450 == 0)
            {
                if (npc.life < npc.lifeMax * 0.5f)
                    npc.ai[1] = Main.rand.Next(1, 3);
                else
                    npc.ai[1] = 1;
                npc.ai[0] = 0;
                npc.netUpdate = true;
            }
            if(npc.ai[1] == 1)
            {
                if (npc.ai[0] <= 200)
                {
                    npc.velocity.X = (float)Math.Sin(npc.ai[0] / 10) * 2;
                    npc.velocity.Y = (float)Math.Cos(npc.ai[0] / 10) * 2;
                    Dust.NewDustPerfect(npc.Center, 113, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat((float)Math.PI*2)));
                }
                else
                {
                    if (npc.ai[0] < 300)
                    {
                        Dust.NewDustPerfect(npc.Center, 113, Vector2.Zero + new Vector2((float)Math.Sin(npc.ai[0] / 10), (float)Math.Cos(npc.ai[0] / 10)));
                        if (npc.ai[0] < 215)
                            Helpers.Move(npc, player, 60, 50, Vector2.Zero,true,-1);
                    }
                }
                if(npc.ai[0] >= 400)
                {
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                }
            }
            if(npc.ai[1] == 2)
            {
                float rotation = Main.rand.NextFloat((float)Math.PI * 2);
                for(int i = 0; i<3; i++)
                Dust.NewDustPerfect(npc.Center + new Vector2(-(float)Math.Sin(npc.ai[0]/30)*100, 0).RotatedBy(rotation), 113, new Vector2((float)Math.Sin(rotation) * 0.4f, (float)Math.Cos(rotation) * 0.4f),255*(int)(npc.velocity.X/10f));
                if (npc.ai[0] % 10 == 0)
                {
                    npc.life += 2;
                    npc.HealEffect(2);
                }
                if(npc.lifeMax - npc.life < 20)
                {
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(npc.ai[0] % 100 > 60 || npc.ai[1] == 1)
            AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            return true;
        }
    }
}

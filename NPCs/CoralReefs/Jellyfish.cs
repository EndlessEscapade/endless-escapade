using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.NPCs.CoralReefs
{
    public class Jellyfish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellyfish");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.friendly = false;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            //npc.alpha = 127;

            npc.lifeMax = 300;

            npc.width = 48;
            npc.height = 32;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;

            npc.damage = 5;
        }
        float counter;
        public int cap = 15;
        public Vector2[,,] lol1;
        public int noOfTentacles = 8;
        float counter2;
        public Color drawColour;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            this.drawColour = drawColor;
            counter2 += 0.1f;
            Texture2D tex = Main.npcTexture[npc.type];
            Vector2 pos = npc.Center.ForDraw();
            Main.spriteBatch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, tex.Width + (int)(Math.Sin(counter2) * 2) - 5, tex.Height + (int)(Math.Cos(counter2) * 5) - 2), npc.frame, Color.Lerp(drawColor, Color.MediumPurple, (float)Math.Sin(counter2) * 0.2f), npc.rotation, npc.frame.Size() / 2, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        public void UpdateJellyfishTesting()
        {
            npc.rotation = npc.velocity.X / 16f;
            lol1 = new Vector2[noOfTentacles / 2, cap, 2];
            Vector2 first = npc.Center;
            float[] lastX = new float[noOfTentacles];
            float[] lastY = new float[noOfTentacles];
            float[] ControlY = new float[noOfTentacles];
            float[] ControlX = new float[noOfTentacles];
            float[] ControlY2 = new float[noOfTentacles];
            float[] ControlX2 = new float[noOfTentacles];
            float tip = first.Y + 120;
            int diff = 8;
            int startingdiff = 10;
            float firstContactPoint = tip - 50;
            float secondContactPoint = tip - 20;
            float accuracy = cap;
            float asnycPeriod = 0.7f;
            float tipVariation = 5;
            float accell = ((float)Math.Sin(counter) + 1.4f) / 2f;
            counter += 0.08f * accell;
            float rot = npc.velocity.X / 10f;
            for (int i = 0; i < noOfTentacles / 2; i++)
            {
                Vector2 firstControl = new Vector2(-startingdiff - i * diff - (float)Math.Sin(counter + 0.35f + i / 10f) * (startingdiff + i * diff), -(float)Math.Cos(counter + 0.05f) * (tip - secondContactPoint)).RotatedBy(rot);
                Vector2 secondControl = new Vector2(-startingdiff - i * diff / 2 - (float)Math.Sin(counter + 0.25f + i / 5f) * (startingdiff + i * diff / 2), -(float)Math.Cos(counter + 0.05f) * (tip - firstContactPoint)).RotatedBy(rot);
                Vector2 thidControl = new Vector2(-startingdiff - i * diff - (float)Math.Sin(counter + 0.15f) * (startingdiff + i * diff), +120 - 30 - (float)Math.Sin(counter + asnycPeriod + 0.05f + i / 20f) * tipVariation).RotatedBy(rot);
                ControlX[i] = first.X + firstControl.X;
                ControlY[i] = secondContactPoint + firstControl.Y;
                ControlX2[i] = first.X + secondControl.X;
                ControlY2[i] = firstContactPoint + secondControl.Y;
                lastX[i] = first.X + thidControl.X;
                lastY[i] = first.Y + thidControl.Y;
            }
            for (int i = noOfTentacles / 2; i < noOfTentacles; i++)
            {
                Vector2 firstControl = new Vector2(startingdiff + (i - noOfTentacles / 2) * diff + (float)Math.Sin(counter + i / 13f) * (startingdiff + (i - noOfTentacles / 2) * diff), -(float)Math.Cos(counter + i / 14f) * (tip - secondContactPoint)).RotatedBy(rot);
                Vector2 secondControl = new Vector2(startingdiff + (i - noOfTentacles / 2) * diff / 2 + (float)Math.Sin(counter + i / 15f) * (startingdiff + (i - noOfTentacles / 2) * diff / 2), -(float)Math.Cos(counter + i / 20f) * (tip - firstContactPoint)).RotatedBy(rot);
                Vector2 thidControl = new Vector2(startingdiff + (i - noOfTentacles / 2) * diff + (float)Math.Sin(counter + i / 10f - 0.1f) * (startingdiff + (i - noOfTentacles / 2) * diff), 120 - 30 - (float)Math.Sin(counter + asnycPeriod + i / 20f + 0.2f) * tipVariation).RotatedBy(rot);
                ControlX[i] = first.X + firstControl.X;
                ControlY[i] = secondContactPoint + firstControl.Y;
                ControlX2[i] = first.X + secondControl.X;
                ControlY2[i] = firstContactPoint + secondControl.Y;
                lastX[i] = first.X + thidControl.X;
                lastY[i] = first.Y + thidControl.Y;
            }
            int sep = 5;
            for (int i = 0; i < noOfTentacles; i++)
            {
                if (i < noOfTentacles / 2)
                {
                    for (int j = 0; j < accuracy; j++)
                    {
                        Vector2 yas = Helpers.TraverseBezier(new Vector2(lastX[i] - i * sep, lastY[i]), new Vector2(first.X - i * sep, first.Y), new Vector2(ControlX2[i] - i * sep, ControlY2[i]), new Vector2(ControlX[i] - i * sep, ControlY[i]), j / accuracy);
                        lol1[i, j, 0] = yas;
                    }
                }
                else
                {
                    for (int j = 0; j < accuracy; j++)
                    {
                        Vector2 yas = Helpers.TraverseBezier(new Vector2(lastX[i] + (i - noOfTentacles / 2) * sep, lastY[i]), new Vector2(first.X + (i - noOfTentacles / 2) * sep, first.Y), new Vector2(ControlX2[i] + (i - noOfTentacles / 2) * sep, ControlY2[i]), new Vector2(ControlX[i] + (i - noOfTentacles / 2) * sep, ControlY[i]), j / accuracy);
                        lol1[i - noOfTentacles / 2, j, 1] = yas;
                    }
                }
            }
        }
        public override void AI()
        {
            UpdateJellyfishTesting();
            Lighting.AddLight(npc.Center, 0.2f, 0.4f, 1.4f);
            npc.TargetClosest();
            Player target = Main.player[npc.target];
            if (counter % ((float)Math.PI * 2) < 0.5f)
            {
                Helpers.Move(npc, target, 18, 40, Vector2.Zero);
            }
            npc.velocity *= 0.98f;
            if (npc.ai[1] == 0)
            {
                EEMod.prims.CreateTrailWithNPC(null, npc);
                npc.ai[1] = 1;
            }
            npc.ai[0]++;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity += Vector2.Normalize(target.Center - npc.Center) * 6;
        }
    }
}
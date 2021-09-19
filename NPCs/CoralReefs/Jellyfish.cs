using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.NPCs.CoralReefs
{
    public class Jellyfish : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellyfish");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            // NPC.friendly = false;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            //npc.alpha = 127;

            NPC.lifeMax = 300;

            NPC.width = 48;
            NPC.height = 32;

            NPC.noGravity = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;

            NPC.damage = 5;
        }
        float counter;
        public int cap = 15;
        public Vector2[,,] lol1;
        public int noOfTentacles = 8;
        float counter2;
        public Color drawColour;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            this.drawColour = drawColor;
            counter2 += 0.1f;
            Texture2D tex = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 pos = NPC.Center.ForDraw();
            Main.spriteBatch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, tex.Width + (int)(Math.Sin(counter2) * 2) - 5, tex.Height + (int)(Math.Cos(counter2) * 5) - 2), NPC.frame, Color.Lerp(drawColor, Color.MediumPurple, (float)Math.Sin(counter2) * 0.2f), NPC.rotation, NPC.frame.Size() / 2, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        public void UpdateJellyfishTesting()
        {
            NPC.rotation = NPC.velocity.X / 16f;
            lol1 = new Vector2[noOfTentacles / 2, cap, 2];
            Vector2 first = NPC.Center;
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
            float rot = NPC.velocity.X / 10f;
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
            Lighting.AddLight(NPC.Center, 0.2f, 0.4f, 1.4f);
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (counter % ((float)Math.PI * 2) < 0.5f)
            {
                Helpers.Move(NPC, target, 18, 40, Vector2.Zero);
            }
            NPC.velocity *= 0.98f;
            if (NPC.ai[1] == 0)
            {
                //EEMod.prims.CreateTrailWithNPC(null, NPC);
                NPC.ai[1] = 1;
            }
            NPC.ai[0]++;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity += Vector2.Normalize(target.Center - NPC.Center) * 6;
        }
    }
}
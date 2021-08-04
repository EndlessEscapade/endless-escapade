using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    internal class Squid : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            DisplayName.SetDefault("Squid");
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

            NPC.lavaImmune = false;
            NPC.noTileCollide = false;
        }

        public override void AI()
        {
            UpdateJellyfishTesting();
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (counter % ((float)Math.PI * 2) < 0.5f)
            {
                Helpers.Move(NPC, target, 18, 40, Vector2.Zero);
            }
            NPC.velocity *= 0.99f;
            if (NPC.ai[1] == 0)
            {
                NPC.ai[1] = 1;
            }
            NPC.ai[0]++;
            NPC.rotation = NPC.velocity.X / 10f;
        }
        float counter;
        public int cap = 35;
        public Vector2[,,] lol1;
        public int noOfTentacles = 8;
        public Color drawColour;
        public void UpdateJellyfishTesting()
        {
            lol1 = new Vector2[noOfTentacles / 2, cap, 2];
            Vector2 first = NPC.Center;
            float[] lastX = new float[noOfTentacles];
            float[] lastY = new float[noOfTentacles];
            float[] ControlY = new float[noOfTentacles];
            float[] ControlX = new float[noOfTentacles];
            float[] ControlY2 = new float[noOfTentacles];
            float[] ControlX2 = new float[noOfTentacles];
            float tip = first.Y + 200;
            int diff = 20;
            int startingdiff = 10;
            float firstContactPoint = tip - 80;
            float secondContactPoint = tip - 170;
            float accuracy = cap;
            float asnycPeriod = 0.9f;
            float tipVariation = 10;
            float accell = ((float)Math.Sin(counter) + 1.4f) / 2f;
            counter += 0.07f * accell;
            float rot = NPC.velocity.X / 6f;
            for (int i = 0; i < noOfTentacles / 2; i++)
            {
                Vector2 firstControl = new Vector2(-startingdiff - i * diff - (float)Math.Sin(counter + 0.35f + i / 10f) * (startingdiff + i * diff), -(float)Math.Cos(counter + 0.05f) * (tip - secondContactPoint)).RotatedBy(rot);
                Vector2 secondControl = new Vector2(-startingdiff - i * diff / 2 - (float)Math.Sin(counter + 0.25f + i / 5f) * (startingdiff + i * diff / 2), -(float)Math.Cos(counter + 0.05f) * (tip - firstContactPoint)).RotatedBy(rot);
                Vector2 thidControl = new Vector2(-startingdiff - i * diff - (float)Math.Sin(counter + 0.15f) * (startingdiff + i * diff), +180 - 30 - (float)Math.Sin(counter + asnycPeriod + 0.05f + i / 20f) * tipVariation).RotatedBy(rot);
                ControlX[i] = first.X + firstControl.X;
                ControlY[i] = secondContactPoint + firstControl.Y;
                ControlX2[i] = first.X + secondControl.X;
                ControlY2[i] = firstContactPoint + secondControl.Y;
                lastX[i] = first.X + thidControl.X;
                lastY[i] = first.Y + thidControl.Y;
            }
            for (int i = noOfTentacles / 2; i < noOfTentacles; i++)
            {
                Vector2 firstControl = new Vector2(startingdiff + (i - noOfTentacles / 2) * diff + (float)Math.Sin(counter + i / 20f) * (startingdiff + (i - noOfTentacles / 2) * diff), -(float)Math.Cos(counter + i / 20f) * (tip - secondContactPoint)).RotatedBy(rot);
                Vector2 secondControl = new Vector2(startingdiff + (i - noOfTentacles / 2) * diff / 2 + (float)Math.Sin(counter + i / 20f) * (startingdiff + (i - noOfTentacles / 2) * diff / 2), -(float)Math.Cos(counter + i / 20f) * (tip - firstContactPoint)).RotatedBy(rot);
                Vector2 thidControl = new Vector2(startingdiff + (i - noOfTentacles / 2) * diff + (float)Math.Sin(counter + i / 20f) * (startingdiff + (i - noOfTentacles / 2) * diff), 180 - 30 - (float)Math.Sin(counter + asnycPeriod + i / 20f) * tipVariation).RotatedBy(rot);
                ControlX[i] = first.X + firstControl.X;
                ControlY[i] = secondContactPoint + firstControl.Y;
                ControlX2[i] = first.X + secondControl.X;
                ControlY2[i] = firstContactPoint + secondControl.Y;
                lastX[i] = first.X + thidControl.X;
                lastY[i] = first.Y + thidControl.Y;
            }
            int sep = 10;
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = mod.GetTexture("NPCs/CoralReefs/GlisteningReefs/SquidChain");
            Texture2D tex2 = mod.GetTexture("NPCs/CoralReefs/GlisteningReefs/SquidEnd");
            Texture2D tex3 = mod.GetTexture("NPCs/CoralReefs/GlisteningReefs/SquidHead");
            for (int i = 1; i < lol1.GetLength(0); i++)
            {
                for (int j = 1; j < lol1.GetLength(1); j++)
                {
                    for (int k = 0; k < lol1.GetLength(2); k++)
                    {
                        Texture2D texure = tex;
                        float rotation = (lol1[i, j, k].ForDraw() - lol1[i, j - 1, k].ForDraw()).ToRotation() + (float)Math.PI/2f;
                        if (j == lol1.GetLength(1) - 1)
                        {
                            Helpers.DrawAdditive(Helpers.RadialMask, lol1[i, j, k].ForDraw(),Color.Purple*0.3f,0.3f);
                            texure = tex2;
                        }
                        spriteBatch.Draw(texure, lol1[i, j, k].ForDraw(), texure.Bounds, drawColor, rotation, texure.TextureCenter(), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            spriteBatch.Draw(tex3,NPC.Center.ForDraw(),tex3.Bounds,drawColor,NPC.rotation, tex3.TextureCenter(),1f,SpriteEffects.None, 0f);
            return false;
        }
    }
}
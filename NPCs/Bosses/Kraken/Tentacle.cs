using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class Tentacle : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
        }

        public override void SetDefaults()
        {
            NPC.width = 184;
            NPC.height = 80;
            NPC.damage = 1;
            NPC.aiStyle = -1;
            NPC.lifeMax = 5000;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
        }

        private Vector2 startingPosition;
        private Vector2 distance;
        private bool isGrabbing0;
        private bool isGrabbing1;
        private bool isRetrating = false;
        private bool yeet;
        private readonly float distanceCovered = 2000;
        private float alpha = 1;

        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, new Vector3(0.2f, 0.2f, 0.2f));
            NPC.ai[0]++;
            NPC.TargetClosest(true);
            //Rectangle npcHitBox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height); // unused
            //Rectangle playerHitBox = new Rectangle((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height); // unused
            if (NPC.ai[3] == 0)
            {
                NPC.spriteDirection = -1;
                if (isGrabbing0 && !isRetrating)
                {
                    NPC.damage = 0;
                    alpha = Math.Abs(distance.X / distanceCovered);
                    (Main.npc[(int)NPC.ai[2]].ModNPC as KrakenHead).GETHIMBOIS = true;
                    if (!yeet)
                    {
                        Main.npc[(int)NPC.ai[2]].ai[0] = 0;
                        yeet = true;
                    }
                    Main.player[NPC.target].Center = NPC.Center;
                    NPC.velocity *= 0.94f;
                    if (NPC.life < 1000)
                    {
                        if (!isRetrating)
                        {
                            (Main.npc[(int)NPC.ai[2]].ModNPC as KrakenHead).Reset(3);
                            isRetrating = true;
                        }
                    }
                    if (distance.X < 0)
                    {
                        NPC.life = 0;
                    }
                }
                else
                {
                    if (NPC.ai[0] == 1)
                    {
                        startingPosition = NPC.Center;
                    }
                    if (distance.X > distanceCovered)
                    {
                        NPC.ai[1] = 1;
                    }
                    if (NPC.ai[1] == 0)
                    {
                        NPC.velocity.X = NPC.ai[0] / 5;
                        NPC.velocity.Y = (Main.player[NPC.target].Center.Y - NPC.Center.Y) / 50f;
                    }
                    else
                    {
                        isRetrating = true;
                        alpha = Math.Abs(distance.X / distanceCovered);
                        NPC.velocity.X = -5;
                        if (distance.X < 0)
                        {
                            NPC.life = 0;
                        }
                    }
                }
                if (Main.npc[(int)NPC.ai[2]].ai[0] >= 278)
                {
                    isRetrating = true;
                }
                if (isRetrating)
                {
                    if (distance.X < 0)
                    {
                        NPC.life = 0;
                    }
                    alpha = Math.Abs(distance.X / distanceCovered);
                    NPC.velocity.X = -5;
                    NPC.velocity.Y = (startingPosition.Y - NPC.Center.Y) / 100f;
                }
                else
                {
                    NPC.velocity.Y += (float)Math.Sin(NPC.ai[0] / 10f) * 0.04f;
                }
                distance = NPC.Center - startingPosition;
            }
            if (NPC.ai[3] == 1)
            {
                NPC.spriteDirection = 1;
                if (isGrabbing1 && !isRetrating)
                {
                    NPC.damage = 0;
                    alpha = Math.Abs(distance.X / distanceCovered);
                    (Main.npc[(int)NPC.ai[2]].ModNPC as KrakenHead).GETHIMBOIS = true;
                    if (!yeet)
                    {
                        Main.npc[(int)NPC.ai[2]].ai[0] = 0;
                        yeet = true;
                    }
                    Main.player[NPC.target].Center = NPC.Center;
                    NPC.velocity *= 0.94f;
                    if (NPC.life < 1000)
                    {
                        if (!isRetrating)
                        {
                            (Main.npc[(int)NPC.ai[2]].ModNPC as KrakenHead).Reset(3);
                            isRetrating = true;
                        }
                    }
                    if (distance.X > 0)
                    {
                        NPC.life = 0;
                    }
                }
                else
                {
                    if (NPC.ai[0] == 1)
                    {
                        startingPosition = NPC.Center;
                    }
                    if (distance.X < -distanceCovered)
                    {
                        NPC.ai[1] = 1;
                    }
                    if (NPC.ai[1] == 0)
                    {
                        NPC.velocity.X = -(NPC.ai[0] / 5);
                        NPC.velocity.Y = (Main.player[NPC.target].Center.Y - NPC.Center.Y) / 50f;
                    }
                    else
                    {
                        isRetrating = true;
                        alpha = Math.Abs(distance.X / distanceCovered);
                        NPC.velocity.X = 5;
                        if (distance.X > 0)
                        {
                            NPC.life = 0;
                        }
                    }
                    distance = NPC.Center - startingPosition;
                }
                if (Main.npc[(int)NPC.ai[2]].ai[0] >= 278)
                {
                    isRetrating = true;
                }
                if (isRetrating)
                {
                    if (distance.X > 0)
                    {
                        NPC.life = 0;
                    }
                    alpha = Math.Abs(distance.X / distanceCovered);
                    NPC.velocity.X = 5;
                    NPC.velocity.Y = (startingPosition.Y - NPC.Center.Y) / 100f;
                }
                else
                {
                    NPC.velocity.Y += (float)Math.Sin(NPC.ai[0] / 10f) * 0.04f;
                }
                distance = NPC.Center - startingPosition;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (NPC.ai[3] == 0)
            {
                isGrabbing0 = true;
                (Main.npc[(int)NPC.ai[2]].ModNPC as KrakenHead).isRightOrLeft = true;
            }
            if (NPC.ai[3] == 1)
            {
                isGrabbing1 = true;
                // (Main.npc[(int)NPC.ai[2]].ModNPC as KrakenHead).isRightOrLeft = false;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D chain = Mod.Assets.Request<Texture2D>("NPCs/Bosses/Kraken/ChainSmol").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Bosses/Kraken/EndOfSmol").Value;
            Helpers.DrawBezier(spriteBatch, chain, "", drawColor * alpha, NPC.Center, startingPosition, startingPosition + (NPC.Center - startingPosition) * 0.33f + new Vector2((float)Math.Cos(NPC.ai[0] / 23f) * 50, (float)Math.Sin(NPC.ai[0] / 10f) * 40), startingPosition + (NPC.Center - startingPosition) * 0.66f + new Vector2((float)Math.Sin(NPC.ai[0] / 20f) * 50, -(float)Math.Cos(NPC.ai[0] / 15f) * 55), NPC.width * 0.6f / distanceCovered, MathHelper.PiOver2, texture2);
            /*  if(npc.ai[3] == 0)
              Main.spriteBatch.Draw(texture, npc.Center + new Vector2(npc.width / 2, 0) - Main.screenPosition - distance / 2 + new Vector2(70,0), new Rectangle(texture.Width - (int)distance.X, 0, (int)distance.X, texture.Height), drawColor, npc.rotation, new Rectangle(texture.Width - (int)distance.X, 0, (int)distance.X, texture.Height).Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
              if (npc.ai[3] == 1)
                  Main.spriteBatch.Draw(texture, npc.Center - new Vector2(npc.width / 2, 0) - Main.screenPosition - distance / 2 + new Vector2(70, 0), new Rectangle(texture.Width + (int)distance.X, 0, -(int)distance.X, texture.Height), drawColor, npc.rotation, new Rectangle(texture.Width + (int)distance.X, 0, -(int)distance.X, texture.Height).Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);*/
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
    }
}
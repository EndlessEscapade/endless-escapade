using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class TentacleEdgeHandler : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
        }

        public override void SetDefaults()
        {
            NPC.width = 1;
            NPC.height = 1;
            NPC.damage = 20;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.damage = 0;
        }

        public override bool CheckActive()
        {
            return false;
        }

        private readonly Vector2[] startingPoint = new Vector2[5];
        private readonly Vector2[] endingPoint = new Vector2[5];
        private readonly Vector2[] midPoint = new Vector2[5];
        private float daFlop;
        private float daFlopX;
        private int coolDownForCollision;
        private KrakenHead krakenHead;
        private NPC npcBase;
        private Player player;

        public override void AI()
        {
            if (Main.npc[(int)NPC.ai[0]].life <= 0)
            {
                NPC.life = 0;
            }
            NPC.TargetClosest(true);
            player = Main.player[NPC.target];
            NPC.Center = player.Center - new Vector2(-200, 0);
            krakenHead = Main.npc[(int)NPC.ai[0]].ModNPC as KrakenHead;
            npcBase = Main.npc[(int)NPC.ai[0]];
            NPC.ai[1]++;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }

        public void DrawTentacleBeziers()
        {
            Color drawColor = NPC.GetAlpha(Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f)));
            int cooldown = 20;
            coolDownForCollision--;
            float chainsPer = 0.03f;
            if (coolDownForCollision < 0)
            {
                coolDownForCollision = 0;
            }
            daFlop = (float)Math.Sin(krakenHead.NPC.ai[2] / 20f) * 100;
            daFlopX = (float)Math.Cos(krakenHead.NPC.ai[2] / 30f) * 60;
            if (krakenHead.hasChains)
            {
                for (int i = 0; i < krakenHead.npcFromPositions.Length; i++)
                {
                    if (npcBase.ai[1] != 4 || (npcBase.ai[0] > krakenHead.dashPositions.Length * 50 && npcBase.ai[1] == 4))
                    {
                        if (NPC.ai[2] == 0)
                        {
                            startingPoint[i] = krakenHead.npcFromPositions[i] + (Vector2.Normalize(krakenHead.dashPositions[i] - krakenHead.npcFromPositions[i]) * 5400);
                            if (i == krakenHead.npcFromPositions.Length - 1)
                            {
                                NPC.ai[2] = 1;
                            }
                        }
                        endingPoint[i] = krakenHead.npcFromPositions[i] + (Vector2.Normalize(krakenHead.dashPositions[i] - krakenHead.npcFromPositions[i]) * 5400);
                        startingPoint[i] -= (startingPoint[i] - krakenHead.npcFromPositions[i]) / 32f;
                        midPoint[i] = startingPoint[i] + (endingPoint[i] - startingPoint[i]) * 0.2f + new Vector2(daFlopX, daFlop);
                    }
                    else
                    {
                        NPC.ai[2] = 0;
                        midPoint[i] = startingPoint[i] + (endingPoint[i] - startingPoint[i]) * 0.5f;
                        startingPoint[i] += (endingPoint[i] - startingPoint[i]) / 32f;
                    }
                    float gradient = (endingPoint[i].Y - startingPoint[i].Y) / (endingPoint[i].X - startingPoint[i].X);
                    if (player.Center.Y >= gradient * (player.Center.X - startingPoint[i].X) + startingPoint[i].Y - 60 && player.Center.Y <= gradient * (player.Center.X - startingPoint[i].X) + startingPoint[i].Y + 60 && coolDownForCollision == 0 && npcBase.ai[1] != 4)
                    {
                        Main.player[NPC.target].velocity += new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6));
                        Main.player[NPC.target].velocity *= -1.6f;
                        coolDownForCollision = cooldown;
                        NPC.netUpdate = true;
                    }
                    Helpers.DrawBezier(Main.spriteBatch, EEMod.Instance.Assets.Request<Texture2D>("NPCs/Bosses/Kraken/TentacleChain").Value, "", drawColor, startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], chainsPer, MathHelper.PiOver2);
                    /*if (npc.ai[1] % 8 == 0)
                    {
                        Rectangle playerHitBox = new Rectangle((int)Main.player[npc.target].Center.X, (int)Main.player[npc.target].Center.Y, Main.player[npc.target].width, Main.player[npc.target].height);

                        for (int j = 0; j < Helpers.ReturnPoints(startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], chainsPer, width, height, accuracy).Length; j++)
                        {
                            if (playerHitBox.Intersects(Helpers.ReturnPoints(startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], chainsPer, width, height, accuracy)[j]))
                            {
                                if (coolDownForCollision == 0)
                                {
                                    Main.player[npc.target].velocity *= -1;
                                    coolDownForCollision = cooldown;
                                }
                            }
                        }
                    }*/
                }
            }
            Texture2D oil = Mod.Assets.Request<Texture2D>("NPCs/Bosses/Kraken/Oil").Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int i = -10; i < 10; i++)
            {
                Main.spriteBatch.Draw(oil, new Vector2(krakenHead.arenaPosition.X + (i * oil.Width), 1000 - krakenHead.waterLevel + krakenHead.arenaPosition.Y + oil.Height / 2 + (float)Math.Sin(NPC.ai[1] / 30) * 20) - Main.screenPosition, new Rectangle(0, 0, oil.Width, oil.Height), drawColor * 0.8f, 0, new Rectangle(0, 0, oil.Width, oil.Height).Size() / 2, 1, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            if (player.Center.Y > 1000 - krakenHead.waterLevel + krakenHead.arenaPosition.Y + (float)Math.Sin(NPC.ai[1] / 30) * 20)
            {
                player.velocity *= .8f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //DrawTentacleBeziers(spriteBatch, drawColor);
            return false;
        }
    }
}
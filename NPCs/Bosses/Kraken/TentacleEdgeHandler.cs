using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class TentacleEdgeHandler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
        }

        public override void SetDefaults()
        {
            npc.width = 1;
            npc.height = 1;
            npc.damage = 20;
            npc.aiStyle = -1;
            npc.lifeMax = 1000;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            npc.dontTakeDamage = true;
            npc.damage = 0;
        }
        public override bool CheckActive()
        {
            return false;
        }
        Vector2[] startingPoint = new Vector2[5];
        Vector2[] endingPoint = new Vector2[5];
        Vector2[] midPoint = new Vector2[5];
        float daFlop;
        float daFlopX;
        int coolDownForCollision;
        KrakenHead krakenHead;
        NPC npcBase;
        Player player;
        public override void AI()
        {

            npc.TargetClosest(true);
            player = Main.player[npc.target];
            npc.Center = player.Center - new Vector2(-200,0);
            krakenHead = Main.npc[(int)npc.ai[0]].modNPC as KrakenHead;
            npcBase = Main.npc[(int)npc.ai[0]];
            npc.ai[1]++;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {


        }
        private static float X(float t,
    float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow((1 - t), 3) +
                x1 * 3 * t * Math.Pow((1 - t), 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }
        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow((1 - t), 3) +
                 y1 * 3 * t * Math.Pow((1 - t), 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }
        public void DrawTentacleBeziers()
        {
            Color drawColor = npc.GetAlpha(Lighting.GetColor((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f)));
            int cooldown = 20;
            coolDownForCollision--;
            float chainsPer = 0.03f;
            int accuracy = 1;
            int width = 60;
            int height = 190;
            if (coolDownForCollision < 0)
            {
                coolDownForCollision = 0;
            }
            daFlop = (float)Math.Sin(krakenHead.npc.ai[2] / 20f) * 100;
            daFlopX = (float)Math.Cos(krakenHead.npc.ai[2] / 30f) * 60;
            if (krakenHead.hasChains)
            {
                for (int i = 0; i < krakenHead.npcFromPositions.Length; i++)
                {
                    if (npcBase.ai[1] != 4 || (npcBase.ai[0] > (krakenHead.dashPositions.Length) * 50 && npcBase.ai[1] == 4))
                    {
                        if (npc.ai[2] == 0)
                        {
                            startingPoint[i] = krakenHead.npcFromPositions[i] + (Vector2.Normalize(krakenHead.dashPositions[i] - krakenHead.npcFromPositions[i]) * 5000);
                            if (i == krakenHead.npcFromPositions.Length - 1)
                                npc.ai[2] = 1;
                        }
                        endingPoint[i] = krakenHead.npcFromPositions[i] + (Vector2.Normalize(krakenHead.dashPositions[i] - krakenHead.npcFromPositions[i]) * 5000);
                        startingPoint[i] -= (startingPoint[i] - krakenHead.npcFromPositions[i]) / 32f;
                        midPoint[i] = startingPoint[i] + (endingPoint[i] - startingPoint[i]) * 0.2f + new Vector2(daFlopX, daFlop);
                    }
                    else
                    {
                        npc.ai[2] = 0;
                        midPoint[i] = startingPoint[i] + (endingPoint[i] - startingPoint[i]) * 0.5f;
                        startingPoint[i] += (endingPoint[i] - startingPoint[i]) / 32f;
                    }
                    float gradient = (endingPoint[i].Y - startingPoint[i].Y) / (endingPoint[i].X - startingPoint[i].X);
                    if (player.Center.Y >= gradient * (player.Center.X - startingPoint[i].X) + startingPoint[i].Y - 60 && player.Center.Y <= gradient * (player.Center.X - startingPoint[i].X) + startingPoint[i].Y + 60 && coolDownForCollision == 0 && npcBase.ai[1] != 4)
                    {
                        Main.player[npc.target].velocity *= -1.6f;
                        coolDownForCollision = cooldown;
                    }
                    Helpers.DrawBezier(Main.spriteBatch, TextureCache.TentacleChain, "", drawColor, startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], chainsPer, (float)Math.PI / 2);
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
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //DrawTentacleBeziers(spriteBatch, drawColor);
            return false;
        }
    }
}

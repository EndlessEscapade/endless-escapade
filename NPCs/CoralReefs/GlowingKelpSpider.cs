using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class GlowingKelpSpider : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Kelp Spider");
            //Main.npcFrameCount[npc.type] = 3;
        }


        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.lifeMax = 50000;
            npc.damage = 13;
            npc.defense = 3;

            npc.width = 84;
            npc.height = 53;
            npc.noGravity = false;
            npc.knockBackResist = 0f;

            npc.npcSlots = 1f;
            npc.buffImmune[BuffID.Confused] = true;
            npc.lavaImmune = false;
            banner = npc.type;
            npc.behindTiles = true;
            //bannerItem = ModContent.ItemType<Items.Banners.ClamBanner>();
            npc.value = Item.sellPrice(0, 0, 0, 75);
        }

        Vector2[] jointPoints;
        Vector2[] legPoints;
        bool[] CanMove;
        int[] CoolDown;
        Vector2 SpiderBodyPosition;
        int numberOfLegs = 6;
        float accell = 0.014f;
        float velocityOfSpider;
        float VertVel;
        bool OnGround;
        bool isCorrecting = true;
        public void DrawSpiderPort(Color drawColour)
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Texture2D UpperLegTex = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/GlowingKelpSpiderLegUpper");
            Texture2D LowerLegTex = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/GlowingKelpSpiderLegLower");
            Texture2D KelpSpiderBody = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/GlowingKelpSpiderBody");
            Texture2D UpperLegTexGlow = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/GlowingKelpSpiderLegUpperGlow");
            Texture2D LowerLegTexGlow = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/GlowingKelpSpiderLegLowerGlow");
            Texture2D KelpSpiderBodyGlow = ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/GlowingKelpSpiderBodyGlow");
            float rotation = (player.Center - npc.Center).ToRotation() + (float)Math.PI;
            bool cond = (rotation > 0 && rotation < Math.PI / 2f) || rotation > (float)Math.PI * 1.5f;
            float lerpCache = Math.Abs((float)Math.Sin(npc.ai[1] / 200f));
            for (int i = 0; i < numberOfLegs; i++)
            {

                Main.spriteBatch.Draw(UpperLegTex, ((SpiderBodyPosition + jointPoints[i]) / 2f).ForDraw(), UpperLegTex.Bounds, drawColour, (SpiderBodyPosition - jointPoints[i]).ToRotation() + (float)Math.PI / 2f, UpperLegTex.TextureCenter(), 1f, SpriteEffects.None, 0f);
                if (i % 2 == 0)
                    Main.spriteBatch.Draw(LowerLegTex, ((legPoints[i] + jointPoints[i]) / 2f).ForDraw(), LowerLegTex.Bounds, drawColour, (legPoints[i] - jointPoints[i]).ToRotation() + (float)Math.PI / 2f, LowerLegTex.TextureCenter(), 0.9f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(UpperLegTexGlow, ((SpiderBodyPosition + jointPoints[i]) / 2f).ForDraw(), UpperLegTex.Bounds, Color.White * lerpCache, (SpiderBodyPosition - jointPoints[i]).ToRotation() + (float)Math.PI / 2f, UpperLegTex.TextureCenter(), 1f, SpriteEffects.None, 0f);
                if (i % 2 == 0)
                    Main.spriteBatch.Draw(LowerLegTexGlow, ((legPoints[i] + jointPoints[i]) / 2f).ForDraw(), LowerLegTex.Bounds, Color.White * lerpCache, (legPoints[i] - jointPoints[i]).ToRotation() + (float)Math.PI / 2f, LowerLegTex.TextureCenter(), 0.9f, SpriteEffects.None, 0f);

            }
            Main.spriteBatch.Draw(KelpSpiderBody, SpiderBodyPosition.ForDraw(), KelpSpiderBody.Bounds, drawColour, rotation, KelpSpiderBody.TextureCenter(), 1f, !cond ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(KelpSpiderBodyGlow, SpiderBodyPosition.ForDraw(), KelpSpiderBody.Bounds, Color.White * lerpCache, rotation, KelpSpiderBody.TextureCenter(), 1f, !cond ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            for (int i = 0; i < numberOfLegs; i++)
            {

                if (i % 2 == 1)
                    Main.spriteBatch.Draw(LowerLegTex, ((legPoints[i] + jointPoints[i]) / 2f).ForDraw(), LowerLegTex.Bounds, drawColour, (legPoints[i] - jointPoints[i]).ToRotation() + (float)Math.PI / 2f, LowerLegTex.TextureCenter(), 0.9f, SpriteEffects.None, 0f);
                if (i % 2 == 1)
                    Main.spriteBatch.Draw(LowerLegTexGlow, ((legPoints[i] + jointPoints[i]) / 2f).ForDraw(), LowerLegTex.Bounds, Color.White * lerpCache, (legPoints[i] - jointPoints[i]).ToRotation() + (float)Math.PI / 2f, LowerLegTex.TextureCenter(), 0.9f, SpriteEffects.None, 0f);

            }
        }
        public void UpdateSpiderPort()
        {
            int seperationOfLegs = 15;
            int lengthOfUpperLeg = 30;
            int lengthOfLowerLeg = 30;
            int legVert = 25;
            int jointElevation = 5;


            if (SpiderBodyPosition == Vector2.Zero)
            {
                SpiderBodyPosition = Main.LocalPlayer.Center;
            }
            jointPoints = jointPoints ?? new Vector2[numberOfLegs];
            legPoints = legPoints ?? new Vector2[numberOfLegs];
            CanMove = CanMove ?? new bool[numberOfLegs];
            CoolDown = CoolDown ?? new int[numberOfLegs];
            for (int i = 0; i < numberOfLegs; i++)
            {
                legPoints[i] = legPoints[i] == Vector2.Zero ? new Vector2(Main.LocalPlayer.Center.X + (i - numberOfLegs * 0.5f + 0.5f) * seperationOfLegs, Main.LocalPlayer.Center.Y + legVert) : legPoints[i];
                jointPoints[i] = jointPoints[i] == Vector2.Zero ? new Vector2(Main.LocalPlayer.Center.X + (i - numberOfLegs * 0.5f + 0.5f) * seperationOfLegs, Main.LocalPlayer.Center.Y + legVert) : jointPoints[i];
            }


            velocityOfSpider *= 0.99f;
            SpiderBodyPosition.X += velocityOfSpider;
            float absVel = Math.Abs(velocityOfSpider);
            for (int i = 0; i < numberOfLegs; i++)
            {

                jointPoints[i] = CorrectLeg(SpiderBodyPosition, jointPoints[i])[1];
                jointPoints[i] = CorrectLeg(legPoints[i], jointPoints[i])[1];
                jointPoints[i].Y -= jointElevation;

                float dx = legPoints[i].X - jointPoints[i].X;
                float dy = legPoints[i].Y - jointPoints[i].Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                float TrueY = SpiderBodyPosition.Y + legVert;
                float TrueX = SpiderBodyPosition.X + (i - numberOfLegs * 0.5f + 0.5f) * seperationOfLegs + velocityOfSpider * 20;
                int Pogger = 0;

                if (WorldGen.InWorld((int)(TrueX / 16), (int)(TrueY / 16), 20))
                {
                    while (Main.tileSolid[Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).type] && Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).active() && Pogger < 64 && Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).slope() == 0)
                    {
                        SpiderBodyPosition.Y -= 0.005f;
                        TrueY--;
                        Pogger++;
                    }
                    while ((!Main.tileSolid[Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).type] || !Framing.GetTileSafely((int)(TrueX / 16), (int)(TrueY / 16)).active()) && Pogger < 32)
                    {
                        if (CanJump)
                            SpiderBodyPosition.Y += 0.005f;
                        TrueY++;
                        Pogger++;
                    }
                }
                Vector2 Goto = new Vector2(TrueX, TrueY);
                float dx2 = legPoints[i].X - Goto.X;
                float dy2 = legPoints[i].Y - Goto.Y;
                float distToGoto = (float)Math.Sqrt(dx2 * dx2 + dy2 * dy2);
                if (dist > lengthOfLowerLeg)
                {
                    if (CoolDown[i] > 0)
                        CoolDown[i]--;
                    if (CoolDown[i] <= 0)
                        CanMove[i] = true;
                    if (!PreparingForJump && CanJump)
                        legPoints[i].Y += (Goto.Y - legPoints[i].Y) / 6f;
                }
                if (CanMove[i])
                {
                    float factor = 4f;
                    if (CoolDown[i] > 0)
                        CoolDown[i]--;
                    float xCompletion = Math.Abs(Goto.X - legPoints[i].X);
                    if (CanJump)
                    {
                        legPoints[i].X += (Goto.X - legPoints[i].X) / factor;

                        legPoints[i].Y += (Goto.Y - legPoints[i].Y) / factor - (!PreparingForJump ? (float)Math.Sin(xCompletion / 20f) * 7 : 0);
                        if (distToGoto < (factor * absVel + 4f + absVel))
                        {
                            CoolDown[i] = Main.rand.Next((int)(20 - absVel * 3), (int)(40 + absVel * 3));
                            CanMove[i] = false;
                        }
                    }
                }
            }
            float UnderSpiderY = SpiderBodyPosition.Y + legVert + 8;
            float UnderSpiderX = SpiderBodyPosition.X;
            bool OnGroundBuffer = OnGround;
            if (!Main.tileSolid[Framing.GetTileSafely((int)UnderSpiderX / 16, (int)UnderSpiderY / 16).type] || !Framing.GetTileSafely((int)UnderSpiderX / 16, (int)UnderSpiderY / 16).active())
            {
                VertVel += 0.2f;
                OnGround = false;
            }
            else
            {
                VertVel -= 0.1f;
                if (npc.ai[1] % ChanceToJump >= 10f)
                {
                    OnGround = true;
                }
            }
            if (OnGroundBuffer != OnGround && OnGround)
            {
                VertVel = 0;
            }
            VertVel *= 0.99f;
            if (!PreparingForJump)
                SpiderBodyPosition.Y += VertVel;

            Vector2[] CorrectLeg(Vector2 feetVec, Vector2 jointVec)
            {
                float dx = feetVec.X - jointVec.X;
                float dy = feetVec.Y - jointVec.Y;
                float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
                float deltaLength = currentLength - lengthOfUpperLeg;
                float perc = (deltaLength / (float)currentLength) * 0.5f;
                float offsetX = perc * dx;
                float offsetY = perc * dy;
                Vector2 F = new Vector2(feetVec.X + offsetX, feetVec.Y + offsetY);
                Vector2 J = new Vector2(jointVec.X + offsetX, jointVec.Y + offsetY);

                return new Vector2[] { F, J };
            }
        }
        void Movement()
        {
            if (npc.Center.X < Main.player[npc.target].Center.X)
            {
                velocityOfSpider += accell;
            }
            if (npc.Center.X > Main.player[npc.target].Center.X)
            {
                velocityOfSpider -= accell;
            }
        }
        void PrepareForJump()
        {
            SpiderBodyPosition.Y += 0.4f;
        }
        bool CanJump = true;
        bool PreparingForJump;
        int ChanceToJump = 500;
        void Jump(float jumpHeight, float horiz)
        {
            VertVel -= jumpHeight;
            velocityOfSpider += horiz;
        }
        void MakeJointsFollowVerticalDisplacement()
        {
            for (int i = 0; i < numberOfLegs; i++)
            {
                legPoints[i].Y += VertVel + (float)Math.Sin(npc.ai[1] / 10f) * 0.8f;
                jointPoints[i].Y += VertVel + (float)Math.Cos(npc.ai[1] / 10f) * 0.8f;
                legPoints[i].X += velocityOfSpider + (float)Math.Cos(npc.ai[1] / 7f + i) * 1.2f;
                jointPoints[i].X += velocityOfSpider;
            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.22f);
        }

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                SpiderBodyPosition = npc.Center;
                npc.ai[0] = 1;
            }
            npc.ai[1]++;
            Movement();
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 npcTilePos = npc.Center / 16;
            bool ifAbove = Framing.GetTileSafely((int)npcTilePos.X, (int)npcTilePos.Y - 6).active() && Main.tileSolid[Framing.GetTileSafely((int)npcTilePos.X, (int)npcTilePos.Y - 6).type];
            bool ifPlayerAbove = (npc.Center.Y - player.Center.Y) > 140;
            UpdateSpiderPort();
            if (npc.ai[1] % ChanceToJump <= 5f && !ifAbove && ifPlayerAbove)
            {
                Jump(0.3f * (4 - (npc.ai[1] % ChanceToJump) / 5f), Helpers.Clamp(player.Center.X - npc.Center.X, -0.6f, 0.6f));
                CanJump = false;
            }
            else if (npc.ai[1] % ChanceToJump <= (ChanceToJump - 60) && !CanJump && npc.ai[1] % 300 >= 7)
            {
                MakeJointsFollowVerticalDisplacement();
            }
            if (OnGround)
            {
                CanJump = true;
            }

            if (npc.ai[1] % ChanceToJump >= (ChanceToJump - 60) && !ifAbove && ifPlayerAbove)
            {
                velocityOfSpider *= 0.98f;
                PrepareForJump();
                PreparingForJump = true;
            }
            else
            {
                PreparingForJump = false;
            }
            npc.Center = SpiderBodyPosition;

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            DrawSpiderPort(drawColor);
            return false;
        }
    }
}

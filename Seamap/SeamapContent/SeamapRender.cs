using EEMod.Extensions;
using EEMod.Seamap.SeamapAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEMod;
namespace EEMod.SeamapAssets
{
    public class SeamapRender
    {
        public static void DrawShip()
        {
            Vector2 position = instance.position;

            float intenstityLightning = instance.intenstityLightning;
            Vector2 currentLightningPos = instance.currentLightningPos;
            Vector2 velocity = instance.velocity;

            int frames = 12;
            Rectangle frame = instance.frame;
            int frameNum = 0;
            Texture2D texture3 = instance.GetTexture("Seamap/SeamapAssets/ShipHelthSheet");
            Texture2D texture = instance.GetTexture("Seamap/SeamapAssets/ShipMount");
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            //Lighting.AddLight(eePlayer.objectPos[1], 0.9f, 0.9f, 0.9f);
            if (Main.rand.NextBool(100) && !Main.dayTime)
            {
                currentLightningPos = Main.screenPosition + new Vector2(Main.rand.Next(500), Main.rand.Next(1000));
                intenstityLightning = Main.rand.NextFloat(.1f, .2f);
            }
            if (intenstityLightning > 0)
            {
                float rand = Main.rand.NextFloat(.2f, 5f);
                intenstityLightning -= 0.008f;
                float light = rand * intenstityLightning;
                Lighting.AddLight(currentLightningPos, light, light, light);
            }

            Lighting.AddLight(Main.screenPosition + position, .1f, .1f, .1f);

            if (Main.netMode == NetmodeID.SinglePlayer || ((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 0))
            {
                if (eePlayer.boatSpeed == 3)
                {
                    frameNum = 1;
                }

                if (eePlayer.boatSpeed == 1)
                {
                    frameNum = 0;
                }
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                switch (player.team)
                {
                    case 1:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 3;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 2;
                        break;
                    case 2:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 9;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 8;
                        break;
                    case 3:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 5;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 4;
                        break;
                    case 4:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 7;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 6;
                        break;
                    case 5:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 11;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 10;
                        break;
                }
            }
            for (int i = 0; i < Main.ActivePlayersCount; i++)
            {
                if (i == 0)
                {
                    Color drawColour = Lighting.GetColor((int)((Main.screenPosition.X + position.X) / 16f), (int)((Main.screenPosition.Y + position.Y) / 16f));
                    Main.spriteBatch.Draw(texture, position, new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), velocity.X / 10, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        EEServerVariableCache.SyncBoatPos(position, velocity.X);
                    }
                    for (int j = 0; j < 255; j++)
                    {
                        if (Main.player[j].active && j != Main.myPlayer)
                        {
                            Color drawColour = Lighting.GetColor((int)(EEServerVariableCache.OtherBoatPos[j].X / 16f), (int)(EEServerVariableCache.OtherBoatPos[j].Y / 16f));
                            Main.spriteBatch.Draw(texture, EEServerVariableCache.OtherBoatPos[j], new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), EEServerVariableCache.OtherRot[j] / 10f, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, EEServerVariableCache.OtherRot[j] < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        }
                    }

                }
            }
            //float quotient = ShipHelth / ShipHelthMax; // unused
            Rectangle rect = new Rectangle(0, (int)(texture3.Height / 8 * ShipHelth), texture3.Width, texture3.Height / 8);
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 175, 50), rect, Color.White, 0, rect.Size() / 2, 1, SpriteEffects.None, 0);
        }
        public static void RenderIslands()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            for (int i = 0; i < modPlayer.SeaObject.Count; i++)
            {
                EEPlayer.Island current = modPlayer.SeaObject[i];
                Vector2 currentPos = current.posToScreen.ForDraw();
                Color drawColour = Lighting.GetColor((int)(current.posToScreen.X / 16f), (int)(current.posToScreen.Y / 16f));
                if (current.isColliding)
                {
                    if (instance.anchorLerp[i] < 1)
                        instance.anchorLerp[i] += 0.02f;
                }
                else
                {
                    if (instance.anchorLerp[i] > 0)
                        instance.anchorLerp[i] -= 0.02f;
                }
                Main.spriteBatch.Draw(instance.GetTexture("Seamap/SeamapAssets/Anchor"), currentPos + new Vector2(0, (float)Math.Sin(instance.markerPlacer / 20f)) * 4 + new Vector2(current.texture.Width / 2f - instance.GetTexture("Seamap/SeamapAssets/Anchor").Width / 2f, -80), drawColour * instance.anchorLerp[i]);
                if (modPlayer.quickOpeningFloat > 0.01f)
                {
                    float lerp = 1 - (modPlayer.quickOpeningFloat / 10f);
                    if (i > 4 && i < 8 || i == 11)
                    {
                        float score = currentPos.X + currentPos.Y;
                        Main.spriteBatch.Draw(current.texture, currentPos + new Vector2(0, (float)Math.Sin(score + instance.markerPlacer / 40f)) * 4, drawColour * lerp);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(current.texture, currentPos, drawColour * lerp);
                    }
                }
                else
                {
                    if (i > 4 && i < 8 || i == 11)
                    {
                        float score = currentPos.X + currentPos.Y;
                        Main.spriteBatch.Draw(current.texture, currentPos + new Vector2(0, (float)Math.Sin(score + instance.markerPlacer / 40f)) * 4, drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                    }
                    else
                    {
                        Main.spriteBatch.Draw(current.texture, currentPos, drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                    }
                }
            }
            var OceanElements = EEPlayer.OceanMapElements;
            for (int i = 0; i < OceanElements.Count; i++)
            {
                var element = OceanElements[i];
                element.Draw(Main.spriteBatch);
            }
            for (int i = 0; i < modPlayer.seagulls.Count; i++)
            {
                var element = modPlayer.seagulls[i];
                element.frameCounter++;
                element.Position += new Vector2(0, -0.5f);
                element.Draw(instance.GetTexture("Seamap/SeamapAssets/Seagulls"), 9, 5);
            }
        }
    }
}
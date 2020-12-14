using EEMod.Extensions;
using EEMod.Seamap.SeamapAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEMod;
namespace EEMod.Seamap.SeamapContent
{
    public partial class Seamap
    {
        static EEPlayer modPlayer
        {
            get => Main.LocalPlayer.GetModPlayer<EEPlayer>();
        }

        public static void RenderShip()
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


            #region Spawning lightning
            //Lighting.AddLight(eePlayer.objectPos[1], 0.9f, 0.9f, 0.9f);
            if (Main.rand.NextBool(100) && eePlayer.isStorming)
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
            #endregion

            #region Setting the player's ship flags
            if (Main.netMode == NetmodeID.SinglePlayer || (player.team == 0))
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
            #endregion

            #region Drawing the ship
            for (int i = 0; i < Main.ActivePlayersCount; i++)
            {
                if (i == 0)
                {
                    Color drawColour = Lighting.GetColor((int)((Main.screenPosition.X + position.X) / 16f), (int)((Main.screenPosition.Y + position.Y) / 16f)) * eePlayer.seamapLightColor;
                    drawColour.A = 255;
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
                            Color drawColour = Lighting.GetColor((int)(EEServerVariableCache.OtherBoatPos[j].X / 16f), (int)(EEServerVariableCache.OtherBoatPos[j].Y / 16f)) * eePlayer.seamapLightColor;
                            drawColour.A = 255;
                            Main.spriteBatch.Draw(texture, EEServerVariableCache.OtherBoatPos[j], new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), EEServerVariableCache.OtherRot[j] / 10f, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, EEServerVariableCache.OtherRot[j] < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        }
                    }
                }
            }
            #endregion

            #region Drawing the ship healthbar
            //float quotient = ShipHelth / ShipHelthMax; // unused
            Rectangle rect = new Rectangle(0, (int)(texture3.Height / 8 * ShipHelth), texture3.Width, texture3.Height / 8);
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 175, 50), rect, Color.White, 0, texture3.TextureCenter(), 1, SpriteEffects.None, 0);
            #endregion
        }

        static int frame = 0;
        public static void Render()
        {
            RenderWater(); //Layer 0
            RenderIslands(); //Layer 1
            RenderShip(); //Layer 2
            RenderClouds(); //Layer 3
        }

        static void RenderClouds()
        {
            frame++;

            #region Drawing elements from the OceanMapElements array
            for (int i = 0; i < EEPlayer.OceanMapElements.Count; i++)
            {
                var element = EEPlayer.OceanMapElements[i];
                element.Draw(Main.spriteBatch);
            }
            #endregion

            #region Drawing seagulls
            for (int i = 0; i < modPlayer.seagulls.Count; i++)
            {
                var element = modPlayer.seagulls[i];
                element.frameCounter++;
                element.Position += new Vector2(0, -0.5f);
                element.Draw(instance.GetTexture("Seamap/SeamapAssets/Seagull"), 9, 5);
            }
            #endregion
        }

        static void RenderIslands()
        {
            for (int i = 0; i < modPlayer.SeaObject.Count; i++)
            {
                EEPlayer.SeaEntity current = modPlayer.SeaObject[i];

                Vector2 currentPos = current.posToScreen.ForDraw();
                Color drawColour = Lighting.GetColor((int)(current.posToScreen.X / 16f), (int)(current.posToScreen.Y / 16f)) * Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapLightColor;
                drawColour.A = 255;

                #region Making the anchor move if the object can be departed to
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
                #endregion

                //Main.spriteBatch.Draw(instance.GetTexture("Seamap/SeamapAssets/Anchor"), currentPos + new Vector2(0, (float)Math.Sin(instance.markerPlacer / 20f)) * 4 + new Vector2(current.texture.Width / 2f - instance.GetTexture("Seamap/SeamapAssets/Anchor").Width / 2f, -80), drawColour * instance.anchorLerp[i]);

                #region Incrementing the frame of the object
                if (current.frameSpeed > 0)
                {
                    if (frame % current.frameSpeed == 0)
                    {
                        modPlayer.SeaObjectFrames[i]++;
                        if (modPlayer.SeaObjectFrames[i] > current.frames - 1)
                            modPlayer.SeaObjectFrames[i] = 0;
                    }
                }
                #endregion

                #region Drawing the object
                if (modPlayer.quickOpeningFloat > 0.01f)
                {
                    float lerp = 1 - (modPlayer.quickOpeningFloat / 10f);
                    if (i > 4 && i < 8 || i == 11)
                    {
                        float score = currentPos.X + currentPos.Y;
                        Vector2 pos = currentPos + new Vector2(0, (float)Math.Sin(score + instance.markerPlacer / 40f)) * 4;
                        Main.spriteBatch.Draw(current.texture, new Rectangle((int)pos.X, (int)pos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, modPlayer.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * lerp);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(current.texture, new Rectangle((int)currentPos.X, (int)currentPos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, modPlayer.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * lerp);
                    }
                }
                else
                {
                    if (i > 4 && i < 8 || i == 11)
                    {
                        float score = currentPos.X + currentPos.Y;
                        Vector2 pos = currentPos + new Vector2(0, (float)Math.Sin(score + instance.markerPlacer / 40f)) * 4;
                        Main.spriteBatch.Draw(current.texture, new Rectangle((int)pos.X, (int)pos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, modPlayer.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                    }
                    else
                    {
                        Main.spriteBatch.Draw(current.texture, new Rectangle((int)currentPos.X, (int)currentPos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, modPlayer.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                    }
                }
                #endregion
            }
        }

        #region Seamap water
        static void RenderWater()
        {
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            Texture2D waterTexture = instance.GetTexture("Seamap/SeamapAssets/WaterBg");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 pos = Main.screenPosition;
            Vector2 toScreen = pos.ForDraw();
            Color colour = Lighting.GetColor((int)(pos.X / 16), (int)(pos.Y / 16));
            Color SeaColour = new Color(0.1568f, 0.6549f, 0.7607f).MultiplyRGB(colour);
            WaterShader.Parameters["noise"].SetValue(instance.GetTexture("Noise/WormNoisePixelated"));
            WaterShader.Parameters["noiseN"].SetValue(instance.GetTexture("Noise/WormNoisePixelated"));
            WaterShader.Parameters["water"].SetValue(instance.GetTexture("ShaderAssets/WaterShaderLightMap"));
            WaterShader.Parameters["yCoord"].SetValue((float)Math.Sin(Main.time / 3000f) * 0.2f);
            WaterShader.Parameters["xCoord"].SetValue((float)Math.Cos(Main.time / 2000f) * 0.2f);
            WaterShader.Parameters["Colour"].SetValue(SeaColour.ToVector3());
            WaterShader.Parameters["LightColour"].SetValue(colour.ToVector3());
            WaterShader.Parameters["waveSpeed"].SetValue(6);
            WaterShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, Main.screenWidth, Main.screenWidth), colour);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        #endregion
    }
}
using EEMod.Extensions;
using EEMod.Seamap.SeamapAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod;
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
            Vector2 position = SeamapPlayerShip.localship.position.ForDraw();

            float intenstityLightning = SeamapPlayerShip.localship.intenstityLightning;
            Vector2 currentLightningPos = SeamapPlayerShip.localship.currentLightningPos;
            Vector2 velocity = SeamapPlayerShip.localship.velocity;

            int frames = 12;
            Rectangle frame = SeamapPlayerShip.localship.frame;
            int frameNum = 0;
            Texture2D playerShipTexture = ModContent.GetTexture("EEMod/Seamap/SeamapAssets/ShipMount");
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
                    Main.spriteBatch.Draw(playerShipTexture, position, new Rectangle(0, frameNum * 52, playerShipTexture.Width, playerShipTexture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), velocity.X / 10, new Rectangle(0, frame.Y, playerShipTexture.Width, playerShipTexture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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
                            Main.spriteBatch.Draw(playerShipTexture, EEServerVariableCache.OtherBoatPos[j], new Rectangle(0, frameNum * 52, playerShipTexture.Width, playerShipTexture.Height / frames), drawColour * (1 - (eePlayer.cutSceneTriggerTimer / 180f)), EEServerVariableCache.OtherRot[j] / 10f, new Rectangle(0, frame.Y, playerShipTexture.Width, playerShipTexture.Height / frames).Size() / 2, 1, EEServerVariableCache.OtherRot[j] < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        }
                    }
                }
            }
            #endregion
        }

        static int frame = 0;

        public static void Render()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            RenderWater(); //Layer 0
            RenderIslands(spriteBatch); //Layer 1
            RenderShip(); //Layer 2
            RenderEntities(); //Layer 3
            RenderClouds(); //Layer 4
            RenderSeamapUI(); //Layer 5
        }

        public static void RenderSeamapUI()
        {
            Texture2D texture3 = ModContent.GetTexture("EEMod/Seamap/SeamapAssets/ShipHelthSheet");

            #region Drawing the ship healthbar
            //float quotient = ShipHelth / ShipHelthMax; // unused
            Rectangle rect = new Rectangle(0, (int)(texture3.Height / 8 * SeamapPlayerShip.localship.shipHelth), texture3.Width, texture3.Height / 8);
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 200, 200), rect, Color.White, 0, texture3.TextureCenter(), 1, SpriteEffects.None, 0);
            #endregion
        }

        public static void RenderEntities()
        {
            for (int asdasdasd = 0; asdasdasd < SeamapObjects.SeamapEntities.Length; asdasdasd++)
            {
                if (SeamapObjects.SeamapEntities[asdasdasd] != null)
                {
                    SeamapObjects.SeamapEntities[asdasdasd].Draw(Main.spriteBatch);
                    SeamapObjects.SeamapEntities[asdasdasd].PostDraw(Main.spriteBatch);
                }
            }
        }

        static void RenderClouds()
        {
            frame++;

            #region Drawing elements from the OceanMapElements array
            for (int i = 0; i < SeamapObjects.OceanMapElements.Count; i++)
            {
                var element = SeamapObjects.OceanMapElements[i];
                element.Draw(Main.spriteBatch);
            }
            #endregion

            #region Drawing seagulls
            for (int i = 0; i < modPlayer.seagulls.Count; i++)
            {
                var element = modPlayer.seagulls[i];
                element.frameCounter++;
                element.Position += new Vector2(0, -0.5f);
                element.Draw(ModContent.GetTexture("EEMod/Seamap/SeamapAssets/Seagull"), 9, 5);
            }
            #endregion
        }

        static void RenderIslands(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < SeamapObjects.IslandEntities.Count; i++)
            {
                var current = SeamapObjects.IslandEntities[i];
                current.Draw(spriteBatch);
                current.Update();
            }
            //for (int i = 0; i < SeamapObjects.SeaObject.Count; i++)
            //{
                //var current = SeamapObjects.SeaObject[i];


                //#region Making the anchor move if the object can be departed to
                //if (current.isColliding)
                //{
                //    if (SeamapPlayerShip.localship.anchorLerp[i] < 1)
                //        SeamapPlayerShip.localship.anchorLerp[i] += 0.02f;
                //}
                //else
                //{
                //    if (SeamapPlayerShip.localship.anchorLerp[i] > 0)
                //        SeamapPlayerShip.localship.anchorLerp[i] -= 0.02f;
                //}
                //#endregion

                ////Main.spriteBatch.Draw(instance.GetTexture("Seamap/SeamapAssets/Anchor"), currentPos + new Vector2(0, (float)Math.Sin(instance.markerPlacer / 20f)) * 4 + new Vector2(current.texture.Width / 2f - instance.GetTexture("Seamap/SeamapAssets/Anchor").Width / 2f, -80), drawColour * instance.anchorLerp[i]);

                //#region Incrementing the frame of the object
                //if (current.frameSpeed > 0)
                //{
                //    if (frame % current.frameSpeed == 0)
                //    {
                //        SeamapObjects.SeaObjectFrames[i]++;
                //        if (SeamapObjects.SeaObjectFrames[i] > current.frames - 1)
                //            SeamapObjects.SeaObjectFrames[i] = 0;
                //    }
                //}
                //#endregion

                //#region Drawing the object
                //if (modPlayer.quickOpeningFloat > 0.01f)
                //{
                //    float lerp = 1 - (modPlayer.quickOpeningFloat / 10f);
                //    if (i > 4 && i < 8 || i == 11)
                //    {
                //        float score = currentPos.X + currentPos.Y;
                //        Vector2 pos = currentPos + new Vector2(0, (float)Math.Sin(score + SeamapPlayerShip.localship.markerPlacer / 40f)) * 4;
                //        Main.spriteBatch.Draw(current.texture, new Rectangle((int)pos.X, (int)pos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, SeamapObjects.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * lerp);
                //    }
                //    else
                //    {
                //        Main.spriteBatch.Draw(current.texture, new Rectangle((int)currentPos.X, (int)currentPos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, SeamapObjects.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * lerp);
                //    }
                //}
                //else
                //{
                //    if (i > 4 && i < 8 || i == 11)
                //    {
                //        float score = currentPos.X + currentPos.Y;
                //        Vector2 pos = currentPos + new Vector2(0, (float)Math.Sin(score + SeamapPlayerShip.localship.markerPlacer / 40f)) * 4;
                //        Main.spriteBatch.Draw(current.texture, new Rectangle((int)pos.X, (int)pos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, SeamapObjects.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                //    }
                //    else
                //    {
                //        Main.spriteBatch.Draw(current.texture, new Rectangle((int)currentPos.X, (int)currentPos.Y, current.texture.Width, current.texture.Height / current.frames), new Rectangle(0, SeamapObjects.SeaObjectFrames[i] * (current.texture.Height / current.frames), current.texture.Width, (current.texture.Height / current.frames)), drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)));
                //    }
                //}
                //#endregion
            //}
        }

        #region Seamap water
        static void RenderWater()
        {
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            Texture2D waterTexture = ModContent.GetTexture("EEMod/Seamap/SeamapAssets/WaterBg");

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos.ForDraw();

            Color colour = Lighting.GetColor((int)(Main.screenPosition.X / 16), (int)(Main.screenPosition.Y / 16));
            Color SeaColour = new Color(0.1568f, 0.6549f, 0.7607f).MultiplyRGB(colour);

            WaterShader.Parameters["noise"].SetValue(ModContent.GetTexture("EEMod/Noise/WormNoisePixelated"));
            WaterShader.Parameters["noiseN"].SetValue(ModContent.GetTexture("EEMod/Noise/WormNoisePixelated"));
            WaterShader.Parameters["water"].SetValue(ModContent.GetTexture("EEMod/Textures/WaterShaderLightMap"));
            WaterShader.Parameters["yCoord"].SetValue((float)Math.Sin(Main.time / 3000f) * 0.2f);
            WaterShader.Parameters["xCoord"].SetValue((float)Math.Cos(Main.time / 3000f) * 0.2f);
            WaterShader.Parameters["Colour"].SetValue(SeaColour.ToVector3());
            WaterShader.Parameters["LightColour"].SetValue(colour.ToVector3());
            WaterShader.Parameters["waveSpeed"].SetValue(3);
            WaterShader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, Main.screenWidth*5, Main.screenWidth*5), colour);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        #endregion
    }
}
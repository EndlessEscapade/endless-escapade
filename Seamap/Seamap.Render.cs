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

        public static void RenderShip(SpriteBatch spriteBatch)
        {
            Vector2 position = SeamapObjects.localship.position.ForDraw();

            float intenstityLightning = SeamapObjects.localship.intenstityLightning;
            Vector2 currentLightningPos = SeamapObjects.localship.currentLightningPos;
            Vector2 velocity = SeamapObjects.localship.velocity;

            int frames = 12;
            Rectangle frame = SeamapObjects.localship.frame;
            int frameNum = 0;
            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShipMount").Value;
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            #region Spawning lightning
            //Lighting.AddLight(eePlayer.objectPos[1], 0.9f, 0.9f, 0.9f);
            //if (Main.rand.NextBool(100) && eePlayer.isStorming)
            //{
            //    currentLightningPos = Main.screenPosition + new Vector2(Main.rand.Next(500), Main.rand.Next(1000));
            //    intenstityLightning = Main.rand.NextFloat(.1f, .2f);
            //}
            //if (intenstityLightning > 0)
            //{
            //    float rand = Main.rand.NextFloat(.2f, 5f);
            //    intenstityLightning -= 0.008f;
            //    float light = rand * intenstityLightning;
            //    Lighting.AddLight(currentLightningPos, light, light, light);
            //}

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
            for (int i = 0; i < Main.PlayerList.Count; i++)
            {
                if (i == 0)
                {
                    //Color drawColour = Lighting.GetColor((int)((Main.screenPosition.X + position.X) / 16f), (int)((Main.screenPosition.Y + position.Y) / 16f)) * eePlayer.seamapLightColor;
                    Color drawColour = Color.White * eePlayer.seamapLightColor;

                    drawColour.A = 255;
                    spriteBatch.Draw(playerShipTexture, position, new Rectangle(0, frameNum * 52, playerShipTexture.Width, playerShipTexture.Height / frames), drawColour * (1 - (Main.LocalPlayer.GetModPlayer<EEPlayer>().cutSceneTriggerTimer / 180f)), velocity.X / 10, new Rectangle(0, frame.Y, playerShipTexture.Width, playerShipTexture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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
                            //Color drawColour = Lighting.GetColor((int)(EEServerVariableCache.OtherBoatPos[j].X / 16f), (int)(EEServerVariableCache.OtherBoatPos[j].Y / 16f)) * eePlayer.seamapLightColor;
                            Color drawColour = Color.White * eePlayer.seamapLightColor;

                            drawColour.A = 255;
                            spriteBatch.Draw(playerShipTexture, EEServerVariableCache.OtherBoatPos[j], new Rectangle(0, frameNum * 52, playerShipTexture.Width, playerShipTexture.Height / frames), drawColour * (1 - (Main.LocalPlayer.GetModPlayer<EEPlayer>().cutSceneTriggerTimer / 180f)), EEServerVariableCache.OtherRot[j] / 10f, new Rectangle(0, frame.Y, playerShipTexture.Width, playerShipTexture.Height / frames).Size() / 2, 1, EEServerVariableCache.OtherRot[j] < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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

            Main.screenPosition = SeamapObjects.localship.Center + new Vector2(-Main.screenWidth / 2f, -Main.screenHeight / 2f);

            Main.screenPosition.X = MathHelper.Clamp(Main.screenPosition.X, 0, (Main.screenWidth * 5) - Main.screenWidth);
            Main.screenPosition.Y = MathHelper.Clamp(Main.screenPosition.Y, 0, (Main.screenWidth * 5) - Main.screenHeight);

            RenderWater(spriteBatch); //Layer 0
            RenderIslands(spriteBatch); //Layer 1
            RenderShip(spriteBatch); //Layer 2
            RenderEntities(spriteBatch); //Layer 3
            RenderClouds(spriteBatch); //Layer 4
            RenderSeamapUI(spriteBatch); //Layer 5

            //EEMod.DrawText();
        }

        public static void RenderSeamapUI(SpriteBatch spriteBatch)
        {
            Texture2D texture3 = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShipHelthSheet").Value;

            #region Drawing the ship healthbar
            Rectangle rect = new Rectangle(0, (int)(texture3.Height / 8 * SeamapObjects.localship.shipHelth), texture3.Width, texture3.Height / 8);
            spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 200, 200), rect, Color.White, 0, texture3.TextureCenter(), 1, SpriteEffects.None, 0);
            #endregion
        }

        public static void RenderEntities(SpriteBatch spriteBatch)
        {
            for (int asdasdasd = 0; asdasdasd < SeamapObjects.SeamapEntities.Length; asdasdasd++)
            {
                if (SeamapObjects.SeamapEntities[asdasdasd] != null)
                {
                    SeamapObjects.SeamapEntities[asdasdasd].Draw(spriteBatch);
                    SeamapObjects.SeamapEntities[asdasdasd].PostDraw(spriteBatch);
                }
            }
        }

        static void RenderClouds(SpriteBatch spriteBatch)
        {
            frame++;

            #region Drawing elements from the OceanMapElements array
            for (int i = 0; i < SeamapObjects.OceanMapElements.Count; i++)
            {
                var element = SeamapObjects.OceanMapElements[i];
                element.Draw(spriteBatch);
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
        }

        #region Seamap water
        static void RenderWater(SpriteBatch spriteBatch)
        {
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/WaterBg").Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos.ForDraw();

            Color colour = Color.White;

            Color SeaColour = new Color(0.1568f, 0.6549f, 0.7607f).MultiplyRGB(colour);

            WaterShader.Parameters["noise"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/WormNoisePixelated").Value);
            WaterShader.Parameters["noiseN"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/WormNoisePixelated").Value);
            WaterShader.Parameters["water"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/WaterShaderLightMapContrast").Value);

            WaterShader.Parameters["yCoord"].SetValue((float)Math.Sin(Main.time / 3000f) * 0.2f);
            WaterShader.Parameters["xCoord"].SetValue((float)Math.Cos(Main.time / 3000f) * 0.2f);
            WaterShader.Parameters["Colour"].SetValue(SeaColour.ToVector3());
            WaterShader.Parameters["LightColour"].SetValue(colour.ToVector3());
            WaterShader.Parameters["waveSpeed"].SetValue(3);
            WaterShader.Parameters["resolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

            WaterShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, Main.screenWidth*5, Main.screenWidth*5), colour);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        #endregion
    }
}
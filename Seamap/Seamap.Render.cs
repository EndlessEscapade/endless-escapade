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
using System.Diagnostics;

namespace EEMod.Seamap.SeamapContent
{
    public partial class Seamap
    {
        static EEPlayer modPlayer
        {
            get => Main.LocalPlayer.GetModPlayer<EEPlayer>();
        }

        public static void Render()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            Main.screenPosition = SeamapObjects.localship.Center + new Vector2(-Main.screenWidth / 2f, -Main.screenHeight / 2f);

            Main.screenPosition.X = MathHelper.Clamp(Main.screenPosition.X, 0, (seamapWidth) - Main.screenWidth);
            Main.screenPosition.Y = MathHelper.Clamp(Main.screenPosition.Y, 0, (seamapHeight) - Main.screenHeight);

            RenderWater(spriteBatch); //Layer 0
            RenderEntities(spriteBatch); //Layer 1, postdraw layer 2
            RenderSeamapUI(spriteBatch); //Layer 3

            //EEMod.DrawText();
        }

        public static void RenderSeamapUI(SpriteBatch spriteBatch)
        {
            Texture2D texture3 = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShipHelthSheet").Value;

            Rectangle rect = new Rectangle(0, (int)(texture3.Height / 8 * SeamapObjects.localship.shipHelth), texture3.Width, texture3.Height / 8);
            spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 200, 200), rect, Color.White, 0, texture3.TextureCenter(), 1, SpriteEffects.None, 0);
        }

        public static void RenderEntities(SpriteBatch spriteBatch)
        {
            for (int asdasdasd = 0; asdasdasd < SeamapObjects.SeamapEntities.Length; asdasdasd++)
            {
                if (SeamapObjects.SeamapEntities[asdasdasd] != null)
                {
                    SeamapObjects.SeamapEntities[asdasdasd].Draw(spriteBatch);
                }
            }
            for (int asdasdasd = 0; asdasdasd < SeamapObjects.SeamapEntities.Length; asdasdasd++)
            {
                if (SeamapObjects.SeamapEntities[asdasdasd] != null)
                {
                    SeamapObjects.SeamapEntities[asdasdasd].PostDraw(spriteBatch);
                }
            }
        }

        public static int seamapWidth = 5000;
        public static int seamapHeight = 5000;

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

            WaterShader.Parameters["uTime"].SetValue(Main.GameUpdateCount);

            WaterShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, seamapWidth, seamapHeight), colour);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        #endregion
    }
}
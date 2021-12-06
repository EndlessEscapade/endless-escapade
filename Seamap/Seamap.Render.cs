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

            RenderWater(spriteBatch); //Layer 0
            RenderEntities(spriteBatch); //Layer 1, postdraw layer 2

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            RenderSeamapUI(spriteBatch); //Layer 3

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            //EEMod.DrawText();
        }

        public static void RenderSeamapUI(SpriteBatch spriteBatch)
        {
            #region Rendering ship healthbar
            Texture2D healthBar = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/HealthbarBg").Value;
            Texture2D healthBarFill = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/HealthbarFill").Value;

            spriteBatch.Draw(healthBar, new Vector2(Main.screenWidth - 200, 40), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.Draw(healthBarFill, new Vector2(Main.screenWidth - 200, 40), 
                new Rectangle(0, 0, (int)(SeamapObjects.localship.shipHelth / SeamapObjects.localship.ShipHelthMax) * 116, 40), 
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            #endregion 

            #region Rendering cannonball target
            Texture2D targetTex = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Target").Value;

            spriteBatch.Draw(targetTex, SeamapObjects.localship.Center + (Vector2.Normalize(Main.MouseWorld - SeamapObjects.localship.Center) * 128) - Main.screenPosition, null, Color.White, Main.GameUpdateCount / 120f, targetTex.TextureCenter(), 1, SpriteEffects.None, 0);
            #endregion
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
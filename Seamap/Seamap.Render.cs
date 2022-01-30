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
using EEMod.Net;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;

namespace EEMod.Seamap.SeamapContent
{
    public partial class Seamap
    {
        static EEPlayer modPlayer
        {
            get => Main.LocalPlayer.GetModPlayer<EEPlayer>();
        }

        public static RenderTarget2D shadowRT;

        public static void Render()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            #region Controlling brightness
            /*if (Main.dayTime)
            {
                if (Main.time <= 200)
                    brightness += 0.0025f;

                if (Main.time >= 52000 && brightness > 0.1f)
                    brightness -= 0.0025f;

                if (Main.time > 2000 && Main.time < 52000)
                    brightness = 1f;
            }
            else
            {
                brightness = 0.5f;
            }*/

            brightness = 1f;


            /*if (Main.dayTime && !isStorming)
            {
                brightness += ((1 - brightness) / 10f);
            }
            if (Main.dayTime && isStorming)
            {
                brightness += ((0.8f - brightness) / 10f);
            }
            if (!Main.dayTime && !isStorming)
            {
                brightness += ((0.7f - brightness) / 10f);
            }
            if (!Main.dayTime && isStorming)
            {
                brightness += ((0.7f - brightness) / 10f);
            }*/


            if (Main.time % 1000 == 0)
            {
                if (Main.rand.NextBool(10)) isStorming = !isStorming;
            }

            #endregion

            RenderWater(spriteBatch); //Layer 0

            RenderEntities(spriteBatch); //Layer 1, postdraw layer 2


            /*if (Main.spriteBatch != null && shadowRT != null)
            {
                spriteBatch.End();

                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(shadowRT);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin();

                for (int i = 0; i < SeamapObjects.SeamapEntities.Length; i++)
                {
                    if (SeamapObjects.SeamapEntities[i] is Seagull)
                    {
                        Seagull gull = SeamapObjects.SeamapEntities[i] as Seagull;

                        Main.spriteBatch.Draw(gull.texture, gull.position.ForDraw() + new Vector2(0, 40), gull.rect, Color.Black, 0, gull.rect.Size() / 2, gull.scale, SpriteEffects.None, 0f);
                    }
                }

                spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

                if (shadowRT != null)
                {
                    spriteBatch.Draw(shadowRT, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

                    spriteBatch.Draw(shadowRT, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                }
            }*/

            spriteBatch.End();

            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Particles/Square").Value;

            Color SeaColour = new Color(28 / 255f, 118 / 255f, 186 / 255f);

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos - Main.screenPosition;

            float weatherDensity = 0f;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            SeamapCloudShader.Parameters["cloudNoisemap"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/CloudNoise").Value);
            SeamapCloudShader.Parameters["densityNoisemap"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/SeamapNoise").Value);

            SeamapCloudShader.Parameters["cloudsColor4"].SetValue((Color.Black * (0.1f + (weatherDensity * 0.1f))).ToVector4());
            SeamapCloudShader.Parameters["cloudsColor3"].SetValue((Color.Black * (0.1f + (weatherDensity * 0.1f))).ToVector4());
            SeamapCloudShader.Parameters["cloudsColor2"].SetValue((Color.Black * (0.1f + (weatherDensity * 0.1f))).ToVector4());
            SeamapCloudShader.Parameters["cloudsColor1"].SetValue((Color.Black * (0.1f + (weatherDensity * 0.1f))).ToVector4());

            SeamapCloudShader.Parameters["wind"].SetValue(new Vector2(Main.GameUpdateCount / 4800f, (Main.GameUpdateCount / 4800f) * 0.6f));

            SeamapCloudShader.Parameters["weatherDensity"].SetValue(weatherDensity);
            SeamapCloudShader.Parameters["stepsX"].SetValue(5f);
            SeamapCloudShader.Parameters["stepsY"].SetValue(4.8f);

            SeamapCloudShader.Parameters["vec"].SetValue(new Vector2(1000, 600));

            for (int i = 0; i < seamapWidth / 1000; i++)
            {
                for (float j = -0.6f; j < seamapHeight / 1000; j += 0.6f)
                {
                    Vector2 arrayOffset = new Vector2(i, j);

                    SeamapCloudShader.Parameters["arrayOffset"].SetValue(arrayOffset);
                    SeamapCloudShader.CurrentTechnique.Passes[0].Apply();

                    spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + (i * 1000), (int)toScreen.Y + (int)(j * 1000) + 100, 1000, 600), Color.White);
                }
            }


            SeamapCloudShader.Parameters["cloudsColor4"].SetValue((new Color(218, 221, 237).LightSeamap() * 1f).ToVector4());
            SeamapCloudShader.Parameters["cloudsColor3"].SetValue((new Color(153, 195, 245).LightSeamap() * 0.9f).ToVector4());
            SeamapCloudShader.Parameters["cloudsColor2"].SetValue((new Color(153, 195, 245).LightSeamap() * 0.75f).ToVector4());
            SeamapCloudShader.Parameters["cloudsColor1"].SetValue((new Color(138, 169, 201).LightSeamap() * 0.7f).ToVector4());

            //SeamapCloudShader.Parameters["cloudsColor4"].SetValue((new Color(242, 243, 249) * 1f).ToVector4());
            //SeamapCloudShader.Parameters["cloudsColor3"].SetValue((new Color(218, 221, 237) * 0.9f).ToVector4());
            //SeamapCloudShader.Parameters["cloudsColor2"].SetValue((new Color(153, 195, 245) * 0.75f).ToVector4());
            //SeamapCloudShader.Parameters["cloudsColor1"].SetValue((new Color(138, 169, 201) * 0.7f).ToVector4());

            //SeamapCloudShader.Parameters["cloudsColor4"].SetValue((new Color(153, 195, 245) * 0.85f).ToVector4());
            //SeamapCloudShader.Parameters["cloudsColor3"].SetValue((new Color(153, 195, 245) * 0.8f).ToVector4());
            //SeamapCloudShader.Parameters["cloudsColor2"].SetValue((new Color(153, 195, 245) * 0.75f).ToVector4());
            //SeamapCloudShader.Parameters["cloudsColor1"].SetValue((new Color(138, 169, 201) * 0.7f).ToVector4());

            for (int i = 0; i < seamapWidth / 1000; i++)
            {
                for (float j = -0.6f; j < seamapHeight / 1000; j += 0.6f)
                {
                    Vector2 arrayOffset = new Vector2(i, j);

                    SeamapCloudShader.Parameters["arrayOffset"].SetValue(arrayOffset);
                    SeamapCloudShader.CurrentTechnique.Passes[0].Apply();

                    spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + (i * 1000), (int)toScreen.Y + (int)(j * 1000), 1000, 600), Color.White);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            SeamapBorderVignette.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, 5000, 4800), Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            if(!Main.hideUI) RenderSeamapUI(spriteBatch); //Layer 3

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

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
            static int CompareSeamapEntities(SeamapObject a, SeamapObject b) => a?.Bottom.Y.CompareTo(b?.Bottom.Y ?? 0f) ?? 0;
            //static bool InactiveOrUnused(SeamapObject obj) => obj?.active != true;

            var source = SeamapObjects.SeamapEntities;

            SeamapObject[] toDraw = source.Where(p => p?.active == true).ToArray();
            Array.Sort(toDraw, CompareSeamapEntities);
            
            foreach(SeamapObject entity in toDraw)
            {
                entity.Draw(spriteBatch);
            }

            foreach(SeamapObject entity in toDraw)
            {
                entity.PostDraw(spriteBatch);
            }
        }

        public static int seamapWidth = 5000;
        public static int seamapHeight = 5000;

        public static float brightness;
        public static bool isStorming;

        #region Seamap water
        static void RenderWater(SpriteBatch spriteBatch)
        {
            //EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Particles/Square").Value;

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos - Main.screenPosition;


            WaterShaderBase.Parameters["icyWaterColor"].SetValue(new Color(36, 46, 199).LightSeamap().ToVector4());
            WaterShaderBase.Parameters["neutralWaterColor"].SetValue(new Color(28, 118, 186).LightSeamap().ToVector4());
            WaterShaderBase.Parameters["tropicalWaterColor"].SetValue(new Color(64, 180, 217).LightSeamap().ToVector4());

            WaterShaderBase.Parameters["densityNoisemap"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/SeamapNoise").Value);

            WaterShaderBase.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, seamapWidth, seamapHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            for(int i = 0; i < SeamapObjects.SeamapEntities.Length; i++)
            {
                if(SeamapObjects.SeamapEntities[i] != null && SeamapObjects.SeamapEntities[i].active)
                {
                    if(SeamapObjects.SeamapEntities[i] is Island)
                    {
                        Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSquish").Value, SeamapObjects.SeamapEntities[i].Center - Main.screenPosition, new Color(64, 180, 217) * 0.4f, SeamapObjects.SeamapEntities[i].texture.Width * 2f / 150f);
                    }
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);


            WaterShader.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/DotNoise2Squish").Value);

            WaterShader.Parameters["baseWaterColor"].SetValue(new Color(0, 0, 0).LightSeamap().ToVector4());
            WaterShader.Parameters["highlightColor"].SetValue(new Color(5, 5, 5).LightSeamap().ToVector4());

            WaterShader.Parameters["sinVal"].SetValue(Main.GameUpdateCount / 1500f); 
            WaterShader.Parameters["width"].SetValue(1000);
            WaterShader.Parameters["height"].SetValue(600);

            WaterShader.CurrentTechnique.Passes[0].Apply();

            for (int i = 0; i < seamapWidth / 1000; i++)
            {
                for (float j = 0; j < seamapHeight / 1000; j += 0.6f)
                {
                    Vector2 arrayOffset = new Vector2(i, j);

                    spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + (i * 1000), (int)toScreen.Y + (int)(j * 1000), 1000, 600), new Color(0.1f, 0.1f, 0.1f, 0.1f).LightSeamap());
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        #endregion
    }

    public static class SeamapExtensions
    {
        public static Color LightSeamap(this Color color) => Color.Lerp(color, Color.Black, 1 - Seamap.brightness);
    }
}
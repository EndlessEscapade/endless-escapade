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

        public static int seamapWidth = 5000;
        public static int seamapHeight = 5000;

        public static float brightness;
        public static bool isStorming;

        public static void Render()
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapUpdateCount <= 0) return;

            SpriteBatch spriteBatch = Main.spriteBatch;

            Main.screenPosition = SeamapObjects.localship.Center + new Vector2(-Main.screenWidth / 2f, -Main.screenHeight / 2f);

            Main.screenPosition.X = MathHelper.Clamp(Main.screenPosition.X, 0, seamapWidth - Main.screenWidth);
            Main.screenPosition.Y = MathHelper.Clamp(Main.screenPosition.Y, 0, seamapHeight - 200 - Main.screenHeight);

            #region Controlling brightness + weather

            CalculateBrightness();

            if (Main.time % 1000 == 0)
                if (Main.rand.NextBool(10)) isStorming = !isStorming;

            #endregion


            spriteBatch.Begin();


            RenderWater(spriteBatch); //Layer 0


            RenderEntities(spriteBatch); //Layer 1, postdraw layer 2


            RenderClouds(spriteBatch); //Layer 3


            if (!Main.hideUI) RenderSeamapUI(spriteBatch); //Layer 4


            spriteBatch.End();
        }

        public static void RenderSeamapUI(SpriteBatch spriteBatch)
        {
            #region Rendering OCEAN screen thingy

            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapUpdateCount > 10 && Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapUpdateCount <= 190)
            {
                int updateCount = Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapUpdateCount;

                Texture2D oceanLogo = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/OceanScreen").Value;

                float yOffset = (updateCount <= 70 ? ((float)Math.Sin((updateCount - 10) * 1.57f / 60f) * 240f) - 120 : (updateCount <= 130 ? 120 : ((float)Math.Sin((updateCount - 70) * 1.57f / 60f) * 240f) - 120));

                spriteBatch.Draw(oceanLogo, new Vector2(Main.screenWidth / 2, yOffset), null, Color.White, 0, new Vector2(186, 92), 1, SpriteEffects.None, 0);
            }

            #endregion

            #region Rendering ship healthbar
            Texture2D healthBar = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/HealthbarBg").Value;
            Texture2D healthBarFill = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/HealthbarFill").Value;

            spriteBatch.Draw(healthBar, new Vector2(Main.screenWidth - 200, 40), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.Draw(healthBarFill, new Vector2(Main.screenWidth - 200, 40),
                new Rectangle(0, 0, (int)((SeamapObjects.localship.shipHelth / SeamapObjects.localship.ShipHelthMax) * 116), 40),
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            #endregion 

            #region Rendering cannonball target
            Texture2D targetTex = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Target").Value;

            spriteBatch.Draw(targetTex, SeamapObjects.localship.Center + (Vector2.Normalize(Main.MouseWorld - SeamapObjects.localship.Center) * 128) - Main.screenPosition, null, Color.White, Main.GameUpdateCount / 120f, targetTex.TextureCenter(), 1, SpriteEffects.None, 0);
            #endregion
        }

        public static float weatherDensity;

        public static void RenderEntities(SpriteBatch spriteBatch)
        {
            static int CompareSeamapEntities(SeamapObject a, SeamapObject b) => a?.Bottom.Y.CompareTo(b?.Bottom.Y ?? 0f) ?? 0;
            //static bool InactiveOrUnused(SeamapObject obj) => obj?.active != true;

            var source = SeamapObjects.SeamapEntities;

            SeamapObject[] toDraw = source.Where(p => p?.active == true).ToArray();
            Array.Sort(toDraw, CompareSeamapEntities);

            foreach (SeamapObject entity in toDraw)
            {
                entity.Draw(spriteBatch);
            }

            foreach (SeamapObject entity in toDraw)
            {
                entity.PostDraw(spriteBatch);
            }
        }

        static void RenderWater(SpriteBatch spriteBatch)
        {
            //EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Particles/Square").Value;

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos - Main.screenPosition;


            //Tropical water palette
            //WaterShaderBase.Parameters["icyWaterColor"].SetValue(new Color(6, 90, 133).LightSeamap().ToVector4());
            //WaterShaderBase.Parameters["neutralWaterColor"].SetValue(new Color(0, 141, 161).LightSeamap().ToVector4());
            //WaterShaderBase.Parameters["tropicalWaterColor"].SetValue(new Color(19, 216, 205).LightSeamap().ToVector4());

            //Storming palette
            //WaterShaderBase.Parameters["icyWaterColor"].SetValue(new Color(34, 30, 45).LightSeamap().ToVector4());
            //WaterShaderBase.Parameters["neutralWaterColor"].SetValue(new Color(44, 44, 68).LightSeamap().ToVector4());
            //WaterShaderBase.Parameters["tropicalWaterColor"].SetValue(new Color(53, 65, 77).LightSeamap().ToVector4());

            //Neutral water palette
            WaterShaderBase.Parameters["icyWaterColor"].SetValue(Vector4.Lerp(new Color(52, 75, 136).LightSeamap().ToVector4(), new Color(34, 30, 45).LightSeamap().ToVector4(), weatherDensity));
            WaterShaderBase.Parameters["neutralWaterColor"].SetValue(Vector4.Lerp(new Color(36, 119, 182).LightSeamap().ToVector4(), new Color(44, 44, 68).LightSeamap().ToVector4(), weatherDensity));
            WaterShaderBase.Parameters["tropicalWaterColor"].SetValue(Vector4.Lerp(new Color(96, 178, 220).LightSeamap().ToVector4(), new Color(53, 65, 77).LightSeamap().ToVector4(), weatherDensity));

            WaterShaderBase.Parameters["densityNoisemap"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/SeamapNoise").Value);

            WaterShaderBase.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, seamapWidth, seamapHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (var entity in SeamapObjects.ActiveEntities)
            {
                if (entity is not Island)
                    continue;

                Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradientSquish").Value, entity.Center - Main.screenPosition, new Color(64, 180, 217) * 0.4f, entity.texture.Width * 2f / 150f);
            }


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);


            WaterShader.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/DotNoise2Squish").Value);

            WaterShader.Parameters["baseWaterColor"].SetValue(new Color(0, 0, 0).LightSeamap().ToVector4());
            WaterShader.Parameters["highlightColor"].SetValue(new Color(5, 5, 5).LightSeamap().ToVector4()); //8,8,8 for storms

            WaterShader.Parameters["sinVal"].SetValue(Main.GameUpdateCount / 1500f); // divided by 1000 for storms

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
        
        static void RenderClouds(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Particles/Square").Value;

            Color SeaColour = new Color(28 / 255f, 118 / 255f, 186 / 255f);

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos - Main.screenPosition;

            if (isStorming && weatherDensity < 1f) weatherDensity += 0.001f;
            if (!isStorming && weatherDensity > 0f) weatherDensity -= 0.001f;

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


            SeamapCloudShader.Parameters["cloudsColor4"].SetValue(Vector4.Lerp((new Color(218, 221, 237).LightSeamap() * 1f).ToVector4(), (new Color(142, 143, 156).LightSeamap() * 1f).ToVector4(), weatherDensity));
            SeamapCloudShader.Parameters["cloudsColor3"].SetValue(Vector4.Lerp((new Color(153, 195, 245).LightSeamap() * 0.9f).ToVector4(), (new Color(92, 117, 162).LightSeamap() * 0.9f).ToVector4(), weatherDensity));
            SeamapCloudShader.Parameters["cloudsColor2"].SetValue(Vector4.Lerp((new Color(153, 195, 245).LightSeamap() * 0.75f).ToVector4(), (new Color(85, 104, 133).LightSeamap() * 0.9f).ToVector4(), weatherDensity));
            SeamapCloudShader.Parameters["cloudsColor1"].SetValue(Vector4.Lerp((new Color(138, 169, 201).LightSeamap() * 0.7f).ToVector4(), (new Color(49, 76, 119).LightSeamap() * 0.9f).ToVector4(), weatherDensity));

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

            SeamapBorderVignette.Parameters["color"].SetValue(Vector4.Lerp(Color.White.LightSeamap().ToVector4(), Color.Gray.LightSeamap().ToVector4(), weatherDensity));

            SeamapBorderVignette.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, 5000, 4800), Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        static void CalculateBrightness()
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapUpdateCount == 1)
                brightness = (Main.dayTime ? (isStorming ? 0.8f : 1f) : (isStorming ? 0.5f : 0.3f));

            if (!isStorming)
            {
                if (Main.dayTime)
                {
                    if (Main.time < 52000 && brightness < 1f)
                        brightness += 0.0025f;

                    if (Main.time >= 52000 && brightness > 0.5f)
                        brightness -= 0.0025f;
                }
                else
                {
                    brightness = 0.5f;
                }
            }
            else
            {
                if (Main.dayTime)
                {
                    if (Main.time < 52000 && brightness < 0.8f)
                        brightness += 0.0025f;

                    if (Main.time >= 52000 && brightness > 0.8f)
                        brightness -= 0.0025f;
                }
                else
                {
                    brightness = 0.3f;
                }
            }
        }
    }

    public static class SeamapExtensions
    {
        public static Color LightSeamap(this Color color) => Color.Lerp(color, Color.Black, 1f - Seamap.brightness);
    }
}
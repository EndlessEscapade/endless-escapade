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

            #region Controlling brightness
            /*if (Main.dayTime)
            {
                if (Main.time <= 200)
                    brightness += 0.0025f;

                if (Main.time >= 52000 && brightness > 0.1f)
                    brightness -= 0.0025f;

                if (Main.time > 2000 && Main.time < 52000)
                    brightness = 0.5f;
            }*/
            //else
            //{
                brightness = 0.1f;
            //}

            if (Main.time % 1000 == 0)
            {
                if (Main.rand.NextBool(10)) isStorming = !isStorming;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncBrightness, brightness);
            }
            #endregion

            seamapDrawColor = new Color(Color.White.R * brightness, Color.White.G * brightness, Color.White.B * brightness, Color.White.A);

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
            SortedSet<SeamapObject> drawObjs = new SortedSet<SeamapObject>(SeamapObjects.SeamapEntities, Comparer<SeamapObject>.Create((a, b) => (a?.Bottom.Y.CompareTo(b?.Bottom.Y ?? 0) ?? 0)));

            foreach(SeamapObject drawObj in drawObjs)
            { 
                if (drawObj != null)
                {
                    drawObj.Draw(spriteBatch);
                }
            }
            foreach (SeamapObject drawObj in drawObjs)
            {
                if (drawObj != null)
                {
                    drawObj.PostDraw(spriteBatch);
                }
            }
        }

        public static int seamapWidth = 5000;
        public static int seamapHeight = 5000;

        public static float brightness;
        public static bool isStorming;

        public static Color seamapDrawColor;

        #region Seamap water
        static void RenderWater(SpriteBatch spriteBatch)
        {
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Particles/Square").Value;

            Color SeaColour = new Color(28 / 255f, 118 / 255f, 186 / 255f);

            Vector2 pos = Vector2.Zero;
            Vector2 toScreen = pos.ForDraw();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + (i * seamapWidth / 5), (int)toScreen.Y + (j * seamapHeight / 5), seamapWidth / 5, seamapHeight / 5), SeaColour);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            //Color colour = Lighting.GetColor((int)(SeamapObjects.localship.Center.X / 16f), (int)(SeamapObjects.localship.Center.Y / 16f));

            //if(colour.R < 1f)
            //{
            //colour = new Color(0.8f, 0.8f, 0.8f);
            //}

            //Color SeaColour = new Color(40 / 255f, 167 / 255f, 194 / 255f);
            //Color SeaHighlight = new Color(83 / 255f, 222 / 255f, 230 / 255f);

            WaterShader.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/DotNoise2Squish").Value);

            WaterShader.Parameters["baseWaterColor"].SetValue(new Color(0, 0, 0).ToVector4());
            WaterShader.Parameters["highlightColor"].SetValue(new Color(5, 5, 5).ToVector4());

            WaterShader.Parameters["sinVal"].SetValue(Main.GameUpdateCount / 1500f); 
            WaterShader.Parameters["width"].SetValue(1000);
            WaterShader.Parameters["height"].SetValue(600);

            //WaterShader.Parameters["waveSpeed"].SetValue(3);

            //WaterShader.Parameters["resolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

            WaterShader.CurrentTechnique.Passes[0].Apply();

            for (int i = 0; i < 5; i++)
            {
                for (float j = 0; j < 5; j += 0.6f)
                {
                    spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + (i * 1000), (int)toScreen.Y + (int)(j * 1000), 1000, 600), new Color(0.1f, 0.1f, 0.1f, 0.1f));
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        #endregion
    }
}
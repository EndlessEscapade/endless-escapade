using EEMod.Extensions;
using EEMod.Systems;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class BulbousPlants : ModSystem
    {
        private void HandleBulbDraw(Vector2 position)
        {
            Lighting.AddLight(position, new Vector3(0, 0.1f, 0.4f));
            Vector2 tilePos = position / 16;
            int spread = 8;

            int down = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X, (int)tilePos.Y, 1, 50);
            int up = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X, (int)tilePos.Y, -1, 50);
            int down2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, 1, 50);
            int up2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, -1, 50);
            int down3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, 1, 50);
            int up3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, -1, 50);

            Vector2 p1 = new Vector2(tilePos.X * 16, down * 16);
            Vector2 p2 = new Vector2(tilePos.X * 16, up * 16);
            Vector2 p3 = new Vector2((tilePos.X - spread) * 16, down2 * 16);
            Vector2 p4 = new Vector2((tilePos.X - spread) * 16, up2 * 16);
            Vector2 p5 = new Vector2((tilePos.X + spread) * 16, down3 * 16);
            Vector2 p6 = new Vector2((tilePos.X + spread) * 16, up3 * 16);

            Texture2D BlueLight = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Vines/LightBlue").Value;
            Texture2D vineTexture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Vines/BigVine").Value;

            float Addon = 10;
            float cockandbol = 0.8f;
            float bolandcock = 7f;
            float sineInt = Main.GameUpdateCount / 100f;

            if (p1.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p1, position + new Vector2(0, 65), 
                    Vector2.Lerp(p1, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p1 + new Vector2(0, Addon), position + new Vector2(0, 65 + Addon), 
                    Vector2.Lerp(p1, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p2.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p2, position + new Vector2(0, -65), 
                    Vector2.Lerp(p2, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.5f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p2 + new Vector2(0, Addon), position + new Vector2(0, -65 + Addon),
                    Vector2.Lerp(p2, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.5f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p3.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p3, position + new Vector2(-60, 55), 
                    Vector2.Lerp(p3, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.2f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p3 + new Vector2(0, Addon), position + new Vector2(-60, 55 + Addon), 
                    Vector2.Lerp(p3, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.2f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p4.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p4, position + new Vector2(-60, -55), 
                    Vector2.Lerp(p4, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.8f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p4 + new Vector2(0, Addon), position + new Vector2(-60, -55 + Addon), 
                    Vector2.Lerp(p4, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.8f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p5.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p5, position + new Vector2(60, 55), 
                    Vector2.Lerp(p5, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.9f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p5 + new Vector2(0, Addon), position + new Vector2(60, 55 + Addon), 
                    Vector2.Lerp(p5, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.9f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }
            if (p6.Y >= 1)
            {
                Helpers.DrawBezier(vineTexture, Color.White, p6, position + new Vector2(60, -55), 
                    Vector2.Lerp(p6, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2.2f) * 40), cockandbol, (float)Math.PI / 2, true, 1, false, true);
                Helpers.DrawBezier(BlueLight, "", Color.White, p6 + new Vector2(0, Addon), position + new Vector2(60, -55 + Addon), 
                    Vector2.Lerp(p6, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2.2f) * 40 + Addon), bolandcock, MathHelper.PiOver2, false, true);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            EEMod.Noise2DShift.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise").Value);
            EEMod.Noise2DShift.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/WormNoisePixelated").Value);
            EEMod.Noise2DShift.Parameters["yCoord"].SetValue((float)Math.Sin(sineInt) * 0.2f);
            EEMod.Noise2DShift.Parameters["xCoord"].SetValue((float)Math.Cos(sineInt) * 0.2f);

            EEMod.Noise2DShift.CurrentTechnique.Passes[0].Apply();

            EEMod.Noise2DShift.Parameters["lightColour"].SetValue(Lighting.GetColor((int)tilePos.X, (int)tilePos.Y).ToVector3());
            
            Texture2D tex = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/BulbousBall").Value;
            
            int SineTicks = (int)(Math.Sin(sineInt * 4) * 10);
            float SineTicksF = (float)(Math.Sin(sineInt /8f + position.X) * 10);
            int CosTicks = (int)(Math.Cos(sineInt * 4) * 10);
            
            Main.spriteBatch.Draw(tex, new Rectangle((int)position.ForDraw().X, (int)position.ForDraw().Y + SineTicks, tex.Width + SineTicks, tex.Height + CosTicks), new Rectangle(0, 0, tex.Width + CosTicks, tex.Height + CosTicks), Color.White * 0, SineTicksF, tex.Bounds.Size() / 2, SpriteEffects.None, 0f);
            
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void PostDrawTiles() 
        { 
            /*
           for (int i = 0; i < EESubWorlds.BulbousTreePosition.Count; i++)
           {
               Vector2 pos = EESubWorlds.BulbousTreePosition[i] * 16;
               if (pos.ForDraw().LengthSquared() < 2000 * 2000)
                   HandleBulbDraw(pos);
           }*/
        }
    }
}
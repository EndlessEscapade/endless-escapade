using EEMod.Extensions;
using EEMod.Subworlds.CoralReefs;
using EEMod.Systems;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class LightingBuffer : ModSystem
    {
        internal readonly List<Vector2> _lightPoints = new List<Vector2>();
        internal readonly List<Color> _colorPoints = new List<Color>();

        public RenderTarget2D lightingTarget;

        internal static LightingBuffer Instance;

        public void UpdateLight()
        {
            if (_lightPoints.Count > 0)
            {
                _lightPoints.Clear();
                _colorPoints.Clear();
            }

            if (Main.LocalPlayer.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
            {
                bgAlpha += 0.02f;
            }
            else
            {
                bgAlpha -= 0.02f;
            }

            bgAlpha = MathHelper.Clamp(bgAlpha, 0f, 1f);
        }

        public override void OnWorldLoad()
        {
            bgAlpha = 0f;
        }

        public void PreDrawTiles()
        {
            if (Main.dedServ)
                return;

            if (SubworldSystem.IsActive<CoralReefs>() && bgAlpha > 0)
            {
                RenderTargetBinding[] oldtargets1 = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTarget(lightingTarget);
                Main.graphics.GraphicsDevice.Clear(Color.Black);

                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

                int Width = Main.screenWidth;
                int Height = Main.screenHeight;
                Vector2 SP = Main.screenPosition / 16;

                for (int i = 0; i < Width / 16; i++)
                {
                    for (int j = 0; j < Height / 16; j++)
                    {
                        Point p = new Point((int)SP.X + i, (int)SP.Y + j);
                        Color c = Lighting.GetColor(p.X, p.Y);

                        float num = (c.R + c.G + c.B) / 3f;

                        float num8 = Lighting.Brightness(p.X, p.Y);

                        bool funi = false;
                        if ((Framing.GetTileSafely(p.X, p.Y).Slope != SlopeType.Solid && (Framing.GetTileSafely(p.X, p.Y - 1).LiquidAmount > 0 || Framing.GetTileSafely(p.X, p.Y + 1).LiquidAmount > 0 || Framing.GetTileSafely(p.X + 1, p.Y).LiquidAmount > 0 || Framing.GetTileSafely(p.X - 1, p.Y).LiquidAmount > 0) || (Framing.GetTileSafely(p.X, p.Y).IsHalfBlock && Framing.GetTileSafely(p.X, p.Y - 1).LiquidAmount > 0)))
                        {
                            if (num8 > 0)
                            {
                                funi = true;
                            }
                        }

                        if(!funi)
                            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, new Rectangle((int)i, (int)j, 16, 16), c);
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets1);

                DrawReefsBackground();

                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            }
        }

        public void DrawReefsBackground()
        {
            Texture2D tex = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceFar").Value;
            Texture2D tex2 = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceMid").Value;
            Texture2D tex3 = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceClose").Value;

            Vector2 chunk1 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.8f, 0.3f)) / tex.Size();
            Vector2 chunk2 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.6f, 0.3f)) / tex2.Size();
            Vector2 chunk3 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.4f, 0.3f)) / tex3.Size();

            for (int i = (int)chunk1.X - 1; i <= (int)chunk1.X + 1; i++)
                for (int j = (int)chunk1.Y - 1; j <= (int)chunk1.Y + 1; j++)
                    global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                    tex,
                    new Vector2(tex.Width * i, tex.Height * j).ParalaxXY(new Vector2(-0.8f, -0.3f)), bgAlpha);

            for (int i = (int)chunk2.X - 1; i <= (int)chunk2.X + 1; i++)
                for (int j = (int)chunk2.Y - 1; j <= (int)chunk2.Y + 1; j++)
                    global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                    tex2,
                    new Vector2(tex2.Width * i, tex2.Height * j).ParalaxXY(new Vector2(-0.6f, -0.3f)), bgAlpha);

            for (int i = (int)chunk3.X - 1; i <= (int)chunk3.X + 1; i++)
                for (int j = (int)chunk3.Y - 1; j <= (int)chunk3.Y + 1; j++)
                    global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                    tex3,
                    new Vector2(tex3.Width * i, tex3.Height * j).ParalaxXY(new Vector2(-0.4f, -0.3f)), bgAlpha);
        }

        public void DrawWithBuffer(Texture2D texture, Vector2 position, float alpha)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            EEMod.LightingBuffer.Parameters["buffer"].SetValue(lightingTarget);
            EEMod.LightingBuffer.Parameters["screenPosition"].SetValue(position.ForDraw());
            EEMod.LightingBuffer.Parameters["texSize"].SetValue(texture.Bounds.Size());
            EEMod.LightingBuffer.Parameters["alpha"].SetValue(alpha);
            
            EEMod.LightingBuffer.CurrentTechnique.Passes[0].Apply();
            
            Main.spriteBatch.Draw(texture, position.ForDraw(), Color.White * bgAlpha);
            
            Main.spriteBatch.End();
        }

        public override void PostUpdateEverything()
        {
            UpdateLight();
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            Main.QueueMainThreadAction(() =>
            {
                lightingTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 16, Main.screenHeight / 16);
            });

            Instance = this;
        }

        public float bgAlpha;
    }
}
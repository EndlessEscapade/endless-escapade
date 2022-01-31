using EEMod.Extensions;
using EEMod.Systems;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        }

        public override void PreUpdateEntities()
        {
            if (Main.dedServ)
                return;
            RenderTargetBinding[] oldtargets1 = Main.graphics.GraphicsDevice.GetRenderTargets();
            Main.graphics.GraphicsDevice.SetRenderTarget(lightingTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Black);

            Main.spriteBatch.Begin();

            int Width = Main.screenWidth;
            int Height = Main.screenHeight;
            for (int i = 0; i < Width / 16; i++)
            {
                for (int j = 0; j < Height / 16; j++)
                {
                    Vector2 SP = Main.screenPosition / 16;
                    Point p = new Point((int)SP.X + i, (int)SP.Y + j);
                    Color c = Lighting.GetColor(p.X, p.Y);
                    Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, new Rectangle(i, j, 1, 1), c);
                }
            }

            EEMod.LightingBuffer.Parameters["buffer"].SetValue(lightingTarget);

            Main.spriteBatch.End();

            Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets1);
        }

        public event Action BufferCalls;

        public override void PostDrawTiles()
        {
            BufferCalls?.Invoke();
            BufferCalls = null;
        }

        public void DrawWithBuffer(Texture2D texture, Vector2 position, float alpha)
        {
            BufferCalls += () =>
            {
                Main.spriteBatch.Begin();

                EEMod.LightingBuffer.Parameters["screenPosition"].SetValue(position.ForDraw());
                EEMod.LightingBuffer.Parameters["texSize"].SetValue(texture.Bounds.Size());
                EEMod.LightingBuffer.Parameters["alpha"].SetValue(alpha);
                EEMod.LightingBuffer.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(texture, position.ForDraw(), Color.White);

                Main.spriteBatch.End();
            };
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
    }
}
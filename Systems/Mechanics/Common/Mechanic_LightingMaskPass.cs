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
    public class LightingBuffer : Mechanic
    {
        internal readonly List<Vector2> _lightPoints = new List<Vector2>();
        internal readonly List<Color> _colorPoints = new List<Color>();

        internal static LightingBuffer Instance;

        public void UpdateLight()
        {
            if (_lightPoints.Count > 0)
            {
                _lightPoints.Clear();
                _colorPoints.Clear();
            }
        }
        public event Action BufferCalls;

        public override void OnDraw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            BufferCalls?.Invoke();
            BufferCalls = null;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void DrawWithBuffer(Texture2D texture, Vector2 position, float alpha)
        {
            BufferCalls += () =>
            {
                EEMod.LightingBufferEffect.Parameters["screenPosition"].SetValue(position.ForDraw());
                EEMod.LightingBufferEffect.Parameters["texSize"].SetValue(texture.Bounds.Size());
                EEMod.LightingBufferEffect.Parameters["alpha"].SetValue(alpha);
                EEMod.LightingBufferEffect.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(texture, position.ForDraw(), Color.White);
            };
        }
        public override void OnUpdate()
        {
            UpdateLight();
        }

        public override void OnLoad()
        {
            Instance = this;
        }
        protected override Layer DrawLayering => Layer.None;
    }
}
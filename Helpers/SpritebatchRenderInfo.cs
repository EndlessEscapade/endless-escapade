using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod
{
    /// <summary>
    /// Gets the <see cref="SpriteSortMode"/>, <see cref="Microsoft.Xna.Framework.Graphics.BlendState"/>, <see cref="Microsoft.Xna.Framework.Graphics.SamplerState"/>, <see cref="Microsoft.Xna.Framework.Graphics.DepthStencilState"/>, <see cref="Microsoft.Xna.Framework.Graphics.RasterizerState"/><br />
    /// and <see cref="Microsoft.Xna.Framework.Graphics.Effect"/> inside a spritebatch.
    /// </summary>
    public struct SpritebatchRenderInfo
    {
        public SpriteSortMode SortMode { get; set; }
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public Effect Effect { get; set; }
        public Matrix Matrix { get; set; }

        public SpritebatchRenderInfo(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException(nameof(spriteBatch));
            SortMode = (SpriteSortMode)sortModeField.GetValue(spriteBatch);
            BlendState = (BlendState)blendStateField.GetValue(spriteBatch);
            SamplerState = (SamplerState)samplerStateField.GetValue(spriteBatch);
            DepthStencilState = (DepthStencilState)depthStencilStateField.GetValue(spriteBatch);
            RasterizerState = (RasterizerState)rasterizerStateField.GetValue(spriteBatch);
            Effect = (Effect)effectField.GetValue(spriteBatch);
            Matrix = (Matrix)transformMatrixField.GetValue(spriteBatch);
        }

        public void ApplyBegin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix);
        }

        private static FieldInfo sortModeField, blendStateField, samplerStateField, depthStencilStateField, rasterizerStateField, effectField, transformMatrixField;
        static SpritebatchRenderInfo()
        {
            Type type = typeof(SpriteBatch);
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            sortModeField = type.GetField("sortMode", flags);
            blendStateField = type.GetField("blendState", flags);
            samplerStateField = type.GetField("samplerState", flags);
            depthStencilStateField = type.GetField("depthStencilState", flags);
            rasterizerStateField = type.GetField("rasterizerState", flags);
            effectField = type.GetField("customEffect", flags);
            transformMatrixField = type.GetField("transformMatrix", flags);
        }
    }
}

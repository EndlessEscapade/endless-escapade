using System.Collections.Generic;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace EEMod.Prim
{
    public class PrimTrailManager
    {
        public List<Primitive> _trails = new List<Primitive>();

        public RenderTarget2D primTargetPixelated;
        public RenderTarget2D primTargetUnpixelated;

        public RenderTarget2D primTargetBTPixelated;
        public RenderTarget2D primTargetBTUnpixelated;

        public void Load()
        {
            _trails.Clear();

            Main.QueueMainThreadAction(() => 
            {
                primTargetPixelated = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                primTargetUnpixelated = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);

                primTargetBTPixelated = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                primTargetBTUnpixelated = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            });
        }

        public void DrawTrailsAboveTiles()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            if (PrimitiveSystem.primitives.primTargetPixelated != null)
                Main.spriteBatch.Draw(PrimitiveSystem.primitives.primTargetPixelated, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            if (PrimitiveSystem.primitives.primTargetUnpixelated != null)
                Main.spriteBatch.Draw(PrimitiveSystem.primitives.primTargetUnpixelated, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            Main.spriteBatch.End();
        }

        public void DrawTrailsBehindTiles()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            if (PrimitiveSystem.primitives.primTargetBTPixelated != null)
                Main.spriteBatch.Draw(PrimitiveSystem.primitives.primTargetBTPixelated, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            if (PrimitiveSystem.primitives.primTargetBTUnpixelated != null)
                Main.spriteBatch.Draw(PrimitiveSystem.primitives.primTargetBTUnpixelated, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            Main.spriteBatch.End();
        }

        public void UpdateTrails()
        {
            foreach (Primitive trail in _trails.ToArray())
            {
                trail.Update();
            }
        }

        public void CreateTrail(Primitive PT) => _trails.Add(PT);

        public void ClearTrailsOn(Entity entity)
        {
            //Primitive[] temp = _trails.ToArray();

            for (int i = 0; i < _trails.Count; i++)
            {
                if (_trails[i] == null)
                {
                    //_trails.RemoveAt(i);
                    continue;
                }
                if (_trails[i].BindableEntity == null)
                { 
                    //_trails.RemoveAt(i);
                    continue;
                }

                if (_trails[i].BindableEntity.whoAmI == entity.whoAmI)
                {
                    _trails[i].OnDestroy();
                }
            }

            //_trails = new List<Primitive>(temp);
        }
    }
}
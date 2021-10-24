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

        public RenderTarget2D primTargetNPC;

        public void Load()
        {
            Main.QueueMainThreadAction(() => 
            {
                primTargetNPC = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
            });
        }

        public void DrawTrailsAboveTiles()
        {
            /*bool lolxd = (bool)typeof(SpriteBatch).GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.spriteBatch);

            if (lolxd) Main.spriteBatch.End();

            RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(primTargetNPC);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin();

            foreach (Primitive trail in _trails.ToArray())
            {
                if (!trail.behindTiles && !trail.ManualDraw)
                    trail.Draw();
            }

            Main.spriteBatch.End();

            Main.graphics.GraphicsDevice.SetRenderTargets(bindings);*/

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            if (PrimitiveSystem.primitives.primTargetNPC != null)
            {
                Main.spriteBatch.Draw(PrimitiveSystem.primitives.primTargetNPC, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            }

            Main.spriteBatch.End();
        }

        public void DrawTrailsBehindTiles()
        {
            foreach (Primitive trail in _trails.ToArray())
            {
                if(trail.behindTiles && !trail.ManualDraw)
                    trail.Draw();
            }
        }

        public void UpdateTrailsBehindTiles()
        {
            foreach (Primitive trail in _trails.ToArray())
            {
                if (trail.behindTiles)
                    trail.Update();
            }
        }

        public void UpdateTrailsAboveTiles()
        {
            foreach (Primitive trail in _trails.ToArray())
            {
                if (!trail.behindTiles)
                    trail.Update();
            }
        }

        public void CreateTrail(Primitive PT) => _trails.Add(PT);
    }
}
using System.Collections.Generic;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace EEMod.Prim
{
    public class PrimTrailManager
    {
        public List<PrimTrail> _trails = new List<PrimTrail>();

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
            bool lolxd = (bool)typeof(SpriteBatch).GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.spriteBatch);

            if (lolxd) Main.spriteBatch.End();

            RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(primTargetNPC);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin();

            foreach (PrimTrail trail in _trails.ToArray())
            {
                if (!trail.behindTiles && !trail.ManualDraw)
                    trail.Draw();
            }

            Main.spriteBatch.End();

            Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
        }

        public void DrawTrailsBehindTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if(trail.behindTiles && !trail.ManualDraw)
                    trail.Draw();
            }
        }

        public void UpdateTrailsBehindTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if (trail.behindTiles)
                    trail.Update();
            }
        }

        public void UpdateTrailsAboveTiles()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                if (!trail.behindTiles)
                    trail.Update();
            }
        }

        public void CreateTrail(PrimTrail PT) => _trails.Add(PT);
    }
}
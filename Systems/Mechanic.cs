using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public enum Layer
    {
        BehindTiles,
        AboveTiles
    }
    public class Mechanic : IComponent
    {
        public EEMod Singleton => ModContent.GetInstance<EEMod>();
        public float ElapsedTicks => Main.GameUpdateCount;
        protected virtual Layer DrawLayering => Layer.BehindTiles;
        public virtual void OnDraw() { }
        public virtual void OnUpdate() { }
        public virtual void OnLoad() { }

        public void Draw(SpriteBatch spritebatch)
        {
            OnDraw();
        }

        public void Update()
        {
            OnUpdate();
        }
        public void Load()
        {
            Singleton.Updatables.Add(this);
            switch(DrawLayering)
            {
                case Layer.BehindTiles:
                    Singleton.BeforeTiles += Draw;
                    break;
                case Layer.AboveTiles:
                    break;
            }
        }
        public Mechanic() { Load(); }
    }
}
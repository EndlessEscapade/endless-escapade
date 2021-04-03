using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EEMod.Autoloading;
using EEMod.Autoloading.AutoloadTypes;

namespace EEMod.Systems
{
    public enum Layer
    {
        None,
        BehindTiles,
        AboveTiles,
        NPCCache
    }
    public class Mechanic : IComponent, IAutoloadType
    {
        public EEMod Singleton => ModContent.GetInstance<EEMod>();
        public float ElapsedTicks => Main.GameUpdateCount;
        protected virtual Layer DrawLayering => Layer.BehindTiles;
        public virtual void OnDraw(SpriteBatch spriteBatch) { }
        public virtual void OnUpdate() { }
        public virtual void OnLoad() { }

        public void Draw(SpriteBatch spritebatch)
        {
            OnDraw(spritebatch);
        }

        public void Update()
        {
            OnUpdate();
        }
        public void Load()
        {
            Singleton.Updatables.Add(this);
            switch (DrawLayering)
            {
                case Layer.BehindTiles:
                    Singleton.BeforeTiles += Draw;
                    break;
                case Layer.AboveTiles:
                    Singleton.AfterTiles += Draw;
                    break;
                case Layer.NPCCache:
                    Singleton.BeforeNPCCache += Draw;
                    break;
            }
            OnLoad();
        }
        public Mechanic()
        {
            // Loaded in MechanicManager
        }
    }
}
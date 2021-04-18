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
        public EEMod Mod => ModContent.GetInstance<EEMod>();
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
            Mod.Updatables.Add(this);
            switch (DrawLayering)
            {
                case Layer.BehindTiles:
                    Mod.BeforeTiles += Draw;
                    break;
                case Layer.AboveTiles:
                    Mod.AfterTiles += Draw;
                    break;
                case Layer.NPCCache:
                    Mod.BeforeNPCCache += Draw;
                    break;
            }
            OnLoad();
        }

        public virtual void PostUpdateWorld() { }

        /// <summary>
        /// Called in <seealso cref="MechanicWorld.PostDrawTiles"/><br />
        /// Spritebatch must begin and end in this method.
        /// </summary>
        public virtual void PostDrawTiles() { }

        /// <summary>
        /// Basically PostUpdateProjectiles and PreUpdateItems
        /// </summary>
        public virtual void MidUpdateProjectileItem() { }

        /// <summary>
        /// PostUpdateNPCs and PreUpdateGore
        /// </summary>
        public virtual void MidUpdateNPCGore() { }

        public virtual void MidUpdateDustTime() { }

        public virtual void PreDrawNPCs() { }

        public virtual void PostDrawNPCs() { }

        public virtual void PreDrawProjectiles() { }

        public virtual void PostDrawProjectiles() { }

        public Mechanic()
        {
            // Loaded in MechanicManager
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

    public class MechanicAutoloadOptionsAttribute : Attribute
    {
        public string MechanicName { get; set; }
        public MechanicAutoloadOptionsAttribute()
        {

        }
    }

    public class Mechanic : IComponent, IAutoloadType
    {
        public EEMod Mod => ModContent.GetInstance<EEMod>();
        public float ElapsedTicks => Main.GameUpdateCount;
        protected virtual Layer DrawLayering => Layer.BehindTiles;

        /// <summary>
        /// The mechanic's identifier name, by default or if null or empty it's the class name
        /// </summary>
        public string Name { get; set; }
        public virtual void OnDraw(SpriteBatch spriteBatch) { }
        public virtual void OnUpdate() { }
        public virtual void OnLoad() { }
        public virtual void AddHooks() { }

        public static T GetMechanic<T>() where T : Mechanic => ModContent.GetInstance<T>();

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

            AddHooks();

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


        /// <summary>
        /// Called in <seealso cref="MechanicWorld.PostUpdate"/>
        /// </summary>
        public virtual void PostUpdateWorld() { }

        /// <summary>
        /// Called in <seealso cref="MechanicWorld.PostDrawTiles"/><br />
        /// Spritebatch must begin and end in this method.
        /// </summary>
        public virtual void PostDrawTiles() { }

        /// <summary>
        /// Called in <seealso cref="EEMod.MidUpdateProjectileItem"/><br />
        /// Basically PostUpdateProjectiles and PreUpdateItems
        /// </summary>
        public virtual void MidUpdateProjectileItem() { }

        /// <summary>
        /// Called in <seealso cref="EEMod.MidUpdateNPCGore"/><br />
        /// Basically PostUpdateNPCs and PreUpdateGore
        /// </summary>
        public virtual void MidUpdateNPCGore() { }

        /// <summary>
        /// Called in <seealso cref="EEMod.MidUpdateDustTime"/><br />
        /// Basically PostUpdateDust and PreUpdateTime
        /// </summary>
        public virtual void MidUpdateDustTime() { }

        public virtual void PreDrawNPCs() { }

        public virtual void PostDrawNPCs() { }

        /// <summary>
        /// In case of begining the spritebatch, it must also end.
        /// </summary>
        public virtual void PreDrawProjectiles() { }

        /// <summary>
        /// In case of begining the spritebatch, it must also end.
        /// </summary>
        public virtual void PostDrawProjectiles() { }

        public virtual void PreUpdateEntities() { }

        /// <summary>
        /// Called at <see cref="EEMod.UpdateMusic(ref int, ref MusicPriority)"/>
        /// </summary>
        /// <param name="music"></param>
        /// <param name="priority"></param>
        public virtual void UpdateMusic(ref int music, ref MusicPriority priority)
        {

        }

        public Mechanic()
        {
            // Loaded in MechanicManager
        }
    }
}
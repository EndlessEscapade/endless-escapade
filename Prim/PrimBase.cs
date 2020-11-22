using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System;
using EEMod.Effects;
using EEMod.Projectiles.Mage;
using static Terraria.ModLoader.ModContent;
using System.Reflection;
using EEMod.Projectiles.Ranged;
using EEMod.Projectiles.Melee;
using EEMod.NPCs.CoralReefs;
namespace EEMod.Prim
{
    public class PrimTrailManager
    {
        public List<PrimTrail> _trails = new List<PrimTrail>();
        public void DrawTrails(SpriteBatch spriteBatch)
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                trail.Draw(spriteBatch);
            }
        }
        public void UpdateTrails()
        {
            foreach (PrimTrail trail in _trails.ToArray())
            {
                trail.Update();
            }
        }
        public void CreateTrail(PrimTrail PT) => _trails.Add(PT);
        
    }
    public partial class PrimTrail : IUpdateable
    {
        protected Projectile _projectile;
        protected NPC _npc;
        protected float _width;
        protected float _alphaValue;
        protected int _cap;

        protected int _counter;
        protected int _noOfPoints;
        protected List<Vector2> _points = new List<Vector2>();

        protected GraphicsDevice _device;
        protected Effect _effect;
        protected BasicEffect _basicEffect;
        public PrimTrail(Projectile projectile)
        {
            _device = Main.graphics.GraphicsDevice;
            _basicEffect = new BasicEffect(_device);
            _projectile = projectile;
            SetDefaults();
        }

        public void Dispose()
        {
            EEMod.primitives._trails.Remove(this);
        }
        public void Update()
        {
            OnUpdate();
        }
        public virtual void OnUpdate()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        public virtual void SetDefaults()
        {

        }

        public virtual void OnDestroy()
        {

        }
        //Helper methods
    }
}
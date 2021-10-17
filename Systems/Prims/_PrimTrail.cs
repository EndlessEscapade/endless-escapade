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
using EEMod.Items.Weapons.Mage;
using static Terraria.ModLoader.ModContent;
using System.Reflection;
using EEMod.NPCs.CoralReefs;

namespace EEMod.Prim
{
    public partial class PrimTrail : IUpdateable
    {
        protected Projectile _projectile;
        protected NPC _npc;
        protected float _width;
        protected float _alphaValue;
        protected int _cap;
        protected ITrailShader _trailShader;
        protected int _counter;
        protected int _noOfPoints;
        protected List<Vector2> _points = new List<Vector2>();
        protected bool _destroyed = false;
        public bool behindTiles = false;
        protected GraphicsDevice _device;
        protected Effect _effect;
        protected BasicEffect _basicEffect;
        protected int RENDERDISTANCE => 2000;
        protected VertexPositionColorTexture[] vertices;
        protected int currentIndex;
        public bool ManualDraw;

        public PrimTrail(Projectile projectile)
        {
            _trailShader = new DefaultShader();
            _device = Main.graphics.GraphicsDevice;
            _basicEffect = new BasicEffect(_device);
            _basicEffect.VertexColorEnabled = true;
            _projectile = projectile;

            SetDefaults();

            vertices = new VertexPositionColorTexture[_cap];
        }

        public void Dispose()
        {
            PrimSystem.primitives._trails.Remove(this);
        }

        public void Update()
        {
            OnUpdate();
        }

        public virtual void OnUpdate()
        {

        }

        public void Draw()
        {
            vertices = new VertexPositionColorTexture[_noOfPoints];

            currentIndex = 0;

            PrimStructure(Main.spriteBatch);

            SetShaders(); //applying all shaders

            if (_noOfPoints >= 1)
                _device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, _noOfPoints / 3);

            PostDraw();
        }

        public virtual void PrimStructure(SpriteBatch spriteBatch)
        {

        }

        public virtual void SetShaders()
        {

        }

        public virtual void SetDefaults()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void PostDraw()
        {

        }
    }
}
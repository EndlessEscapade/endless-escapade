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
    public class PrimTrailHelper
    {
        public static List<PrimTrail> _trails = new List<PrimTrail>();
        public void DrawTrails(SpriteBatch spriteBatch)
        {
            foreach (PrimTrail trail in _trails)
            {
                trail.Draw(spriteBatch);
            }
        }
        public void UpdateTrails()
        {
            foreach (PrimTrail trail in _trails)
            {
                trail.Update();
            }
        }
    }
    public class PrimTrail : IUpdateable
    {
        protected Projectile _projectile;
        protected NPC _npc;
        protected float _width;
        protected float _alphaValue;
        protected int _cap;

        protected int _lerper;
        protected int _noOfPoints;
        protected List<Vector2> _listOfPoints = new List<Vector2>();

        protected GraphicsDevice _device;
        protected Effect _effect;
        protected BasicEffect _basicEffect;
        public PrimTrail()
        {
            _device = Main.graphics.GraphicsDevice;
            _basicEffect = new BasicEffect(_device);
            PrimTrailHelper._trails.Add(this);
            SetDefaults();
        }
        
        public virtual void Update()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
           
        }
        public virtual void SetDefaults()
        {

        }

        public virtual void Dispose()
        {

        }
        //Helper methods
        protected static Vector2 CurveNormal(List<Vector2> points, int index)
        {
            if (points.Count == 1) return points[0];

            if (index == 0)
            {
                return Clockwise90(Vector2.Normalize(points[1] - points[0]));
            }
            if (index == points.Count - 1)
            {
                return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
            }
            return Clockwise90(Vector2.Normalize(points[index + 1] - points[index - 1]));
        }

        protected static Vector2 Clockwise90(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }
    }
}
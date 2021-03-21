using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;

namespace EEMod.Prim
{
    class WaterPrimitive : PrimTrail
    {
        public WaterPrimitive(Projectile projectile) : base(projectile) //Constructor method
        {
            _projectile = null; //Getting the projectile object the trail is bound to
        }

        private Color _color;
        public override void SetDefaults()
        {
            ManualDraw = true;
            _color = new Color(26,147,217); //Setting the color of the prims
            _alphaValue = 0.2f; //Setting the alpha of the prims
            _width = 100; //The width of the prims(aka the amount the normalized vector is multiplied by)
            _cap = 10; //Max amount of points
            _points.Add(Main.LocalPlayer.Center);
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1) return; //If there aren't enough prims, don't run the code

            Vector2 p = Main.LocalPlayer.Center;


            Vector2 firstUp = p + new Vector2(-2000, -2000);
            Vector2 firstDown = p + new Vector2(-2000, 2000);

            Vector2 secondUp = p + new Vector2(2000, -2000);
            Vector2 secondDown = p + new Vector2(2000, 2000);


            AddVertex(firstUp, _color * _alphaValue, new Vector2(0,1));
            AddVertex(secondDown, _color * _alphaValue, new Vector2(0,0));
            AddVertex(firstDown, _color * _alphaValue, new Vector2(1,1));

            AddVertex(firstUp, _color * _alphaValue, new Vector2(0, 1));
            AddVertex(secondUp, _color * _alphaValue, new Vector2(0, 0));
            AddVertex(secondDown, _color * _alphaValue, new Vector2(1, 1));
        }

        public override void SetShaders()
        {
            //PrepareShader(EEMod.TrailPractice, "AlphaFadeOff"); //Applying shaders to the prims(none here?)
            PrepareBasicShader();
        }

        public override void OnUpdate()
        {
            _counter++; //Incrementing the counter
            _noOfPoints = _points.Count() * 6; //Getting the total number of vectors
        }

        public override void OnDestroy()
        {
            _destroyed = true; //Set destroyed to true
            _points.RemoveAt(0); //Remove the first point in the trail
            if (_points.Count() <= 1) //If there are 1 or less points...
            {
                Dispose(); //Dispose of the primitives
            }
        }
    }
}
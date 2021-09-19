using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;

namespace EEMod.Prim
{
    class MemePrimTrail : PrimTrail
    {
        public MemePrimTrail(Projectile projectile) : base(projectile) //Constructor method
        {
            _projectile = projectile; //Getting the projectile object the trail is bound to
        }

        private Color _color;
        public override void SetDefaults()
        {
            _color = new Color(100, 255, 0); //Setting the color of the prims
            _alphaValue = 0.5f; //Setting the alpha of the prims
            _width = 0; //The width of the prims(aka the amount the normalized vector is multiplied by)
            _cap = 10; //Max amount of points
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_noOfPoints <= 1) return; //If there aren't enough prims, don't run the code

            float widthVar; //Local width var
            float colorSin = (float)Math.Sin(_counter / 3f); //Just for visuals

            widthVar = _width;

            //if (i == 0) //If it's the first point in the list
            {
                widthVar = _width; //Getting the width for the local width var
                Vector2 normalAhead = CurveNormal(_points, 1); //CurveNormal normalizes the vector and rotates it 90 degrees
                Vector2 secondUp = _points[1] - normalAhead * widthVar; //Gets the vector of the point ahead of the first point(so the second point), subtracts it from the current vector, and multiplies it by the width
                Vector2 secondDown = _points[1] + normalAhead * widthVar; //Gets the vector of the point ahead of the first point(so the second point), adds the current vector, and multiplies it by the width

                //Creates a triangle between the first point and the vectors above and below the next point
                Vector2 v = new Vector2((float)Math.Sin(_counter / 20f));
                AddVertex(_points[0], _color * _alphaValue, v); //Adds the vector of the first point
                AddVertex(secondUp, _color * _alphaValue, v); //Adds the vector secondUp, which is the point above the current vector
                AddVertex(secondDown, _color * _alphaValue, v); //Adds the vector secondDown, which is the point below the current vector
            }
            for (int i = 1; i < _points.Count - 1; i++)
            {
                //If it's not the first point in the list

                //If it isn't the last point in the list

                Vector2 normal = CurveNormal(_points, i); //Getting the CurveNormal(see above) of the current point
                Vector2 normalAhead = CurveNormal(_points, i + 1); //Getting the CurveNormal(see above) of the next point in the trail


                Vector2 firstUp = _points[i] - normal * widthVar; //Gets the vector above the current point
                Vector2 firstDown = _points[i] + normal * widthVar; //Gets the vector below the current point

                Vector2 secondUp = _points[i + 1] - normalAhead * widthVar; //Gets the vector above the next point
                Vector2 secondDown = _points[i + 1] + normalAhead * widthVar; //Gets the vector below the next point


                //Creates a triangle between the vector above and below the current point and the vector below the next point
                AddVertex(firstDown, _color * _alphaValue, new Vector2((i / (float)_cap), 1));
                AddVertex(firstUp, _color * _alphaValue, new Vector2((i / (float)_cap), 0));
                AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / (float)_cap, 1));

                //Creates a triangle between the vector above and below the next point and the vector above the next current point
                AddVertex(secondUp, _color * _alphaValue, new Vector2((i + 1) / (float)_cap, 0));
                AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / (float)_cap, 1));
                AddVertex(firstUp, _color * _alphaValue, new Vector2((i / (float)_cap), 0));

                widthVar++;
            }
        }

        public override void SetShaders()
        {
            PrepareShader(EEMod.TrailPractice, "AlphaFadeOff"); //Applying shaders to the prims(none here?)
        }

        public override void OnUpdate()
        {
            _counter++; //Incrementing the counter
            _noOfPoints = _points.Count() * 6; //Getting the total number of vectors

            if (_cap < _noOfPoints / 6) //If there are more points than the cap...
            {
                _points.RemoveAt(0); //Remove the excess points(delete them)
            }
            if ((!_projectile.active && _projectile != null) || _destroyed) //If the projectile these prims are bound to is dead or null, or if this prim is destroyed...
            {
                OnDestroy(); //Call the OnDestroy method
            }
            else //If the prims are active and the projectile is still active...
            {
                _points.Add(_projectile.Center); //Add a new point at the center of the projectile
            }
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
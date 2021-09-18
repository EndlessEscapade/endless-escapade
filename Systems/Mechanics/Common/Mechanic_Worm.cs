using EEMod.Extensions;
using EEMod.Prim;
using EEMod.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{


    public class Worm : Mechanic
    {
        ExampleWorm EEWorm;
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            //EEWorm.Draw(Main.spriteBatch);
        }
        public override void OnUpdate()
        {
            //EEWorm.Update();
        }
        public override void OnLoad()
        {
            EEWorm = new ExampleWorm();
        }
        protected override Layer DrawLayering => base.DrawLayering;
    }

    public class ExampleWorm : SegmentedWorm
    {
        public Color color;
        float lerp;
        public override void OnDraw()
        {
           // Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, ControlSegment.position.ForDraw(), new Rectangle(0, 0, 8, 8), color);
        }

        public override void SegmentDraw(SpriteBatch spriteBatch, Segment segment)
        {
           // if(segment.index % 2 == 0)
           // Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, segment.position.ForDraw(), new Rectangle(0, 0, 5, 5), color);
           // else
           // Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, segment.position.ForDraw(), new Rectangle(0, 0, 5, 5), color);
        }
        public override void OnLoad()
        {
        }
        Vector2 Limit(Vector2 vec, float val)
        {
            if (vec.LengthSquared() > val * val)
                return Vector2.Normalize(vec) * val;
            return vec;
        }
        public Vector2 AvoidTiles(int range) //WIP for Qwerty
        {
            Vector2 sum = new Vector2(0, 0);
            Point tilePos = ControlSegment.position.ToTileCoordinates();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (WorldGen.InWorld(tilePos.X + i, tilePos.Y + j, 10))
                    {
                        Tile tile = Framing.GetTileSafely(tilePos.X + i, tilePos.Y + j);
                        float pdist = Vector2.DistanceSquared(ControlSegment.position, new Vector2(tilePos.X + i, tilePos.Y + j) * 16);
                        if (pdist < range * range && pdist > 0 && tile.IsActive && Main.tileSolid[tile.type])
                        {
                            Vector2 d = ControlSegment.position - new Vector2(tilePos.X + i, tilePos.Y + j) * 16;
                            Vector2 norm = Vector2.Normalize(d);
                            Vector2 weight = norm;
                            sum += weight;
                        }
                    }
                }
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * 5f;
                Vector2 acc = sum - ControlSegment.velocity;
                RoamVelocity = sum.RotatedBy(3.14f);
                AgressionMeter--;
                return Limit(acc, 0.4f);
            }
            return Vector2.Zero;
        }
        int AgressionMeter;
        bool CanSee => Collision.CanHitLine(Main.LocalPlayer.Center,2,2, ControlSegment.position,2,2);

        Vector2 RoamVelocity;
        float VecLerp = 4;
        public void Passive()
        {
            if (Main.GameUpdateCount % 160 == 0)
            {
                RoamVelocity = new Vector2(3, 0).RotatedBy(Main.rand.NextFloat(6.1f));
            }
            VecLerp += (4 - VecLerp) / 4f;
            ControlSegment.velocity += Limit(ControlSegment.velocity - RoamVelocity, 0.05f);
            ControlSegment.velocity += AvoidTiles(50) * 3;
            ControlSegment.velocity = Limit(ControlSegment.velocity, VecLerp);
            lerp -= lerp / 16f;
        }

        Point[] Pathing;
        int PathingInterpolation;
        public void Attack()
        {
            if(Main.GameUpdateCount % 30 == 0)
            {
                Pathing = Pathfinding.FindShortestPath(ControlSegment.position.ToTileCoordinates(),Main.LocalPlayer.Center.ToTileCoordinates(),1000);
                PathingInterpolation = 0;
            }

            if(Pathing != null && Pathing.Length > 3 && Main.GameUpdateCount % 3 == 0)
            {
                if (PathingInterpolation < Pathing.Length - 1)
                {
                    PathingInterpolation++;
                }
                Vector2 PathingVector = Pathing[Pathing.Length - PathingInterpolation - 1].ToVector2()*16 - ControlSegment.position;
                Vector2 PathingSHM = PathingVector / 300f - ControlSegment.velocity / 8f;

                ControlSegment.velocity += Limit(PathingSHM, 8)*4;
            }
            Vector2 Dist = Main.LocalPlayer.Center - ControlSegment.position;


            VecLerp += (8 - VecLerp) / 4f;

           // ControlSegment.velocity += Limit(Norm, 1);
            ControlSegment.velocity += AvoidTiles(50) * 2 * ControlSegment.velocity.Length();
            ControlSegment.velocity = Limit(ControlSegment.velocity*1.4f, Math.Max(VecLerp, Dist.Length()/50f));

            float Sin = (float)Math.Sin(Main.GameUpdateCount / 6f);
            float Sin2 = (float)Math.Sin(Main.GameUpdateCount / 7f);

            ControlSegment.position += new Vector2(0, Sin)*0.5f;
            ControlSegment.position += new Vector2(0, Sin2);
            lerp += (1 - lerp) / 16f;
        }
        public override void OnAdditiveDraw()
        {
            for (int i = 0; i < l; i++)
            {
                Main.spriteBatch.Draw(Helpers.RadialMask, Segments[i].position.ForDraw(), Helpers.RadialMask.Bounds, color*0.3f,0f,Helpers.RadialMask.TextureCenter(),0.4f,SpriteEffects.None,0f);
            }
        }
        public override void PostAI()
        {
            Vector2 Dist = Main.LocalPlayer.Center - ControlSegment.position;

            if (CanSee && Dist.LengthSquared() < 1000 * 1000)
            {
                if(AgressionMeter < 400)
                AgressionMeter++;
            }
            else
            {
                if(AgressionMeter > 30)
                    lerp = (float)Math.Sin(Main.GameUpdateCount/3f);
                if (AgressionMeter > 0)
                    AgressionMeter--;
            }
            if (Main.GameUpdateCount <= 30)
            {
                if(Main.GameUpdateCount == 2)
                EEMod.primitives.CreateTrail(new WormMesh(null, this));
                ControlSegment.position = Main.LocalPlayer.Center;
                for (int i = 0; i < l; i++)
                {
                    Segments[i].position = ControlSegment.position + new Vector2(-i * l - l, 0);
                    Segments[i].velocity = Vector2.Zero;
                }
            }
            if (AgressionMeter > 50)
            {
                Attack();
            }
            else if(AgressionMeter > 30)
            {
                Passive();
            }
            else
            {
                Passive();
            }
            if (Main.LocalPlayer.controlUseItem)
            {
                ControlSegment.position = Main.MouseWorld;
                for (int i = 0; i < l; i++)
                {
                    Segments[i].position = ControlSegment.position + new Vector2(-i * l - l, 0);
                    Segments[i].velocity = Vector2.Zero;
                }
            }
            color = Color.Lerp(Color.ForestGreen, Color.Red, lerp);

            float Sin = (float)Math.Sin(Main.GameUpdateCount / 10f);

            ControlSegment.position += new Vector2(0, Sin);
        }
    }
    public class SegmentedWorm : IComponent, IDrawAdditive
    {
        public Segment[] Segments;

        public Segment ControlSegment;
        public float l => 20;
        public float StretchFactor => 0.01f;

        public SegmentedWorm()
        {
            Segments = new Segment[(int)l];
            AdditiveCalls.Instance.LoadObject(this);
            OnLoad();
        }

        public virtual void SegmentDraw(SpriteBatch spriteBatch, Segment segment) { }
        public virtual void OnLoad() { }
        public virtual void PreAI() { }
        public virtual void PostAI() { }
        public virtual void OnDraw() { }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            OnDraw();

            for (int i = 0; i < Segments.Length; i++)
            {
                SegmentDraw(spriteBatch, Segments[i]);
            }
        }
        public void Update()
        {
            PreAI();
            UpdatePoints();
            ApplyForces();
            PostAI();
        }
        /// <summary>
        /// Applies velocity to all worm objects
        /// </summary>
        public void ApplyForces()
        {
            for (int i = 0; i < Segments.Length; i++)
            {
                Segments[i].position += Segments[i].velocity;
            }

            ControlSegment.position += ControlSegment.velocity;
        }
        /// <summary>
        /// Implements a generic paramaterizable Worm AI
        /// </summary>
        public void UpdatePoints()
        {
            for (int i = 0; i < Segments.Length; i++)
            {
                if (i == 0)
                {
                    Vector2 diff = ControlSegment.position - Segments[i].position;
                    Segments[i].velocity = diff * (StretchFactor * (diff.Length() - l/10));
                }
                else
                { 
                    Vector2 diff = Segments[i - 1].position - Segments[i].position;
                    Segments[i].velocity = diff * (StretchFactor * (diff.Length() - l / 10));
                }
                Segments[i].index = i;
            }
        }
        public virtual void OnAdditiveDraw()
        {

        }
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            OnAdditiveDraw();
        }
    }

    public struct Segment
    {
        internal Vector2 position;

        internal Vector2 velocity;

        public int index;
    }

}
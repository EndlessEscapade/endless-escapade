using EEMod.Extensions;
using EEMod.ID;
using EEMod.VerletIntegration;
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
    public class Boids : Mechanic
    {
        internal List<Flock> fishflocks = new List<Flock>();

        public override void OnDraw()
        {
            /*foreach(Flock fishflock in fishflocks)
            {
                fishflock.Draw(Main.spriteBatch);
            }*/
        }

        public override void OnUpdate()
        {
            /*foreach (Flock fishflock in fishflocks)
            {
                fishflock.Update();
            }

            if (ElapsedTicks % 200 == 0 && Main.worldName == KeyID.CoralReefs)
            {
                int randInt = Main.rand.Next(0, fishflocks.Count);

                fishflocks[randInt].Populate(Main.LocalPlayer.Center, 40, 50f);
            }*/
        }

        public override void OnLoad()
        {
            //fishflocks.Add(new Flock("Particles/Fish"));
            //fishflocks.Add(new Flock("Particles/BetaFish"));
            //fishflocks.Add(new Flock("Particles/Clownfish"));
            //fishflocks.Add(new Flock("Particles/StripedBass"));
        }

        protected override Layer DrawLayering => Layer.BehindTiles;
    }

    internal class Fish : Entity,IComponent
    {
        public Vector2 acceleration { get; set; }

        public const float Vision = 100;

        private const float MaxForce = 0.03f;

        private const float MaxVelocity = 2f;

        private Flock parent;

        public List<Fish> AdjFish = new List<Fish>();

        public Fish(Flock osSucksAtBedwars)
        {
            parent = osSucksAtBedwars;
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
            Point tilePos = position.ToTileCoordinates();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (WorldGen.InWorld(tilePos.X + i, tilePos.Y + j,10))
                    {
                        Tile tile = Framing.GetTileSafely(tilePos.X + i, tilePos.Y + j);
                        float pdist = Vector2.DistanceSquared(position, new Vector2(tilePos.X + i, tilePos.Y + j) * 16);
                        if (pdist < range * range && pdist > 0 && tile.active() && Main.tileSolid[tile.type])
                        {
                            Vector2 d = position - new Vector2(tilePos.X + i, tilePos.Y + j) * 16;
                            Vector2 norm = Vector2.Normalize(d);
                            Vector2 weight = norm;
                            sum += weight;
                        }
                    }
                }
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public Vector2 AvoidHooman(int range)
        {
            float pdist = Vector2.DistanceSquared(position, Main.LocalPlayer.Center);
            Vector2 sum = new Vector2(0, 0);
            if (pdist < range * range && pdist > 0)
            {
                Vector2 d = position - Main.LocalPlayer.Center;
                Vector2 norm = Vector2.Normalize(d);
                Vector2 weight = norm;
                sum += weight;
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public Vector2 Seperation(int range)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            for (int j = 0; j < AdjFish.Count; j++)
            {
                var OtherFish = AdjFish[j];
                float dist = Vector2.DistanceSquared(position, OtherFish.position);
                if (dist < range * range && dist > 0)
                {
                    Vector2 d = position - OtherFish.position;
                    Vector2 norm = Vector2.Normalize(d);
                    Vector2 weight = norm / dist;
                    sum += weight;
                    count++;
                }
            }
            if (count > 0)
            {
                sum /= count;
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public Vector2 Allignment(int range)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            for (int j = 0; j < AdjFish.Count; j++)
            {
                var OtherFish = AdjFish[j];
                float dist = Vector2.DistanceSquared(position, OtherFish.position);
                if (dist < range* range && dist > 0)
                {
                    sum += OtherFish.velocity;
                    count++;
                }
            }
            if (count > 0)
            {
                sum /= count;
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public Vector2 Cohesion(int range)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            for (int j = 0; j < AdjFish.Count; j++)
            {
                var OtherFish = AdjFish[j];
                float dist = Vector2.DistanceSquared(position, OtherFish.position);
                if (dist < range * range && dist > 0)
                {
                    sum += OtherFish.position;
                    count++;
                }
            }

            if (count > 0)
            {
                sum /= count;
                sum -= position;
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Point point = position.ParalaxX(-0f).ToTileCoordinates();
            //Color lightColour = Lighting.GetColor(point.X, point.Y);
            Texture2D texture = ModContent.GetInstance<EEMod>().GetTexture(parent.flockTex);
            spritebatch.Draw(texture, position.ForDraw().ParalaxX(-0f), texture.Bounds, Color.White, velocity.ToRotation(), Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 0f);
        }

        public void ApplyForces()
        {
            velocity += acceleration;
            velocity = Limit(velocity, MaxVelocity);
            position += velocity;
            acceleration *= 0;
        }

        public void Update()
        {
            //arbitrarily weight
            acceleration += Seperation(25) * 1.5f;
            acceleration += Allignment(50) * 1f;
            acceleration += Cohesion(50) * 1f;
            acceleration += AvoidHooman(50) * 4f;
            acceleration += AvoidTiles(50) * 2.5f;
            ApplyForces();
        }
    }

    internal class Flock : ComponentManager<Fish>
    {
        public string flockTex;

        public Flock(string tex)
        {
            tex = flockTex;
        }

        internal void Populate(Vector2 position, int amount, float spread)
        {
            for(int i = 0; i<amount; i++)
            {
                if (Objects.Count < 250)
                {
                    Fish fish = new Fish(this)
                    {
                        position = position.ParalaxX(0f) + new Vector2(Main.rand.NextFloat(-spread, spread), Main.rand.NextFloat(-spread, spread)),
                        velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1))
                    };
                    Objects.Add(fish);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach(Fish fish in Objects.ToArray())
            {
                if (fish != null)
                {
                    fish.AdjFish.Clear();
                    foreach (Fish adjfish in Objects)
                    {
                        if (!fish.Equals(adjfish))
                        {
                            if (Vector2.DistanceSquared(fish.position, adjfish.position) < Fish.Vision * Fish.Vision)
                            {
                                fish.AdjFish.Add(adjfish);
                            }
                        }
                    }
                    if (Vector2.DistanceSquared(fish.position.ParalaxX(-0f), Main.LocalPlayer.Center) > 2000 * 2000)
                        Objects.Remove(fish);
                }
            }
        }
    }
}
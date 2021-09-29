using EEMod.Extensions;
using EEMod.ID;
using EEMod.Systems;
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
    public class Boids : ModSystem
    {
        internal List<Flock> fishflocks = new List<Flock>();

        public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin();

            foreach(Flock fishflock in fishflocks)
            {
                fishflock.Draw(Main.spriteBatch);
            }

            Main.spriteBatch.End();
        }

        public override void PostUpdateEverything()
        {
            foreach (Flock fishflock in fishflocks)
            {
                fishflock.Update();
            }

            if (Main.GameUpdateCount % 150 == 0 && Main.worldName == KeyID.CoralReefs)
            {
                Vector2 rand = new Vector2(Main.rand.Next(-(int)(Main.screenWidth / 1.5f), (int)(Main.screenWidth / 1.5f)), Main.rand.Next(-(int)(Main.screenHeight / 1.5f), (int)(Main.screenHeight / 1.5f)));

                while (new Rectangle(-Main.screenWidth / 2, -Main.screenHeight / 2, Main.screenWidth, Main.screenHeight).Contains(rand.ToPoint()) && WorldGen.InWorld((int)(rand.X / 16f), (int)(rand.Y / 16f)))
                {
                    rand = new Vector2(Main.rand.Next(-(int)(Main.screenWidth / 1.5f), (int)(Main.screenWidth / 1.5f)), Main.rand.Next(-(int)(Main.screenHeight / 1.5f), (int)(Main.screenHeight / 1.5f)));
                }

                if (!new Rectangle(-Main.screenWidth / 2, -Main.screenHeight / 2, Main.screenWidth, Main.screenHeight).Contains(rand.ToPoint()))
                {
                    if (Main.LocalPlayer.GetModPlayer<EEPlayer>().reefMinibiome == MinibiomeID.KelpForest)
                    {
                        int randInt = Main.rand.Next(5, 8);

                        Vector2 vec = (Main.LocalPlayer.Center + rand) / 16;

                        if (Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidType == 0 && Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidAmount >= 100)
                            fishflocks[randInt].Populate(Main.LocalPlayer.Center + rand, Main.rand.Next(fishflocks[randInt].randMin, fishflocks[randInt].randMax), 50f);
                    }
                    else if (Main.LocalPlayer.GetModPlayer<EEPlayer>().reefMinibiome == MinibiomeID.AquamarineCaverns)
                    {
                        int randInt = Main.rand.Next(8, 9);

                        Vector2 vec = (Main.LocalPlayer.Center + rand) / 16;

                        if (Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidType == 0 && Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidAmount >= 100)
                            fishflocks[randInt].Populate(Main.LocalPlayer.Center + rand, Main.rand.Next(fishflocks[randInt].randMin, fishflocks[randInt].randMax), 50f);
                    }
                    else if (Main.LocalPlayer.GetModPlayer<EEPlayer>().reefMinibiome == MinibiomeID.ThermalVents)
                    {
                        int randInt = Main.rand.Next(9, 10);

                        Vector2 vec = (Main.LocalPlayer.Center + rand) / 16;

                        if (Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidType == 0 && Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidAmount >= 100)
                            fishflocks[randInt].Populate(Main.LocalPlayer.Center + rand, Main.rand.Next(fishflocks[randInt].randMin, fishflocks[randInt].randMax), 50f);
                    }
                    else
                    {
                        int randInt = Main.rand.Next(0, 5);

                        Vector2 vec = (Main.LocalPlayer.Center + rand) / 16;

                        if(WorldGen.InWorld((int)vec.X, (int)vec.Y))
                        if (Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidType == 0 && Main.tile[(int)(vec.X), (int)(vec.Y)].LiquidAmount >= 100)
                            fishflocks[randInt].Populate(Main.LocalPlayer.Center + rand, Main.rand.Next(fishflocks[randInt].randMin, fishflocks[randInt].randMax), 50f);
                    }
                }
            }
        }

        public override void Load()
        {
            fishflocks.Add(new Flock("Particles/Fish", 1f, 10, 20));
            fishflocks.Add(new Flock("Particles/Coralfin", 1f, 5, 15));
            fishflocks.Add(new Flock("Particles/Barracuda", 1f, 3, 7));
            fishflocks.Add(new Flock("Particles/LargeBubblefish", 1f, 5, 10));
            fishflocks.Add(new Flock("Particles/SmallBubblefish", 1f, 15, 25));

            fishflocks.Add(new Flock("Particles/KelpEel2", 1f, 10, 20));
            fishflocks.Add(new Flock("Particles/Kelpfin", 1f, 3, 7));
            fishflocks.Add(new Flock("Particles/GoldenFish", 1f, 5, 10));

            fishflocks.Add(new Flock("Particles/AquamarineFish1", 1f, 3, 7));

            fishflocks.Add(new Flock("Particles/Thermalfin", 1f, 25, 35));
        }
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
                        if (pdist < range * range && pdist > 0 && tile.IsActive && Main.tileSolid[tile.type] || tile.LiquidAmount < 100)
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
            Color lightColour = Lighting.GetColor(point.X, point.Y);
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>(parent.flockTex).Value;
            spritebatch.Draw(texture, position.ForDraw().ParalaxX(-0f), texture.Bounds, lightColour, velocity.ToRotation(), texture.Bounds.Size() / 2f, parent.fishScale, SpriteEffects.None, 0f);
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
        public float fishScale;
        public int randMin;
        public int randMax;

        public Flock(string tex, float scale, int randmin, int randmax)
        {
            flockTex = tex;
            fishScale = scale;
            randMin = randmin;
            randMax = randmax;
        }

        internal void Populate(Vector2 position, int amount, float spread)
        {
            for(int i = 0; i<amount; i++)
            {
                if (Objects.Count < 60)
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
                    if (Vector2.DistanceSquared(fish.position.ParalaxX(-0f), Main.LocalPlayer.Center) > 2200 * 2200)
                        Objects.Remove(fish);
                }
            }
        }
    }
}

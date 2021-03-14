using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace EEMod
{
    public class ParticleZone
    {
        public ParticleZone(int MaxLength)
        {
            particles = new Particle[MaxLength];
        }
        internal readonly Particle[] particles;
        private List<IParticleModule> BaseZoneModules = new List<IParticleModule>();
        private readonly List<IParticleSpawner> SpawningModules = new List<IParticleSpawner>();
        public bool CanSpawn { get; set; }
        public int zoneTimer;
        int MAXDRAWDISTANCE => 2000;
        public int SpawnParticles(Vector2 position, Vector2 velocity = default, Texture2D texture = null, int timeLeft = 60, float scale = 1, Color? colour = null, Texture2D masks = null, float paralax = 0, params IParticleModule[] CustomBaseZoneModule)
        {
            if (!Main.gamePaused)
            {
                if ((position - Main.LocalPlayer.Center).LengthSquared() < MAXDRAWDISTANCE * MAXDRAWDISTANCE)
                {
                    CanSpawn = false;
                    foreach (IParticleSpawner Module in SpawningModules)
                    {
                        Module.CanSpawn(this);
                    }

                    if (!CanSpawn) return -1;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        if (particles[i] != null)
                        {
                            if (!particles[i].active)
                            {
                                particles[i] = new Particle(position + new Vector2(Main.LocalPlayer.Center.X * paralax, Main.LocalPlayer.Center.Y * paralax), timeLeft, texture ?? Main.magicPixel, velocity, scale, colour ?? Color.White, masks, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                                if (texture != null)
                                {
                                    particles[i].Frame = texture.Bounds;
                                }
                                particles[i].paralax = paralax;
                                return i;
                            }
                        }
                        else
                        {
                            particles[i] = new Particle(position + new Vector2(Main.LocalPlayer.Center.X * paralax, Main.LocalPlayer.Center.Y * paralax), timeLeft, texture ?? Main.magicPixel, velocity, scale, colour ?? Color.White, masks, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                            if (texture != null)
                            {
                                particles[i].Frame = texture.Bounds;
                            }
                            particles[i].paralax = paralax;
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        public int SpawnParticles(Vector2 position, Vector2 velocity = default, Texture2D texture = null, int timeLeft = 60, float scale = 1, Color? colour = null, params IParticleModule[] CustomBaseZoneModule)
        {
            if (!Main.gamePaused)
            {
                if ((position - Main.LocalPlayer.Center).LengthSquared() < MAXDRAWDISTANCE * MAXDRAWDISTANCE)
                {
                    CanSpawn = false;
                    foreach (IParticleSpawner Module in SpawningModules)
                    {
                        Module.CanSpawn(this);
                    }

                    if (!CanSpawn) return -1;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        if (particles[i] != null)
                        {
                            if (!particles[i].active)
                            {
                                particles[i] = new Particle(position, timeLeft, texture ?? Main.magicPixel, velocity, scale, colour ?? Color.White, null, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                                if (texture != null)
                                {
                                    particles[i].Frame = texture.Bounds;
                                }
                                return i;
                            }
                        }
                        else
                        {
                            particles[i] = new Particle(position, timeLeft, texture ?? Main.magicPixel, velocity, scale, colour ?? Color.White, null, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                            if (texture != null)
                            {
                                particles[i].Frame = texture.Bounds;
                            }
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, Color col, Texture2D mask, params IParticleModule[] modules) =>
            SpawnParticles(player.Center + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, 3, col, mask, 0.6f, modules);
        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, Color col, params IParticleModule[] modules) =>
            SpawnParticles(player.Center + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, 2, col, null, 0.6f, modules);
        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, params IParticleModule[] modules) =>
          SpawnParticles(player.Center + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, 1, null, null, 0.6f, modules);
        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex,float scale, float paralaxFactor, params IParticleModule[] modules) =>
          SpawnParticles(player.Center + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, scale, null, null, paralaxFactor, modules);

        public int SpawnParticles(Vector2 position, Vector2 velocity = default, params IParticleModule[] CustomBaseZoneModule)
        {
            if (!Main.gamePaused)
            {
                if ((position - Main.LocalPlayer.Center).LengthSquared() < MAXDRAWDISTANCE * MAXDRAWDISTANCE)
                {
                    CanSpawn = false;
                    foreach (IParticleSpawner Module in SpawningModules)
                    {
                        Module.CanSpawn(this);
                    }

                    if (!CanSpawn) return -1;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        if (particles[i] != null)
                        {
                            if (!particles[i].active)
                            {
                                particles[i] = new Particle(position, 60, Main.magicPixel, Vector2.Zero, 1, Color.White, null, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                                return i;
                            }
                        }
                        else
                        {
                            particles[i] = new Particle(position, 60, Main.magicPixel, velocity, 1, Color.White, null, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        public int SpawnParticles(Vector2 position, Vector2 velocity = default, int scale = 1, Color? colour = null, params IParticleModule[] CustomBaseZoneModule)
        {
            if (!Main.gamePaused)
            {
                if ((position - Main.LocalPlayer.Center).LengthSquared() < MAXDRAWDISTANCE * MAXDRAWDISTANCE)
                {
                    CanSpawn = false;
                    foreach (IParticleSpawner Module in SpawningModules)
                    {
                        Module.CanSpawn(this);
                    }

                    if (!CanSpawn) return -1;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        if (particles[i] != null)
                        {
                            if (!particles[i].active)
                            {
                                particles[i] = new Particle(position, 60, Main.magicPixel, velocity, scale, colour ?? Color.White, null, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                                return i;
                            }
                        }
                        else
                        {
                            particles[i] = new Particle(position, 60, Main.magicPixel, velocity, scale, colour ?? Color.White, null, CustomBaseZoneModule.ToArray().Length == 0 ? BaseZoneModules.ToArray() : CustomBaseZoneModule);
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        public int SpawnParticles(Vector2 position, Vector2 velocity = default, Color? colour = null, params IParticleModule[] CustomBaseZoneModule)
        {
            if (!Main.gamePaused)
            {
                if ((position - Main.LocalPlayer.Center).LengthSquared() < MAXDRAWDISTANCE * MAXDRAWDISTANCE)
                {
                    CanSpawn = false;
                    foreach (IParticleSpawner Module in SpawningModules)
                    {
                        Module.CanSpawn(this);
                    }

                    if (!CanSpawn) return -1;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        if (particles[i] != null)
                        {
                            if (!particles[i].active)
                            {
                                particles[i] = new Particle(position, 60, Main.magicPixel, velocity, 1, colour ?? Color.White, null, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                                return i;
                            }
                        }
                        else
                        {
                            particles[i] = new Particle(position, 60, Main.magicPixel, velocity, 1, colour ?? Color.White, null, CustomBaseZoneModule.ToArray().Length == 0 ? BaseZoneModules.ToArray() : CustomBaseZoneModule);
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        public void SetModules(params IParticleModule[] Module) { BaseZoneModules.Clear(); BaseZoneModules = Module.ToList(); }
        public void SetSpawningModules(IParticleSpawner Module) { SpawningModules.Clear(); SpawningModules.Add(Module); }
        public void Update()
        {
            for (int k = 0; k < particles.Length; k++)
            {
                if (particles[k] != null)
                {
                    if (particles[k].active)
                    {
                        particles[k]?.Update();
                    }
                }
            }

            zoneTimer++;
        }
        public void Draw()
        {
            for (int k = 0; k < particles.Length; k++)
            {
                if (particles[k] != null)
                {
                    if (particles[k].active)
                    {
                        particles[k]?.Draw();
                    }
                }
            }
        }
    }
    class SpawnPeriodically : IParticleSpawner
    {
        int spawnRate;
        bool ForTiles;
        public SpawnPeriodically(int spawnRate, bool ForTiles = false)
        {
            this.spawnRate = spawnRate;
            this.ForTiles = ForTiles;
        }

        public void CanSpawn(in ParticleZone pz)
        {
            if (!ForTiles)
            {
                if (pz.zoneTimer % spawnRate == 0)
                    pz.CanSpawn = true;
            }
            else
            {
                if ((pz.zoneTimer/5) % spawnRate == 0)
                    pz.CanSpawn = true;
            }
        }
    }
    class SpawnRandomly : IParticleSpawner
    {
        float chance;
        public SpawnRandomly(float chance)
        {
            this.chance = chance;
        }

        public void CanSpawn(in ParticleZone pz)
        {
            if (Main.rand.NextFloat(1f) < chance)
                pz.CanSpawn = true;
        }
    }
    /*class Lightning : IParticleSpawner
    {
        float sizeMin;
        float sizeMax;
        float intervalMin;
        float intervalMax;
        int frequency;
        public Lightning(float sizeMin, float sizeMax, float intervalMin, float intervalMax, int frequency)
        {
            this.sizeMin = sizeMin;
            this.sizeMax = sizeMax;
            this.intervalMin = intervalMin;
            this.intervalMax = intervalMax;
            this.frequency = frequency;
        }

        public void CanSpawn(in ParticleZone pz)
        {
            foreach(Particle particle in pz.particles)
            {
                particle.position = 
            }
        }
    }*/
    public interface IParticleSpawner
    {
        void CanSpawn(in ParticleZone pz);
    }
}
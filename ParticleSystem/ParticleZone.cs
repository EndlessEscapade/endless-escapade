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
        private readonly List<IParticleModule> BaseZoneModules = new List<IParticleModule>();
        private readonly List<IParticleSpawner> SpawningModules = new List<IParticleSpawner>();
        public bool CanSpawn { get; set; }
        public int zoneTimer;
        public int SpawnParticles(Vector2 position, Vector2? velocity = null, Texture2D texture = null, int timeLeft = 60,params IParticleModule[] CustomBaseZoneModule)
        {
            CanSpawn = false;
            foreach (IParticleSpawner Module in SpawningModules)
            {
                Module.CanSpawn(this);
            }

            if (!CanSpawn) return -1;

            for (int i = 0; i<particles.Length; i++)
            {
                if (particles[i] != null)
                {
                    if (!particles[i].active)
                    {
                        particles[i] = new Particle(position, timeLeft, texture ?? Main.magicPixel, velocity, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                        return i;
                    }
                }
                else
                {
                    particles[i] = new Particle(position, timeLeft, texture ?? Main.magicPixel, velocity, CustomBaseZoneModule.ToArray() ?? BaseZoneModules.ToArray());
                    return i;
                }
            }
            return -1;
        }

        public void AddModule(IParticleModule Module) => BaseZoneModules.Add(Module);
        public void AddSpawningModule(IParticleSpawner Module) => SpawningModules.Add(Module);
        public void Update()
        {
            for (int k = 0; k<particles.Length; k++)
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
        public SpawnPeriodically(int spawnRate)
        {
            this.spawnRate = spawnRate;
        }

        public void CanSpawn(in ParticleZone pz)
        {
            if (pz.zoneTimer % spawnRate == 0)
                pz.CanSpawn = true;
        }
    }
    class SpawnRandomly : IParticleSpawner
    {
        float chance;
        public SpawnRandomly(int chance)
        {
            this.chance = chance;
        }

        public void CanSpawn(in ParticleZone pz)
        {
            if (Main.rand.NextFloat(1f) < chance)
                pz.CanSpawn = true;
        }
    }
    public interface IParticleSpawner
    {
        void CanSpawn(in ParticleZone pz);
    }
}
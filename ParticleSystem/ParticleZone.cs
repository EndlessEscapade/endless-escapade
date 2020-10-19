using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace EEMod
{
    public class ParticleZone
    {
        public ParticleZone(int MaxLength)
        {
            particles = new Particle[MaxLength];
        }
        private readonly Particle[] particles;
        private readonly List<IParticleModule> BaseZoneModules = new List<IParticleModule>();
        public int AddParticle(Vector2 position, Vector2? velocity = null, int timeLeft = 60, List<IParticleModule> CustomBaseZoneModule = null)
        {
            for(int i = 0; i<particles.Length; i++)
            {
                if(!particles[i].active)
                {
                    particles[i] = new Particle(position, timeLeft,velocity, CustomBaseZoneModule ?? BaseZoneModules);
                    return i;
                }
            }
            return -1;
        }
        public void Update()
        {
            for (int k = 0; k<particles.Length; k++)
            {
                if (particles[k].active)
                {
                    particles[k]?.Update();
                }
            }
        }
        public void Draw()
        {
            for (int k = 0; k < particles.Length; k++)
            {
                if (particles[k].active)
                {
                    particles[k]?.Draw();
                }
            }
        }
    }
    interface SpawnModule
    {
        void Spawn();
    }
}
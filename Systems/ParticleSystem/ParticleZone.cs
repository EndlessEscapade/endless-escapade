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
        private readonly List<IParticleSpawner> SpawningModules = new List<IParticleSpawner>();
        private List<IParticleModule> BaseZoneModules = new List<IParticleModule>();
        public bool CanSpawn { get; set; }
        public int zoneTimer;
        int MAXDRAWDISTANCE => 2000;

        public int SpawnParticles(Vector2 position, Vector2 velocity = default, params IParticleModule[] CustomBaseZoneModule) =>
            _SpawnParticles(position, velocity, CustomBaseZoneModule: CustomBaseZoneModule);

        public int SpawnParticles(Vector2 position, Vector2 velocity = default, Color? colour = null, params IParticleModule[] CustomBaseZoneModule)=>
            _SpawnParticles(position, velocity, colour: colour, CustomBaseZoneModule: CustomBaseZoneModule);

        public int SpawnParticles(Vector2 position, Vector2 velocity = default, float scale = 1, Color? colour = null, params IParticleModule[] CustomBaseZoneModule)=>
            _SpawnParticles(position, velocity, scale: scale, colour: colour, CustomBaseZoneModule: CustomBaseZoneModule);

        public int SpawnParticles(Vector2 position, Vector2 velocity = default, Texture2D texture = null, int timeLeft = 60, float scale = 1, Color? colour = null, params IParticleModule[] CustomBaseZoneModule)=>
            _SpawnParticles(position, velocity, texture, timeLeft, scale, colour, CustomBaseZoneModule: CustomBaseZoneModule);
        
        public int SpawnParticles(Vector2 position, Vector2 velocity = default, Texture2D texture = null, int timeLeft = 60, float scale = 1, Color? colour = null, Texture2D masks = null, float paralax = 0, params IParticleModule[] CustomBaseZoneModule) =>
            _SpawnParticles(position, velocity, texture, timeLeft, scale, colour, masks, paralax, CustomBaseZoneModule);

        // different method so the it doesn't confuse overloads when doing paramname: param
        private int _SpawnParticles(Vector2 position, Vector2 velocity = default, Texture2D texture = null, int timeLeft = 60, float scale = 1, Color? colour = null, Texture2D masks = null, float paralax = 0, params IParticleModule[] CustomBaseZoneModule)
        {
            if (!Main.gamePaused)
            {
                if (Vector2.DistanceSquared(position, Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f)) < MAXDRAWDISTANCE * MAXDRAWDISTANCE)
                {
                    CanSpawn = false;
                    foreach (IParticleSpawner Module in SpawningModules)
                    {
                        Module.CanSpawn(this);
                    }

                    if (!CanSpawn) return -1;

                    for (int i = 0; i < particles.Length; i++)
                    {
                        Particle particle = particles[i];
                        if (particle == null || !particle.active)
                        {
                            particles[i] = particle = new Particle(position + (Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f)) * paralax, timeLeft, texture ?? Terraria.GameContent.TextureAssets.MagicPixel.Value, velocity, scale, colour ?? Color.White, masks, (CustomBaseZoneModule?.Length ?? 0) > 0 ? CustomBaseZoneModule : BaseZoneModules.ToArray());
                            if (texture != null)
                                particle.Frame = texture.Bounds;
                            particle.paralax = paralax;
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, Color col, Texture2D mask, params IParticleModule[] modules) =>
            SpawnParticles(Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, 3, col, mask, 0.6f, modules);

        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, Color col, params IParticleModule[] modules) =>
            SpawnParticles(Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, 2, col, null, 0.6f, modules);

        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, params IParticleModule[] modules) =>
            SpawnParticles(Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, 1, null, null, 0.6f, modules);

        public void SpawnParticleDownUp(Player player, Vector2 vel, Texture2D tex, float scale, float paralaxFactor, params IParticleModule[] modules) =>
            SpawnParticles(Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) + new Vector2(Main.rand.Next(-1000, 1000), 1000), vel, tex, 1000, scale, null, null, paralaxFactor, modules);

        public void SetModules(params IParticleModule[] Module) 
        { 
            BaseZoneModules.Clear(); 
            BaseZoneModules.AddRange(Module);
        }

        public void SetSpawningModules(IParticleSpawner Module) 
        { 
            SpawningModules.Clear(); 
            SpawningModules.Add(Module); 
        }

        public void Update()
        {
            for (int k = 0; k < particles.Length; k++)
            {
                Particle particle = particles[k];
                if (particle != null && particle.active)
                {
                    particle.Update();
                }
            }

            zoneTimer++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < particles.Length; k++)
            {
                Particle particle = particles[k];
                if (particle != null && particle.active)
                {
                    particle.Draw(spriteBatch);
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

        public void CanSpawn(ParticleZone pz)
        {
            if (!ForTiles)
            {
                if (pz.zoneTimer % spawnRate == 0)
                    pz.CanSpawn = true;
            }
            else
            {
                if ((pz.zoneTimer / 5) % spawnRate == 0)
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

        public void CanSpawn(ParticleZone pz)
        {
            if (Main.rand.NextFloat(1f) < chance)
                pz.CanSpawn = true;
        }
    }

    public interface IParticleSpawner
    {
        void CanSpawn(ParticleZone pz);
    }
}
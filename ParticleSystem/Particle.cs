using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace EEMod
{
    public class Particle : Entity
    {
        internal float timeLeft;
        List<IParticleModule> Modules = new List<IParticleModule>();
        Texture2D texture;
        public virtual void OnUpdate()
        {

        }

        public virtual void OnDraw()
        {

        }
        public Particle(Vector2 position, int timeLeft,Texture2D texture, Vector2? velocity = null, List<IParticleModule> StartingModule = null)
        {
            this.timeLeft = timeLeft;
            this.position = position;
            this.velocity = velocity ?? Vector2.Zero;
            this.texture = texture;
            active = true;
            SetModules(StartingModule ?? new List<IParticleModule> { }); 
        }

        public void AddModule(IParticleModule Module) => Modules.Add(Module);
        public void SetModules(List<IParticleModule> Module) => Modules = Module;

        public void Update()
        {
            foreach(IParticleModule Module in Modules)
            {
                Module.Update(this);
            }
            position += velocity;
            OnUpdate();
            if(timeLeft > 0)
            timeLeft--;
            if (timeLeft == 0)
                active = false;
        }

        public void Draw()
        {
            Vector2 positionDraw = position.ForDraw();
            Main.spriteBatch.Draw(texture, new Rectangle((int)positionDraw.X, (int)positionDraw.Y,2,2), Color.White);
            OnDraw();
        }
    }
    class TestModule : IParticleModule
    {
        public void Update(in Particle particle)
        {
            particle.position.X++;
        }
    }
    class SlowDown : IParticleModule
    {
        float slowDownFactor;
        public SlowDown(float slowDownFactor)
        {
            this.slowDownFactor = slowDownFactor;
        }
        public void Update(in Particle particle)
        {
            particle.velocity *= slowDownFactor;
        }
    }
    class BaseModule : IParticleModule
    {
        public void Update(in Particle particle) { ; }
    }
    public interface IParticleModule
    {
        void Update(in Particle particle);
    }
}
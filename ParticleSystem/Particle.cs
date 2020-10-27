using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace EEMod
{
    public class Particle : Entity
    {
        internal float timeLeft;
        List<IParticleModule> Modules = new List<IParticleModule>();
        Texture2D texture;
        int RENDERDISTANCE => 2000;
        public float scale { get; set; }
        public float alpha;
        public Color colour;
        public float rotation;
        public virtual void OnUpdate()
        {

        }

        public virtual void OnDraw()
        {

        }
        public Particle(Vector2 position, int timeLeft,Texture2D texture, Vector2? velocity = null,int scale = 1,Color? colour = null, params IParticleModule[] StartingModule)
        {
            this.timeLeft = timeLeft;
            this.position = position;
            this.velocity = velocity ?? Vector2.Zero;
            this.texture = texture;
            active = true;
            alpha = 1;
            this.scale = scale;
            this.colour = colour ?? Color.White;
            SetModules(StartingModule.ToArray() ?? new IParticleModule[0]); 
        }

        public void AddModule(IParticleModule Module) => Modules.Add(Module);
        public void SetModules(params IParticleModule[] Module) => Modules = Module.ToList();

        public void Update()
        {
            position += velocity;
            OnUpdate();
            if(timeLeft > 1)
            timeLeft--;
            if (timeLeft == 1)
            {
                scale *= 0.98f;
                if(scale < 0.001f)
                {
                    timeLeft--;
                }
            }
            if(timeLeft == 0)
            {
                active = false;
            }

            foreach (IParticleModule Module in Modules)
            {
                Module.Update(this);
            }
        }

        public void Draw()
        {
            Vector2 positionDraw = position.ForDraw();
            Main.spriteBatch.Draw(texture, positionDraw,new Rectangle(0, 0,1, 1), colour * alpha, rotation, new Rectangle(0, 0, 1, 1).Size()/2, scale, SpriteEffects.None, 0f);
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
    class RotateTexture : IParticleModule
    {
        float rotationSpeed;
        public RotateTexture(float rotationSpeed)
        {
            this.rotationSpeed = rotationSpeed;
        }
        public void Update(in Particle particle)
        {
            particle.rotation += rotationSpeed;
        }
    }
    class SimpleBrownianMotion : IParticleModule
    {
        float intensity;
        public SimpleBrownianMotion(float intensity)
        {
            this.intensity = intensity;
        }
        public void Update(in Particle particle)
        {
            particle.velocity.X += Main.rand.NextFloat(-1,1)*intensity;
            particle.velocity.Y += Main.rand.NextFloat(-1,1)*intensity;
        }
    }
    class AdditiveCircularMotion : IParticleModule
    {
        float width;
        float height;
        float speed;
        float timer;
        public AdditiveCircularMotion(float width, float height, float speed)
        {
            this.width = width;
            this.height = height;
            this.speed = speed;
        }
        public void Update(in Particle particle)
        {
            timer += speed;
            Vector2 rotVec = new Vector2((float)Math.Sin(timer) * width, (float)Math.Cos(timer) * height);
            particle.position.X += rotVec.X;
            particle.position.Y += rotVec.Y;
        }
    }
    class Spew : IParticleModule
    {
        float randomAngle;
        float randomSpeed;
        Vector2 initialSpeed;
        float airResistance;
        bool initial;
        public Spew(float randomAngle, float randomSpeed, Vector2 initialSpeed, float airResistance)
        {
            this.randomAngle = randomAngle;
            this.randomSpeed = randomSpeed;
            this.initialSpeed = initialSpeed;
            this.airResistance = airResistance;
        }
        public void Update(in Particle particle)
        {
            if (!initial)
            {
                initial = true;
                float randVel = 1 + Main.rand.NextFloat(-randomSpeed / 2, randomSpeed / 2);
                float randAngle = Main.rand.NextFloat(-randomAngle / 2, randomAngle / 2);
                particle.velocity = new Vector2(initialSpeed.X, initialSpeed.Y).RotatedBy(randAngle)*randVel;
            }
            particle.velocity *= airResistance;
        }
    }
    class AddVelocity : IParticleModule
    {
        Vector2 velocity;
        public AddVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        public void Update(in Particle particle)
        {
            particle.velocity += velocity;
        }
    }
    class RotateVelocity : IParticleModule
    {
        float rotFac;
        public RotateVelocity(float rotFac)
        {
            this.rotFac = rotFac;
        }
        public void Update(in Particle particle)
        {
            particle.velocity = particle.velocity.RotatedBy(rotFac);
        }
    }

    class CircularMotion : IParticleModule
    {
        float width;
        float height;
        float speed;
        float timer;
        Entity orbitPoint;
        float rotation;
        public CircularMotion(float width, float height, float speed, Entity orbitPoint,float rotation = 0f)
        {
            this.width = width;
            this.height = height;
            this.speed = speed;
            this.orbitPoint = orbitPoint;
            this.rotation = rotation;
        }
        public void Update(in Particle particle)
        {
            timer += speed;
            Vector2 rotVec = new Vector2((float)Math.Sin(timer) * width, (float)Math.Cos(timer) * height).RotatedBy(rotation);
            particle.position.X = orbitPoint.Center.X + rotVec.X;
            particle.position.Y = orbitPoint.Center.Y + rotVec.Y;
        }
    }
    class CircularMotionSin : IParticleModule
    {
        float width;
        float height;
        float speed;
        float timer;
        Entity orbitPoint;
        float rotation;
        float intensity;
        float period;
        bool disapearFromBack;
        public CircularMotionSin(float width, float height, float speed, Entity orbitPoint, float rotation = 0f,float intensity = 0f, float period = 0f,bool disapearFromBack = false)
        {
            this.width = width;
            this.height = height;
            this.speed = speed;
            this.orbitPoint = orbitPoint;
            this.rotation = rotation;
            this.intensity = intensity;
            this.disapearFromBack = disapearFromBack;
        }
        public void Update(in Particle particle)
        {
            timer += speed * (1 + (float)Math.Sin(timer*period) * intensity);
            Vector2 rotVec = new Vector2((float)Math.Sin(timer) * width, (float)Math.Cos(timer) * height).RotatedBy(rotation);
            particle.position.X = orbitPoint.Center.X + rotVec.X;
            particle.position.Y = orbitPoint.Center.Y + rotVec.Y;
            if(disapearFromBack)
            {
               if(timer % (float)Math.PI*4 < (float)Math.PI)
                {
                    particle.alpha = 0f;
                }
               else
                {
                    particle.alpha = 1f;
                }
            }
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
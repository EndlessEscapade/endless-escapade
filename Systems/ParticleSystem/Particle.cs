using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public class Particle : Entity, IDrawAdditive
    {
        internal float timeLeft;
        List<IParticleModule> Modules = new List<IParticleModule>();
        public Texture2D texture;
        public Texture2D PresetNoiseMask;
        public Texture2D mask;
        public float paralax;
        public Vector3 lightingColor;
        public float lightingIntensity = 0;
        public float shrinkSpeed;
        public bool LightingBlend;
        public float alphaFade = 14f;
        public float MaskAlpha = 0.5f;
        protected int RENDERDISTANCE => 2000;
        internal bool WithinDistance => Vector2.DistanceSquared(position, Main.LocalPlayer.position) < RENDERDISTANCE * RENDERDISTANCE;
        public float varScale;
        public float scale { get; set; }
        public float alpha;
        public Color colour;
        public float rotation;
        public int TrailLength;
        public Rectangle Frame;
        public int noOfFrames;
        public int AnimSpeedPerTick;
        public int CurrentFrame => (int)(Main.GameUpdateCount / AnimSpeedPerTick) % noOfFrames;
        public Vector2 PARALAXPOSITION => position.ParalaxX(paralax);
        public List<Vector2> PositionCache = new List<Vector2>();
        public void UpdatePositionCache()
        {
            PositionCache.Insert(0, position);
            if (PositionCache.Count > TrailLength)
            {
                PositionCache.RemoveAt(PositionCache.Count - 1);
            }
        }
        public virtual void OnUpdate() { }

        public virtual void OnDraw(SpriteBatch spriteBatch) { }
        public Particle(Vector2 position, int timeLeft, Texture2D texture, Vector2 velocity = default, float scale = 1, Color? colour = null, Texture2D masks = null, params IParticleModule[] StartingModule)
        {
            this.timeLeft = timeLeft;
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;
            active = true;
            this.scale = scale;
            alpha = 1;
            shrinkSpeed = 0.99f;
            this.colour = colour ?? Color.White;
            TrailLength = 8;
            Frame = new Rectangle(0, 0, 1, 1);
            PresetNoiseMask = masks;
            lightingColor = new Vector3(0, 0, 0);
            lightingIntensity = 0;
            LightingBlend = false;
            AnimSpeedPerTick = 1;
            noOfFrames = 1;
            SetModules(StartingModule.ToArray() ?? new IParticleModule[0]);
            AdditiveCalls.Instance.LoadObject(this);
        }

        public void AddModule(IParticleModule Module) => Modules.Add(Module);
        public void SetModules(params IParticleModule[] Module) => Modules = Module.ToList();

        public void Update()
        {
            position += velocity;
            OnUpdate();
            UpdatePositionCache();
            if (timeLeft > 1)
                timeLeft--;
            if (timeLeft == 1)
            {
                varScale *= shrinkSpeed;
                if (varScale < 0.2f)
                {
                    timeLeft--;
                }
            }
            else if (Math.Abs(scale - varScale) > 0.01f && timeLeft != 0)
            {
                varScale += (scale - varScale) / 14f;
            }
            if (timeLeft == 0)
            {
                active = false;
                AdditiveCalls.Instance.DisposeObject(this);
            }
            if (lightingIntensity > 0)
            {
                Lighting.AddLight(position, lightingColor * lightingIntensity * varScale);
            }
            foreach (IParticleModule Module in Modules)
            {
                Module.Update(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IParticleModule Module in Modules)
            {
                Module.Draw(this);
            }
            Vector2 positionDraw = position.ForDraw();
            spriteBatch.Draw(texture, positionDraw.ParalaxX(paralax), new Rectangle(0, CurrentFrame * (Frame.Height / noOfFrames), Frame.Width, Frame.Height / noOfFrames), LightingBlend ? Lighting.GetColor((int)PARALAXPOSITION.X / 16, (int)PARALAXPOSITION.Y / 16) * alpha : colour * alpha, rotation, Frame.Size() / 2, varScale, SpriteEffects.None, 0f);

            OnDraw(spriteBatch);
        }

        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            Vector2 positionDraw = position.ForDraw();
            if (PresetNoiseMask != null)
                Helpers.DrawAdditiveFunkyNoBatch(PresetNoiseMask, positionDraw.ParalaxX(paralax), colour * alpha, 0.4f, 0.14f);
            if (mask != null)
                spriteBatch.Draw(mask, positionDraw.ParalaxX(paralax), mask.Bounds, colour * varScale * MaskAlpha, 0f, mask.TextureCenter(), 0.1f * varScale, SpriteEffects.None, 0f);
        }
    }
    class TestModule : IParticleModule
    {
        public void Update(in Particle particle)
        {
            particle.position.X++;
        }
        public void Draw(in Particle particle) {; }
    }
    class MovementSin : IParticleModule
    {
        int counter;
        float frequency;
        public MovementSin(float frequency)
        {
            this.frequency = frequency;
        }
        public void Update(in Particle particle)
        {
            counter++;
            particle.velocity *= Math.Abs((float)Math.Sin(counter * frequency));
        }
        public void Draw(in Particle particle) {; }
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
        public void Draw(in Particle particle) {; }
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
        public void Draw(in Particle particle) {; }
    }
    class SetAnimData : IParticleModule
    {
        int AnimSpeedPerTick;
        int noOfFrames;
        public SetAnimData(int AnimSpeedPerTick, int noOfFrames)
        {
            this.AnimSpeedPerTick = AnimSpeedPerTick;
            this.noOfFrames = noOfFrames;
        }
        public void Update(in Particle particle)
        {
            particle.AnimSpeedPerTick = AnimSpeedPerTick;
            particle.noOfFrames = noOfFrames;
        }
        public void Draw(in Particle particle) {; }
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
            particle.velocity.X += Main.rand.NextFloat(-1, 1) * intensity;
            particle.velocity.Y += Main.rand.NextFloat(-1, 1) * intensity;
        }
        public void Draw(in Particle particle) {; }
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
        public void Draw(in Particle particle) {; }
    }
    class AfterImageTrail : IParticleModule
    {
        float alphaFallOff;
        bool fadeToWhite = false;
        public AfterImageTrail(float alphaFallOff)
        {
            this.alphaFallOff = alphaFallOff;
        }
        public void Draw(in Particle particle)
        {
            for (int i = 0; i < particle.PositionCache.Count; i++)
            {
                float globalFallOff = 1 - (i / (float)(particle.PositionCache.Count - 1)) * alphaFallOff;
                Main.spriteBatch.Draw(Main.magicPixel, particle.PositionCache[i].ForDraw().ParalaxX(particle.paralax), new Rectangle(0, particle.CurrentFrame * (particle.Frame.Height / particle.noOfFrames), particle.Frame.Width, particle.Frame.Height / particle.noOfFrames), particle.colour * particle.alpha * globalFallOff, particle.rotation, new Rectangle(0, particle.CurrentFrame * (particle.Frame.Height / particle.noOfFrames), particle.Frame.Width, particle.Frame.Height / particle.noOfFrames).Size() / 2, particle.varScale * globalFallOff, SpriteEffects.None, 0f);
            }
        }
        public void Update(in Particle particle) {; }
    }
    class SetTimeLeft : IParticleModule
    {
        float timeLeft;
        bool initial;
        public SetTimeLeft(float timeLeft)
        {
            this.timeLeft = timeLeft;
        }
        public void Update(in Particle particle)
        {
            if (!initial)
            {
                initial = true;
                particle.timeLeft = timeLeft;
            }
        }
        public void Draw(in Particle particle) {; }
    }

    class SetTrailLength : IParticleModule
    {
        int traillength;
        public SetTrailLength(int traillength)
        {
            this.traillength = traillength;
        }
        public void Update(in Particle particle)
        {
            particle.TrailLength = traillength;
        }
        public void Draw(in Particle particle) {; }
    }
    class SetLightingBlend : IParticleModule
    {
        bool LightingBlend;
        bool initial;
        public SetLightingBlend(bool LightingBlend)
        {
            this.LightingBlend = LightingBlend;
        }
        public void Update(in Particle particle)
        {
            if (!initial)
            {
                initial = true;
                particle.LightingBlend = LightingBlend;
            }
        }
        public void Draw(in Particle particle) {; }
    }
    class SetShrinkSize : IParticleModule
    {
        float shrinkSize;
        bool initial;
        public SetShrinkSize(float shrinkSize)
        {
            this.shrinkSize = shrinkSize;
        }
        public void Update(in Particle particle)
        {
            if (!initial)
            {
                initial = true;
                particle.shrinkSpeed = shrinkSize;
            }
        }
        public void Draw(in Particle particle) {; }
    }
    class SetParalax : IParticleModule
    {
        float paralax;
        bool initial;
        public SetParalax(float paralax)
        {
            this.paralax = paralax;
        }
        public void Update(in Particle particle)
        {
            if (!initial)
            {
                initial = true;
                particle.paralax = paralax;
            }
        }
        public void Draw(in Particle particle) {; }
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
                particle.velocity = new Vector2(initialSpeed.X, initialSpeed.Y).RotatedBy(randAngle) * randVel;
            }
            particle.velocity *= airResistance;
        }
        public void Draw(in Particle particle) {; }
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
        public void Draw(in Particle particle) {; }
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
        public void Draw(in Particle particle) {; }
    }
    class SetLighting : IParticleModule
    {
        Vector3 color;
        float intensity;
        public SetLighting(Vector3 color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }
        public void Update(in Particle particle)
        {
            particle.lightingColor = color;
            particle.lightingIntensity = intensity;
        }
        public void Draw(in Particle particle) {; }
    }
    class SetFrame : IParticleModule
    {
        Rectangle frame;
        public SetFrame(Rectangle bounds)
        {
            frame = bounds;
        }
        public void Update(in Particle particle)
        {
            particle.Frame = frame;
        }
        public void Draw(in Particle particle) {; }
    }
    class SetPresetNoiseMask : IParticleModule
    {
        Texture2D tex;
        public SetPresetNoiseMask(Texture2D bounds)
        {
            tex = bounds;
        }
        public void Update(in Particle particle)
        {
            particle.PresetNoiseMask = tex;
        }
        public void Draw(in Particle particle) {; }
    }
    class SetMask : IParticleModule
    {
        Texture2D tex;
        float MaskAlpha;
        public SetMask(Texture2D bounds, float MaskAlpha = 0.25f)
        {
            tex = bounds;
            this.MaskAlpha = MaskAlpha;
        }
        public void Update(in Particle particle)
        {
            particle.mask = tex;
            particle.MaskAlpha = MaskAlpha;
        }
        public void Draw(in Particle particle) {; }
    }
    class CircularMotion : IParticleModule
    {
        float width;
        float height;
        float speed;
        float timer;
        Entity orbitPoint;
        float rotation;
        public CircularMotion(float width, float height, float speed, Entity orbitPoint, float rotation = 0f)
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
        public void Draw(in Particle particle) {; }
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
        public CircularMotionSin(float width, float height, float speed, Entity orbitPoint, float rotation = 0f, float intensity = 0f, float period = 0f, bool disapearFromBack = false)
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
            timer += speed * (1 + (float)Math.Sin(timer * period) * intensity);
            Vector2 rotVec = new Vector2((float)Math.Sin(timer) * width, (float)Math.Cos(timer) * height).RotatedBy(rotation);
            particle.position.X = orbitPoint.Center.X + rotVec.X;
            particle.position.Y = orbitPoint.Center.Y + rotVec.Y;
            if (disapearFromBack)
            {
                if (timer % (float)Math.PI * 4 < (float)Math.PI)
                {
                    particle.alpha = 0f;
                }
                else
                {
                    particle.alpha = 1f;
                }
            }
        }
        public void Draw(in Particle particle) {; }
    }
    class FollowEntity : IParticleModule
    {
        float resistance;
        float dampen;
        Entity orbitPoint;

        public FollowEntity(Entity orbitPoint, float resistance = 100f, float dampen = 10f)
        {
            this.dampen = dampen;
            this.orbitPoint = orbitPoint;
            this.resistance = resistance;
        }
        public void Update(in Particle particle)
        {
            particle.velocity += (orbitPoint.Center - particle.position) / resistance - particle.velocity / dampen;
        }
        public void Draw(in Particle particle) {; }
    }
    class CircularMotionSinSpinC : IParticleModule
    {
        float width;
        float height;
        float speed;
        float timer;
        Vector2 Center;
        float rotation;
        float intensity;
        float period;
        float spinSpeed;
        bool disapearFromBack;
        float rot;
        public CircularMotionSinSpinC(float width, float height, float speed, Vector2 orbitPoint, float spinSpeed = 0f, float rotation = 0f, float intensity = 0f, float period = 0f, bool disapearFromBack = false)
        {
            this.width = width;
            this.height = height;
            this.speed = speed;
            this.Center = orbitPoint;
            this.rotation = rotation;
            this.intensity = intensity;
            this.disapearFromBack = disapearFromBack;
            this.spinSpeed = spinSpeed;
        }
        public void Update(in Particle particle)
        {
            timer += speed * (1 + (float)Math.Sin(timer * period) * intensity);
            rot += spinSpeed;
            Vector2 rotVec = new Vector2((float)Math.Sin(timer) * width, (float)Math.Cos(timer) * height).RotatedBy(rotation);
            Vector2 alt = rotVec.RotatedBy(rot);
            particle.position.X = Center.X + alt.X;
            particle.position.Y = Center.Y + alt.Y;
            if (disapearFromBack)
            {
                if (timer % (float)Math.PI * 4 < (float)Math.PI)
                {
                    particle.alpha = 0f;
                }
                else
                {
                    particle.alpha = 1f;
                }
            }
        }
        public void Draw(in Particle particle) {; }
    }
    class CircularMotionSinSpin : IParticleModule
    {
        float width;
        float height;
        float speed;
        float timer;
        Entity orbitPoint;
        float rotation;
        float intensity;
        float period;
        float spinSpeed;
        bool disapearFromBack;
        float rot;
        public CircularMotionSinSpin(float width, float height, float speed, Entity orbitPoint, float spinSpeed = 0f, float rotation = 0f, float intensity = 0f, float period = 0f, bool disapearFromBack = false)
        {
            this.width = width;
            this.height = height;
            this.speed = speed;
            this.orbitPoint = orbitPoint;
            this.rotation = rotation;
            this.intensity = intensity;
            this.disapearFromBack = disapearFromBack;
            this.spinSpeed = spinSpeed;
        }
        public void Update(in Particle particle)
        {
            timer += speed * (1 + (float)Math.Sin(timer * period) * intensity);
            rot += spinSpeed;
            Vector2 rotVec = new Vector2((float)Math.Sin(timer) * width, (float)Math.Cos(timer) * height).RotatedBy(rotation);
            Vector2 alt = rotVec.RotatedBy(rot);
            particle.position.X = orbitPoint.Center.X + alt.X;
            particle.position.Y = orbitPoint.Center.Y + alt.Y;
            if (disapearFromBack)
            {
                if (timer % (float)Math.PI * 4 < (float)Math.PI)
                {
                    particle.alpha = 0f;
                }
                else
                {
                    particle.alpha = 1f;
                }
            }
        }
        public void Draw(in Particle particle) {; }
    }
    class ZigzagMotion : IParticleModule
    {
        float interval;
        float maxDistance;
        int timer;
        public ZigzagMotion(float interval, float maxDistance)
        {
            this.interval = interval;
            this.maxDistance = maxDistance;
        }
        public void Update(in Particle particle)
        {
            timer++;
            if (timer > interval)
            {
                particle.velocity = particle.velocity.RotatedBy(Main.rand.NextFloat(-maxDistance, maxDistance));
                timer = 0;
            }
        }
        public void Draw(in Particle particle) {; }
    }
    class BaseModule : IParticleModule
    {
        public void Update(in Particle particle) {; }
        public void Draw(in Particle particle) {; }
    }
    public interface IParticleModule
    {
        void Update(in Particle particle);
        void Draw(in Particle particle);
    }
}
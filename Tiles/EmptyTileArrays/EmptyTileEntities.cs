

using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EEMod.Tiles.EmptyTileArrays
{
    public class EmptyTileDrawEntity
    {
        public Vector2 position;
        public int activeTime;
        public float alpha = 1;
        public Color colour = Color.White;
        public float rotation;
        public Vector2 origin;
        public Vector2 ScreenPosition => position * 16;
        public int RENDERDISTANCE => 2000;
        public virtual int activityTime { get; set; }
        public virtual string tex { get; set; }

        public bool CanActivate { get; set; }
        public Texture2D texture => EEMod.instance.GetTexture(tex);
        public EmptyTileDrawEntity(Vector2 position, string text)
        {
            this.position = position;
            tex = text;
            origin = new Vector2(0, texture.Height);
        }
        public void Activiate()
        {
            if(CanActivate)
            activeTime = activityTime;
        }
        public virtual void DuringActivation()
        {

        }
        public virtual void DuringNonActivation()
        {

        }

        public virtual void OnUpdate()
        {

        }
        public void Update()
        {
            if ((position * 16 - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
            {
                OnUpdate();
                if (activeTime > 0)
                {
                    CanActivate = false;
                    DuringActivation();
                    activeTime--;
                }
                else
                {
                    CanActivate = true;
                    DuringNonActivation();
                }
            }
        }
        public virtual void Draw()
        {
            if ((position * 16 - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                Main.spriteBatch.Draw(texture, (position*16).ForDraw() + new Vector2(0, texture.Height),new Rectangle(0,0, texture.Width, texture.Height), colour * alpha, rotation, origin, 1f,SpriteEffects.None,0f);
        }
    }
    public static class EmptyTileEntityCache
    {
        static internal Dictionary<Vector2, Vector2> EmptyTilePairs = new Dictionary<Vector2, Vector2>();
        static internal Dictionary<Vector2, EmptyTileDrawEntity> EmptyTileEntityPairs = new Dictionary<Vector2, EmptyTileDrawEntity>();

        public static void AddPair(EmptyTileDrawEntity ETE, Vector2 position, byte[,,] array)
        {
            if(!EmptyTileEntityPairs.ContainsKey(position))
            EmptyTileEntityPairs.Add(position, ETE);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i,0] == 1)
                    {
                        if (!EmptyTilePairs.ContainsKey(position + new Vector2(i, j)))
                            EmptyTilePairs.Add(position + new Vector2(i, j), position);
                    }
                }
            }
            EEWorld.EEWorld.CreateInvisibleTiles(array, position);
        }
        public static void Remove(Vector2 position)
        {
            EmptyTileEntityPairs.Remove(Convert(position));
            foreach (var item in EmptyTilePairs.Where(kvp => kvp.Value == Convert(position)).ToList())
            {
                //WorldGen.KillTile((int)item.Key.X,(int)item.Key.Y);
                EmptyTilePairs.Remove(item.Key);
            }
        }
        public static Vector2 Convert(Vector2 position)
        {
            return EmptyTilePairs[position];
        }

        public static void Update()
        {
            foreach(EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values)
            {
                if(ETE != null)
                ETE.Update();
            }
        }
        public static void Draw()
        {
            foreach (EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values)
            {
                if (ETE != null)
                    ETE.Draw();
            }
        }
        public static void Invoke(Vector2 position)
        {
            EmptyTileEntityPairs[Convert(position)].Activiate();
        }
    }
    public class Crystal : EmptyTileDrawEntity
    {
        public float speed;
        public Texture2D glow => EEMod.instance.GetTexture(glowPath);
        public string glowPath;
        public float shaderLerp;
        public float lerp;
        public Crystal(Vector2 position, string texture, string glow) : base(position, texture)
        {
            this.position = position;
            tex = texture;
            speed = Main.rand.NextFloat(0.01f, 0.03f);
            glowPath = glow;
        }
        public override int activityTime => 20;

        public override void DuringActivation()
        {
            shaderLerp = 1 + (float)Math.Sin((Math.PI / (float)activityTime) * activeTime);
            colour = Color.Lerp(Lighting.GetColor((int)position.X, (int)position.Y), Color.LightBlue, (float)Math.Sin((Math.PI / (float)activityTime) * activeTime));
            rotation = (shaderLerp-1)/20f;
        }
        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + texture.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + texture.Height));
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.01f));
            EEMod.Particles.Get("Main").SpawnParticles(rand,new Vector2(Main.rand.NextFloat(-1f,1f), Main.rand.NextFloat(-1f,1f)), 2,Color.LightBlue, new SlowDown(0.98f), new RotateVelocity(Main.rand.NextFloat(-.08f, .08f)));
            rotation = 0;
            shaderLerp = 1;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }

        public override void Draw()
        {
            lerp += speed;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.ReflectionShader.Parameters["alpha"].SetValue(lerp * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(colour.ToVector3());
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(glow);
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(shaderLerp/3f);
            EEMod.ReflectionShader.CurrentTechnique.Passes[0].Apply();
            if ((position * 16 - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                Main.spriteBatch.Draw(texture, (position * 16).ForDraw() + new Vector2(0, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), colour * alpha, rotation, new Vector2(0, texture.Height), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

    }
    public class BigCrystal : EmptyTileDrawEntity
    {
        public float speed;
        public Texture2D glow => EEMod.instance.GetTexture(glowPath);
        public string glowPath;
        public float shaderLerp;
        public float lerp;
        public BigCrystal(Vector2 position, string text, string glow) : base(position, text)
        {
            this.position = position;
            tex = text;
            speed = Main.rand.NextFloat(0.01f,0.02f);
            glowPath = glow;
            origin = new Vector2(texture.Width, texture.Height);
        }
        public override int activityTime => 40;

        public override void DuringActivation()
        {
            shaderLerp = 1 + (float)Math.Sin((Math.PI / (float)activityTime) * activeTime);
            colour = Color.Lerp(Lighting.GetColor((int)position.X, (int)position.Y), Color.LightBlue, (float)Math.Sin((Math.PI / (float)activityTime) * activeTime));
            rotation = (shaderLerp - 1) / 100f;
        }
        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + texture.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + texture.Height));
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.02f));
            EEMod.Particles.Get("Main").SpawnParticles(rand, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), 3, new Color(7, 185, 172), new SlowDown(0.98f), new RotateTexture(0.01f), new RotateVelocity(Main.rand.NextFloat(-.18f, .18f)));
            rotation = 0;
            shaderLerp = 1;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }

        public override void Draw()
        {
            lerp += speed;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.ReflectionShader.Parameters["alpha"].SetValue(lerp * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(colour.ToVector3());
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(glow);
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(shaderLerp / 3f);
            EEMod.ReflectionShader.CurrentTechnique.Passes[0].Apply();
            if ((position * 16 - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                Main.spriteBatch.Draw(texture, (position * 16).ForDraw() + new Vector2(texture.Width, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), colour * alpha, rotation, origin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

    }
}

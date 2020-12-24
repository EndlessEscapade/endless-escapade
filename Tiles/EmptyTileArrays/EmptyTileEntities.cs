using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria.ModLoader;
using System.IO;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

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
        public virtual int ActivityTime { get; set; }
        public virtual string Tex { get; set; }

        public bool CanActivate { get; set; }
        public Texture2D texture => EEMod.instance.GetTexture(Tex);
        public EmptyTileDrawEntity(Vector2 position, string text)
        {
            this.position = position;
            Tex = text;
            origin = new Vector2(0, texture.Height);
        }
        public void Activiate()
        {
            if (CanActivate)
                activeTime = ActivityTime;
        }
        public virtual void DuringActivation()
        {

        }
        public virtual void DuringNonActivation()
        {

        }
        public void Destroy()
        {
            OnDestroy();
            EmptyTileEntityCache.EmptyTileEntityPairs.Remove(position);
            try
            {
                foreach (var item in EmptyTileEntityCache.EmptyTilePairs.Where(kvp => kvp.Value == position).ToList()) // Turning into a list is needed because if the collection is modified while it's looping an exception will be thrown
                {
                    if (Main.tile[(int)item.Key.X, (int)item.Key.Y].active())
                        WorldGen.KillTile((int)item.Key.X, (int)item.Key.Y);
                }
            }
            catch
            {
                Main.NewText("TileNotFound");
            }

            EmptyTileEntityCache.EmptyTilePairs.Remove(position);
        }
        public virtual void OnDestroy()
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
                Main.spriteBatch.Draw(texture, (position * 16).ForDraw() + new Vector2(0, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), colour * alpha, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
    public static class EmptyTileEntityCache
    {
        static internal Dictionary<Vector2, Vector2> EmptyTilePairs = new Dictionary<Vector2, Vector2>();
        static internal Dictionary<Vector2, EmptyTileDrawEntity> EmptyTileEntityPairs = new Dictionary<Vector2, EmptyTileDrawEntity>();

        public static void AddPair(EmptyTileDrawEntity ETE, Vector2 position, byte[,,] array)
        {
            if (!EmptyTileEntityPairs.ContainsKey(position))
                EmptyTileEntityPairs.Add(position, ETE);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i, 0] == 1)
                    {
                        if (!EmptyTilePairs.ContainsKey(position + new Vector2(i, j)))
                            EmptyTilePairs.Add(position + new Vector2(i, j), position);
                    }
                }
            }
            EEWorld.EEWorld.CreateInvisibleTiles(array, position);
        }
        public static void Remove(Vector2 position) =>
            EmptyTileEntityPairs[Convert(position)].Destroy();

        public static Vector2 Convert(Vector2 position) => EmptyTilePairs.TryGetValue(position, out var val) ? val : Vector2.Zero;
        //{
        //    if (EmptyTilePairs.ContainsKey(position))
        //        return EmptyTilePairs[position];
        //    else
        //    {
        //        return Vector2.Zero;
        //    }
        //}

        public static void Update()
        {
            foreach (EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values.ToList()) // List because if the collection is modified an exception will be thrown
            {
                if (ETE != null)
                    ETE.Update();
            }
        }
        public static void Draw()
        {
            foreach (EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values.ToList())
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
            Tex = texture;
            speed = Main.rand.NextFloat(0.01f, 0.03f);
            glowPath = glow;
        }
        public override int ActivityTime => 20;

        public override void DuringActivation()
        {
            shaderLerp = 1 + (float)Math.Sin((Math.PI / (float)ActivityTime) * activeTime);
            colour = Color.Lerp(Lighting.GetColor((int)position.X, (int)position.Y), Color.LightBlue, (float)Math.Sin((Math.PI / (float)ActivityTime) * activeTime));
            rotation = (shaderLerp - 1) / 20f;
        }
        public override void OnUpdate()
        {
            Tile tile = Main.tile[(int)position.X, (int)position.Y];
            lerp += speed;
            if (tile.type != ModContent.TileType<EmptyTile>() && tile.active())
            {
                Destroy();
            }
        }
        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + texture.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + texture.Height));
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.01f));
            EEMod.Particles.Get("Main").SpawnParticles(rand, new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)), ModContent.GetTexture("EEMod/Particles/Crystal"), 60, 1, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), new SlowDown(0.98f), new RotateTexture(0.01f), new RotateVelocity(Main.rand.NextFloat(-.1f, .1f)), new SetMask(Helpers.RadialMask,0.4f));
            rotation = 0;
            shaderLerp = 1;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }

        public override void Draw()
        {

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.ReflectionShader.Parameters["alpha"].SetValue(lerp * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(colour.ToVector3());
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(glow);
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(shaderLerp / 3f);
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
            Tex = text;
            speed = Main.rand.NextFloat(0.01f, 0.02f);
            glowPath = glow;
            origin = new Vector2(texture.Width, texture.Height);
        }
        public override int ActivityTime => 40;

        public override void DuringActivation()
        {
            shaderLerp = 1 + (float)Math.Sin((Math.PI / (float)ActivityTime) * activeTime);
            colour = Color.Lerp(Lighting.GetColor((int)position.X, (int)position.Y), Color.LightBlue, (float)Math.Sin((Math.PI / (float)ActivityTime) * activeTime));
            rotation = (shaderLerp - 1) / 100f;
        }
        public override void OnUpdate()
        {
            lerp += speed;
            Tile tile = Main.tile[(int)position.X, (int)position.Y];
            if (tile.type != ModContent.TileType<EmptyTile>() && tile.active())
            {
                Destroy();
            }
        }
        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + texture.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + texture.Height));
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.02f));
            EEMod.Particles.Get("Main").SpawnParticles(rand, new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)), ModContent.GetTexture("EEMod/Particles/Crystal"), 60, 1, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), new SlowDown(0.98f), new RotateTexture(0.01f), new RotateVelocity(Main.rand.NextFloat(-.1f, .1f)), new SetMask(Helpers.RadialMask));
            rotation = 0;
            shaderLerp = 1;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }

        public override void Draw()
        {
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

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
    public class EmptyTileEntity
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
        public Texture2D texture => EEMod.Instance.Assets.Request<Texture2D>(Tex).Value;

        public EmptyTileEntity(Vector2 position, string text)
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
            EmptyTileEntities.Instance.EmptyTileEntityPairsCache.Remove(position);
            try
            {
                foreach (var item in EmptyTileEntities.Instance.EmptyTilePairsCache.Where(kvp => kvp.Value == position).ToList()) // Turning into a list is needed because if the collection is modified while it's looping an exception will be thrown
                {
                    if (Framing.GetTileSafely((int)item.Key.X, (int)item.Key.Y).IsActive)
                        WorldGen.KillTile((int)item.Key.X, (int)item.Key.Y);
                }
            }
            catch
            {
                Main.NewText("TileNotFound");
            }

            EmptyTileEntities.Instance.EmptyTilePairsCache.Remove(position);
        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public void Update()
        {
            if ((ScreenPosition - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
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

        public void Draw()
        {
            if ((ScreenPosition - Main.LocalPlayer.Center).LengthSquared() > RENDERDISTANCE * RENDERDISTANCE)
                return;
                OnDraw();
        }

        public virtual void OnDraw()
        {
            Main.spriteBatch.Draw(texture, (position * 16).ForDraw() + new Vector2(0, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), colour * alpha, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }

    public class Crystal : EmptyTileEntity
    {
        public float speed;
        public Texture2D glow => EEMod.Instance.Assets.Request<Texture2D>(glowPath).Value;
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
            Tile tile = Framing.GetTileSafely((int)position.X, (int)position.Y);

            lerp += speed;

            if (tile.type != ModContent.TileType<EmptyTile>() && tile.IsActive)
            {
                Destroy();
            }
        }

        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + texture.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + texture.Height));

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.0075f));
            EEMod.MainParticles.SpawnParticles(rand, new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)), ModContent.Request<Texture2D>("EEMod/Particles/Crystal").Value, 60, 1.5f, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), new SlowDown(0.98f), new RotateTexture(0.01f), new RotateVelocity(Main.rand.NextFloat(-.1f, .1f)), new SetMask(Helpers.RadialMask, 0.4f));

            rotation = 0;
            shaderLerp = 1;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }

        public override void OnDraw()
        {
            Player myPlayer = Main.LocalPlayer;

            Vector2 DrawPos = position * 16;
            Vector2 Scaling = new Vector2(texture.Width / (float)Helpers.playerTexture.Width, texture.Height / (float)Helpers.playerTexture.Height);

            float percX = (myPlayer.Center.X - DrawPos.X) / texture.Width;
            float percY = (myPlayer.Center.Y - DrawPos.Y) / texture.Height;

            EEMod.ReflectionShader.Parameters["alpha"].SetValue(lerp * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.5f);
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(colour.ToVector3());
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(glow);
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(shaderLerp / 3f);
            EEMod.ReflectionShader.Parameters["headTexture"].SetValue(Helpers.playerTexture);
            EEMod.ReflectionShader.Parameters["XPROG"].SetValue(percX);
            EEMod.ReflectionShader.Parameters["YPROG"].SetValue(percY);
            EEMod.ReflectionShader.Parameters["Scaling"].SetValue(Scaling);

            EEMod.ReflectionShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(texture, DrawPos.ForDraw() + new Vector2(0, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), colour * alpha, rotation, new Vector2(texture.Width / 2, texture.Height), 1f, SpriteEffects.None, 0f);
        }
    }

    public class BigCrystal : EmptyTileEntity
    {
        public float speed;
        public Texture2D glow => EEMod.Instance.Assets.Request<Texture2D>(glowPath).Value;
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
            Tile tile = Framing.GetTileSafely((int)position.X, (int)position.Y);
            if (tile.type != ModContent.TileType<EmptyTile>() && tile.IsActive)
            {
                Destroy();
            }
        }

        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + texture.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + texture.Height));
            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.02f));
            EEMod.MainParticles.SpawnParticles(rand, new Vector2(Main.rand.NextFloat(-0.75f, 0.75f), Main.rand.NextFloat(-0.75f, 0.75f)), ModContent.Request<Texture2D>("EEMod/Particles/Crystal").Value, 60, 1, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), new SlowDown(0.98f), new RotateTexture(0.01f), new RotateVelocity(Main.rand.NextFloat(-.1f, .1f)), new SetMask(Helpers.RadialMask));
            rotation = 0;
            shaderLerp = 1;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }

        public override void OnDraw()
        {
            Player myPlayer = Main.LocalPlayer;
            Vector2 DrawPos = position * 16;
            Vector2 Scaling = new Vector2(texture.Width / (float)Helpers.playerTexture.Width, texture.Height / (float)Helpers.playerTexture.Height);
            float percX = (myPlayer.Center.X - DrawPos.X) / texture.Width;
            float percY = (myPlayer.Center.Y - DrawPos.Y) / texture.Height;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.ReflectionShader.Parameters["alpha"].SetValue(lerp * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(colour.ToVector3());
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(glow);
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(shaderLerp / 3f);
            EEMod.ReflectionShader.Parameters["headTexture"].SetValue(Helpers.playerTexture);
            EEMod.ReflectionShader.Parameters["XPROG"].SetValue(percX);
            EEMod.ReflectionShader.Parameters["YPROG"].SetValue(percY);
            EEMod.ReflectionShader.Parameters["Scaling"].SetValue(Scaling);
            EEMod.ReflectionShader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(texture, (position * 16).ForDraw() + new Vector2(texture.Width, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), colour * alpha, rotation, origin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}

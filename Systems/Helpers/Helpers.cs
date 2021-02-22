using EEMod.Extensions;
using EEMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EEMod.Tiles.Furniture;

namespace EEMod
{
    public static partial class Helpers
    {
        internal const string EmptyTexture = "EEMod/Empty";
        // public static InteritosGlobalNPC Interitos(this NPC npc) => npc.GetGlobalNPC<InteritosGlobalNPC>();
        // public static InteritosGlobalProjectile Interitos(this Projectile proj) => proj.GetGlobalProjectile<InteritosGlobalProjectile>();

        public const BindingFlags FlagsInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public const BindingFlags FlagsStatic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        public const BindingFlags FlagsALL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public const float DegreeInRadians = (float)(Math.PI / 180);
        public const float RadianInDegrees = (float)(180 / Math.PI);

        private static float X(float t,
    float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 3) +
                x1 * 3 * t * Math.Pow(1 - t, 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow(1 - t, 3) +
                 y1 * 3 * t * Math.Pow(1 - t, 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }
        private static float X(float t,
   float x0, float x1, float x2)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 2) +
                x1 * 2 * t * (1 - t) +
                x2 * Math.Pow(t, 2)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2)
        {
            return (float)(
                y0 * Math.Pow(1 - t, 2) +
                y1 * 2 * t * (1 - t) +
                y2 * Math.Pow(t, 2)
            );
        }
        public static int[,] ConvertTexToBitmap(string tex, int thresh)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap($@"{Main.SavePath}\Mod Sources\EEMod\" + tex + ".png");
            int[,] Array = new int[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Array[i, j] = bitmap.GetPixel(i, j).R < thresh ? 1 : 0;
                }
            }
            return Array;
        }
        public static void TexToDust(string path, Vector2 position, int accuracy = 1, float spacing = 1, int threshold = 126)
        {
            int[,] array = ConvertTexToBitmap(path, threshold);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 1 && i % accuracy == 0 && j % accuracy == 0)
                    {
                        Dust dust = Dust.NewDustPerfect(position + new Vector2(i, j) * spacing, 219, Vector2.Zero);
                        dust.noGravity = true;
                    }
                }
            }
        }
        public static void DrawBezier(SpriteBatch spriteBatch, Texture2D headTexture, string glowMaskTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, float rotDis, bool alphaBlend = false, bool emitsDust = false)
        {
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, c2.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y);
                    if (emitsDust)
                    {
                        if (Main.rand.Next(50) == 0)
                        {
                            if (!Main.tile[(int)x / 16, (int)y / 16].active())
                            {
                                Dust dust = Dust.NewDustPerfect(new Vector2(x, y), DustID.AmberBolt);
                                dust.fadeIn = 1f;
                                dust.scale = 0.1f;
                                dust.noGravity = true;
                                dust.velocity *= 0.25f;
                            }
                        }
                    }
                    distBetween = new Vector2(x -
                    X(i - chainsPerUse, startingPos.X, c1.X, c2.X, endPoints.X),
                    y -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, c2.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    spriteBatch.Draw(headTexture, new Vector2(x - Main.screenPosition.X, y - Main.screenPosition.Y),
                    new Rectangle(0, 0, headTexture.Width, headTexture.Height), alphaBlend ? Lighting.GetColor((int)(x / 16), (int)(y / 16)) : drawColor, projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), 1, SpriteEffects.None, 0);
                }
            }
            //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }
        public static void DrawBezier(Texture2D headTexture, string glowMaskTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, float addonPerUse, float rotDis, bool alphaBlend = false, bool emitsDust = false)
        {
            float width = headTexture.Width;
            float length = (startingPos - endPoints).Length();
            float chainsPerUse = (width / length) * addonPerUse;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);
                    if (emitsDust)
                    {
                        if (Main.rand.Next(50) == 0)
                        {
                            if (!Main.tile[(int)x / 16, (int)y / 16].active())
                            {
                                Dust dust = Dust.NewDustPerfect(new Vector2(x, y), DustID.BlueCrystalShard);
                                dust.fadeIn = 1f;
                                dust.scale = 0.1f;
                                dust.noGravity = true;
                                dust.velocity *= 0.25f;
                                dust.noLight = false;
                            }
                        }
                    }
                    distBetween = new Vector2(x -
                    X(i - chainsPerUse, startingPos.X, c1.X, endPoints.X),
                    y -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    Main.spriteBatch.Draw(headTexture, new Vector2(x - Main.screenPosition.X, y - Main.screenPosition.Y),
                    new Rectangle(0, 0, headTexture.Width, headTexture.Height), alphaBlend ? Lighting.GetColor((int)(x / 16), (int)(y / 16)) : drawColor, projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), 1, SpriteEffects.None, 0);
                }
            }
            //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }
        public static void DrawBezier(Texture2D headTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, float addonPerUse, Rectangle frame, float rotDis = 0f, bool alphaBlend = false, float scale = 1, bool emitsDust = false, bool fadeScale = false)
        {
            float width = frame.Width;
            float length = (startingPos - endPoints).Length();
            float chainsPerUse = (width / length) * addonPerUse;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);
                    if (emitsDust)
                    {
                        if (Main.rand.Next(50) == 0)
                        {
                            if (!Main.tile[(int)x / 16, (int)y / 16].active())
                            {
                                Dust dust = Dust.NewDustPerfect(new Vector2(x, y), DustID.AmberBolt);
                                dust.fadeIn = 1f;
                                dust.scale = 0.1f;
                                dust.noGravity = true;
                                dust.velocity *= 0.25f;
                            }
                        }
                    }
                    distBetween = new Vector2(x -
                    X(i - chainsPerUse, startingPos.X, c1.X, endPoints.X),
                    y -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    Main.spriteBatch.Draw(headTexture, new Vector2(x, y).ForDraw(),
                    frame, alphaBlend ? Lighting.GetColor((int)(x / 16), (int)(y / 16)) : drawColor, projTrueRotation,
                    frame.Center(), scale * (fadeScale ? (i + 0.5f) : 1), SpriteEffects.None, 0);
                }
            }
        }
        public static void DrawBezier(Texture2D headTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, float addonPerUse, float rotDis = 0f, bool alphaBlend = false, float scale = 1, bool emitsDust = false, bool fadeScale = false, float lerpIntensity = 0, bool TrueRotation = false, bool HasFunny = false)
        {
            float c = (Main.GameUpdateCount / (60f + endPoints.X % 20)) % 4 - 2;
            float width = headTexture.Width;
            float length = (startingPos - endPoints).Length();
            float chainsPerUse = (width / length) * addonPerUse;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);
                    if (emitsDust)
                    {
                        if (Main.rand.Next(50) == 0)
                        {
                            if (!Main.tile[(int)x / 16, (int)y / 16].active())
                            {
                                Dust dust = Dust.NewDustPerfect(new Vector2(x, y), DustID.AmberBolt);
                                dust.fadeIn = 1f;
                                dust.scale = 0.1f;
                                dust.noGravity = true;
                                dust.velocity *= 0.25f;
                            }
                        }
                    }
                    bool ifBlack = Lighting.GetColor((int)(x / 16), (int)(y / 16)) == Color.Black;
                    float cDist = 0.5f - Math.Abs(i - c) * 2;
                    float cDist2 = 2 - Math.Abs(i - c) * 2;
                    if (cDist < 0)
                        cDist = 0;
                    if (cDist2 < 0)
                        cDist2 = 0;
                    distBetween = new Vector2(x -
                    X(i - chainsPerUse, startingPos.X, c1.X, endPoints.X),
                    y -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    if (HasFunny)
                    {
                        DrawAdditiveFunky(headTexture, new Vector2(x, y).ForDraw(), Color.White, 1.1f + cDist2, cDist2 / 6f, i * 1000);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        EEMod.SolidOutline.CurrentTechnique.Passes[0].Apply();
                        EEMod.SolidOutline.Parameters["alpha"].SetValue(cDist * 0.5f);
                        Main.spriteBatch.Draw(headTexture, new Vector2(x, y).ForDraw(),
                    headTexture.Bounds, ifBlack ? Color.Black : Color.White, TrueRotation ? 0 : projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), scale * (fadeScale ? (i + 0.5f) : 1) * 1.07f, SpriteEffects.None, 0);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                    }

                    Main.spriteBatch.Draw(headTexture, new Vector2(x, y).ForDraw(),
                    headTexture.Bounds, ifBlack ? Color.Black : Color.Lerp((alphaBlend ? Lighting.GetColor((int)(x / 16), (int)(y / 16)) : drawColor), Color.White, cDist * lerpIntensity), TrueRotation ? 0 : projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), scale * (fadeScale ? (i + 0.5f) : 1), SpriteEffects.None, 0);
                }
            }
        }
        public static void DrawBezierAdditive(Texture2D headTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, float addonPerUse, float rotDis = 0f, bool alphaBlend = false, float scale = 1, bool emitsDust = false, bool fadeScale = false, float lerpIntensity = 0, bool TrueRotation = false, bool HasFunny = false)
        {
            float c = (Main.GameUpdateCount / (60f + endPoints.X % 20)) % 4 - 2;
            float width = headTexture.Width;
            float length = (startingPos - endPoints).Length();
            float chainsPerUse = (width / length) * addonPerUse;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);
                    if (emitsDust)
                    {
                        if (Main.rand.Next(50) == 0)
                        {
                            if (!Main.tile[(int)x / 16, (int)y / 16].active())
                            {
                                Dust dust = Dust.NewDustPerfect(new Vector2(x, y), DustID.AmberBolt);
                                dust.fadeIn = 1f;
                                dust.scale = 0.1f;
                                dust.noGravity = true;
                                dust.velocity *= 0.25f;
                            }
                        }
                    }
                    bool ifBlack = Lighting.GetColor((int)(x / 16), (int)(y / 16)) == Color.Black;
                    float cDist = 0.5f - Math.Abs(i - c) * 2;
                    float cDist2 = 2 - Math.Abs(i - c) * 2;
                    if (cDist < 0)
                        cDist = 0;
                    if (cDist2 < 0)
                        cDist2 = 0;
                    distBetween = new Vector2(x -
                    X(i - chainsPerUse, startingPos.X, c1.X, endPoints.X),
                    y -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    if (HasFunny)
                    {
                        DrawAdditiveFunky(headTexture, new Vector2(x, y).ForDraw(), Color.White, 1.1f + cDist2, cDist2 / 6f, i * 1000);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        EEMod.SolidOutline.CurrentTechnique.Passes[0].Apply();
                        EEMod.SolidOutline.Parameters["alpha"].SetValue(cDist * 0.5f);
                        Main.spriteBatch.Draw(headTexture, new Vector2(x, y).ForDraw(),
                    headTexture.Bounds, ifBlack ? Color.Black : Color.White, TrueRotation ? 0 : projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), scale * (fadeScale ? (i + 0.5f) : 1) * 1.07f, SpriteEffects.None, 0);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                    Main.spriteBatch.Draw(headTexture, new Vector2(x, y).ForDraw(),
                    headTexture.Bounds, ifBlack ? Color.Black : Color.Lerp((alphaBlend ? Lighting.GetColor((int)(x / 16), (int)(y / 16)) : drawColor), Color.White, cDist * lerpIntensity), TrueRotation ? 0 : projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), scale * (fadeScale ? (i + 0.5f) : 1), SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
        }
        public static ParticleZone Particles => EEMod.Particles.Get("Main");
        public static void DrawParticlesAlongBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, float chainsPerUse, Color color, float spawnChance = 1f, params IParticleModule[] modules)
        {
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);

                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(spawnChance));
                    EEMod.Particles.Get("Main").SpawnParticles(new Vector2(x, y), default, 2, color, modules);
                }
            }
        }
        public static void DrawParticlesAlongLine(Vector2 endPoints, Vector2 startingPos, float chainsPerUse, Color color, float spawnChance = 1f, params IParticleModule[] modules)
        {
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                if (i != 0)
                {
                    Vector2 pos = Vector2.Lerp(startingPos, endPoints,i);

                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(spawnChance));
                    EEMod.Particles.Get("Main").SpawnParticles(pos, default, 2, color, modules);
                }
            }
        }
        public static void DrawParticlesAlongBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, float chainsPerUse, Color color, float spawnChance = 1f,Vector2 velocity = default, params IParticleModule[] modules)
        {
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);

                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(spawnChance));
                    EEMod.Particles.Get("Main").SpawnParticles(new Vector2(x, y), velocity, 2, color, modules);
                }
            }
        }
        public static void DrawChain(Texture2D tex, Vector2 p1, Vector2 p2, float rotOffset = 0)
        {
            //USE IN PROPER HOOK PLZ THX
            float width = tex.Width;
            float length = (p1 - p2).Length();
            float rotation = (p1 - p2).ToRotation();
            Rectangle rect = new Rectangle(0, 0, tex.Width, tex.Height);
            for (float i = 0; i < 1; i += width / length)
            {
                Vector2 lerp = p1 + (p2 - p1) * i;
                Main.spriteBatch.Draw(tex, lerp.ForDraw(), rect, Color.White, rotation + rotOffset, rect.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        public static void DrawChain(Texture2D tex, Vector2 p1, Vector2 p2, float rotOffset = 0,float per = 1)
        {
            //USE IN PROPER HOOK PLZ THX
            float width = tex.Width;
            float length = (p1 - p2).Length();
            float rotation = (p1 - p2).ToRotation();
            Rectangle rect = new Rectangle(0, 0, tex.Width, tex.Height);
            for (float i = 0; i < 1; i += (width / length)*per)
            {
                Vector2 lerp = p1 + (p2 - p1) * i;
                Main.spriteBatch.Draw(tex, lerp.ForDraw(), rect, Color.White, rotation + rotOffset, rect.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        public static Vector2 TraverseBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float t)
        {
            float x = X(t, startingPos.X, c1.X, c2.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, c2.Y, endPoints.Y);
            return new Vector2(x, y);
        }
        public static Vector2 TraverseBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, float t)
        {
            float x = X(t, startingPos.X, c1.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, endPoints.Y);
            return new Vector2(x, y);
        }
        public static void DrawBezier(SpriteBatch spriteBatch, Texture2D headTexture, string glowMaskTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, float rotDis, Rectangle source)
        {
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    distBetween = new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) -
                    X(i - chainsPerUse, startingPos.X, c1.X, c2.X, endPoints.X),
                    Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, c2.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    spriteBatch.Draw(headTexture, new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) - Main.screenPosition.X, Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) - Main.screenPosition.Y),
                    source, drawColor, projTrueRotation,
                    source.Size() / 2, 1, SpriteEffects.None, 0);
                }
            }
            //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }
        public static void DrawBezier(SpriteBatch spriteBatch, Texture2D headTexture, string glowMaskTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, float rotDis, int frame, int noOfFrames, int frameDiff)
        {
            int yeet = 0;
            for (float i = 0; i < 1; i += chainsPerUse)
            {
                yeet += frameDiff;
                Vector2 distBetween;
                float projTrueRotation;
                int frameHeight = headTexture.Height / noOfFrames;
                Rectangle frames;
                frames = new Rectangle(0, (int)((yeet + frame) % noOfFrames) * frameHeight, headTexture.Width, frameHeight);
                if (i != 0)
                {
                    distBetween = new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) -
                    X(i - chainsPerUse, startingPos.X, c1.X, c2.X, endPoints.X),
                    Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, c2.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    spriteBatch.Draw(headTexture, new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) - Main.screenPosition.X, Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) - Main.screenPosition.Y),
                    frames, drawColor, projTrueRotation,
                    frames.Size() / 2, 1, SpriteEffects.None, 0);
                }
            }
            //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }
        public static void DrawBezier(SpriteBatch spriteBatch, Texture2D headTexture, string glowMaskTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, float rotDis, Texture2D endingTexture)
        {
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    if (i >= 1 - chainsPerUse)
                    {
                        headTexture = endingTexture;
                    }
                    distBetween = new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) -
                    X(i - chainsPerUse, startingPos.X, c1.X, c2.X, endPoints.X),
                    Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, c2.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;
                    spriteBatch.Draw(headTexture, new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) - Main.screenPosition.X, Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) - Main.screenPosition.Y),
                    new Rectangle(0, 0, headTexture.Width, headTexture.Height), drawColor, projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), 0.8f + (1 - i) * .6f, SpriteEffects.None, 0);
                }
            }
            //spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }

        public static Rectangle[] ReturnPoints(Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, int chogsizeX, int chogsizeY, int accuracy)
        {
            Rectangle[] collision = new Rectangle[(int)(1 / (chainsPerUse * accuracy)) + 1]; //41
            int keeper = -2;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                keeper++;
                if (i != 0 && keeper % accuracy == 0)
                {
                    collision[keeper / accuracy] = new Rectangle((int)X(i, startingPos.X, c1.X, c2.X, endPoints.X) - chogsizeX / 2, (int)Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) - chogsizeY / 2, chogsizeX, chogsizeY);
                }
            }
            return collision;
        }

        private static int misckeep;

        public static void DrawBezierProj(Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, float rotDis, int projType, bool isBridge)
        {
            bool misc;
            misckeep = 0;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                misckeep++;
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    distBetween = new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) -
                    X(i - chainsPerUse, startingPos.X, c1.X, c2.X, endPoints.X),
                    Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, c2.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() + rotDis;
                    int proj = Projectile.NewProjectile(new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X), Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y)), Vector2.Zero, projType, 0, 0f, Main.myPlayer, 0, i);
                    Main.projectile[proj].rotation = projTrueRotation;
                    if (misckeep % 3 == 0)
                    {
                        misc = true;
                    }
                    else
                    {
                        misc = false;
                    }

                    if (isBridge)
                    {
                        Bridge bridge = (Bridge)Main.projectile[proj].modProjectile;
                        bridge.isSupport = misc;
                        bridge.chainsPerUse = chainsPerUse;
                        bridge.rotDis = rotDis;
                    }
                }
            }
            //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }

        public static void NewTextAutoSync(string text, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, color);
            }
            else
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
        }

        public static bool PlayerIsInForest(Player player)
        {
            return !player.ZoneJungle
                && !player.ZoneDungeon
                && !player.ZoneCorrupt
                && !player.ZoneCrimson
                && !player.ZoneHoly
                && !player.ZoneSnow
                && !player.ZoneUndergroundDesert
                && !player.ZoneGlowshroom
                && !player.ZoneMeteor
                && !player.ZoneBeach
                && player.ZoneOverworldHeight;
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint = default, float lineWidth = 1f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            Main.spriteBatch.Draw(Main.magicPixel, p1, new Rectangle(0, 0, 1, 1), tint == default ? Color.White : tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }
        public static void SpawnOre(int type, double frequency, float depth, float depthLimit)
        {
            int x = Main.maxTilesX;
            int y = Main.maxTilesY;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < (int)(x * y * frequency); k++)
                {
                    int tilesX = WorldGen.genRand.Next(0, x);
                    int tilesY = WorldGen.genRand.Next((int)(y * depth), (int)(y * depthLimit));
                    WorldGen.OreRunner(tilesX, tilesY, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(3, 8), (ushort)type);
                }
            }
        }

        /* public static class VanillasDoing
        {
            public static void DrawNPCVanilla()
            {
            }
        } */

        public static class Debug
        {
            public static void LogChat(string obj, [CallerFilePath] string callerName = "", [CallerLineNumber] int lineNumber = -1)
            {
                Main.NewText(obj + $"<{Path.GetDirectoryName(callerName).Split('\\').Last()}/{Path.GetFileName(callerName)}>" + $"({lineNumber})", 0, 255, 0);
            }

            public static void Log(object obj, [CallerFilePath] string callerName = "", [CallerLineNumber] int lineNumber = -1)
            {
                LogChat(obj.ToString(), callerName, lineNumber);
            }

            public static void LogError(string obj, [CallerFilePath] string callerName = "", [CallerLineNumber] int lineNumber = -1)
            {
                Main.NewText(obj + $"<{Path.GetDirectoryName(callerName).Split('\\').Last()}/{Path.GetFileName(callerName)}>" + $"({lineNumber})", 255, 0, 0);
            }

            public static void Message(string obj, Color color = default)
            {
                if (color == default)
                {
                    color = Color.White;
                }

                Main.NewText(obj, color);
            }
        }
    }
}
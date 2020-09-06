using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using EEMod.Tiles;
using EEMod.Extensions;

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

        private static float X(float t,
    float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow((1 - t), 3) +
                x1 * 3 * t * Math.Pow((1 - t), 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }
        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow((1 - t), 3) +
                 y1 * 3 * t * Math.Pow((1 - t), 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
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
                    if(emitsDust)
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
                    projTrueRotation = distBetween.ToRotation() - (float)Math.PI / 2 + rotDis;
                    spriteBatch.Draw(headTexture, new Vector2(x - Main.screenPosition.X, y - Main.screenPosition.Y),
                    new Rectangle(0, 0, headTexture.Width, headTexture.Height), alphaBlend ? Lighting.GetColor((int)(x / 16), (int)(y / 16)) : drawColor, projTrueRotation,
                    new Vector2(headTexture.Width * 0.5f, headTexture.Height * 0.5f), 1, SpriteEffects.None, 0);
                }
            }
            //  spriteBatch.Draw(neckTex2D, new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, drawColor, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mod.GetTexture(glowMaskTexture), new Vector2(head.Center.X - Main.screenPosition.X, head.Center.Y - Main.screenPosition.Y), head.frame, Color.White, head.rotation, new Vector2(36 * 0.5f, 32 * 0.5f), 1f, SpriteEffects.None, 0f);
        }

        public static void DrawChain(Texture2D tex, Vector2 p1, Vector2 p2, float accuracy)
        {
            //USE IN PROPER HOOK PLZ THX
           for(float i = 0; i<1; i += accuracy)
            {
                Vector2 lerp = p1 + (p2 - p1) * i;
                Main.spriteBatch.Draw(tex, lerp.ForDraw(), Color.White);
            }
        }
        public static Vector2 TraverseBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float t)
        {
            float x = X(t, startingPos.X, c1.X, c2.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, c2.Y, endPoints.Y);
            return new Vector2(x, y);
        }
        public static void DrawBezier(SpriteBatch spriteBatch, Texture2D headTexture, string glowMaskTexture, Color drawColor, Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float chainsPerUse, float rotDis, int dim)
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
                    projTrueRotation = distBetween.ToRotation() - (float)Math.PI / 2 + rotDis;
                    spriteBatch.Draw(headTexture, new Vector2(X(i, startingPos.X, c1.X, c2.X, endPoints.X) - Main.screenPosition.X, Y(i, startingPos.Y, c1.Y, c2.Y, endPoints.Y) - Main.screenPosition.Y),
                    new Rectangle(0, 0, dim, dim), drawColor, projTrueRotation,
                    new Vector2(dim * 0.5f, dim * 0.5f), 1, SpriteEffects.None, 0);
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
                    projTrueRotation = distBetween.ToRotation() - (float)Math.PI / 2 + rotDis;
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
        static int misckeep;
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
                        misc = true;
                    else
                        misc = false;
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
                Main.NewText(text, color);
            else
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
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

        public static void SpawnOre(int type, double frequency, float depth, float depthLimit)
        {
            int x = Main.maxTilesX;
            int y = Main.maxTilesY;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < (int)((double)(x * y) * frequency); k++)
                {
                    int tilesX = WorldGen.genRand.Next(0, x);
                    int tilesY = WorldGen.genRand.Next((int)(y * depth), (int)(y * depthLimit));
                    WorldGen.OreRunner(tilesX, tilesY, (double)WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(3, 8), (ushort)type);
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
                if (color == default) color = Color.White;
                Main.NewText(obj, color);
            }
        }
    }
}
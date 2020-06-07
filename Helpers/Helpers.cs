using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using InteritosMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InteritosMod
{
    public static partial class Helpers
    {
        internal const string EmptyTexture = "InteritosMod/Empty";
        // public static InteritosGlobalNPC Interitos(this NPC npc) => npc.GetGlobalNPC<InteritosGlobalNPC>();
        // public static InteritosGlobalProjectile Interitos(this Projectile proj) => proj.GetGlobalProjectile<InteritosGlobalProjectile>();

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
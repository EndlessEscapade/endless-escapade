using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Extensions;
using EEMod.Net;
using EEMod.Net.Serializers;

namespace EEMod
{
    public static class EEServerVariableCache
    {

        public static Vector2 VectorStorage;
        public static Vector2 PositionStorage;
        public static int Cool;

        public static void SyncVelocity(Vector2 v)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
               EENet.SendPacket(EEMessageType.SyncVector, v);
               VectorStorage = v;
            }
        }
        public static void SyncPosition(Vector2 v)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                    EENet.SendPacket(EEMessageType.SyncPosition, v);
                    PositionStorage = v;
            }
        }
        public static void SyncCoolDown(int v)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                    EENet.SendPacket(EEMessageType.SyncCool, v);
                    Cool = v;
            }
        }
        internal static void HandlePacket(BinaryReader reader, int fromwho, EEMessageType msg)
        {
            if (msg == EEMessageType.SyncVector)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.SyncVector, -1, fromwho, reader.Read<Vector2>());
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Vector2 v = reader.Read<Vector2>();
                    VectorStorage = v;
                }
            }
            if (msg == EEMessageType.SyncPosition)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.SyncPosition, -1, fromwho, reader.Read<Vector2>());
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Vector2 v = reader.Read<Vector2>();
                    PositionStorage = v;
                }
            }
            if (msg == EEMessageType.SyncCool)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.SyncCool, -1, fromwho, reader.Read<int>());
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    int v = reader.Read<int>();
                    Cool = v;
                }
            }
        }
    }
}

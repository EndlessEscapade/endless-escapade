﻿using System;
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
        public static Vector2[] OtherBoatPos = new Vector2[300];
        public static float[] OtherRot = new float[300];
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
        public static void SyncBoatPos(Vector2 v,float f)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncBoatPos, v,f);
                OtherBoatPos[Main.myPlayer] = v;
                OtherRot[Main.myPlayer] = f;
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
            else if (msg == EEMessageType.SyncPosition)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.SyncPosition, -1, fromwho, reader.Read<Vector2>());
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Vector2 v = reader.Read<Vector2>();
                    PositionStorage = v;
                }
            }
            else if (msg == EEMessageType.SyncCool)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.SyncCool, -1, fromwho, reader.Read<int>());
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    int v = reader.Read<int>();
                    Cool = v;
                }
            }
            else if (msg == EEMessageType.SyncBoatPos)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.SyncBoatPos, -1, fromwho, reader.Read<Vector2>(), reader.Read<float>(),(ushort)fromwho);
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Vector2 v = reader.Read<Vector2>();
                    float f = reader.Read<float>();
                    ushort from = reader.Read<ushort>();
                    OtherBoatPos[from] = v;
                    OtherRot[from] = f;
                }
            }
        }
    }
}
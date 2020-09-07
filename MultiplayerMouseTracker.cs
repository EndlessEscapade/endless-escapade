using EEMod.Net;
using EEMod.Net.Serializers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace EEMod
{
    public static class MultiplayerMouseTracker
    {
        public static Vector2[] Mouses = new Vector2[300];
        public static Vector2 VectorStorage;
        public static Vector2 PositionStorage;

        public static Vector2 GetMousePos(int playerWhoAmI) => Mouses[playerWhoAmI];

        public static void UpdateMyMouse()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Vector2 current = Main.MouseWorld;

                if (Mouses[Main.myPlayer] != current)
                {
                    EENet.SendPacket(EEMessageType.MouseCheck, current);

                    Mouses[Main.myPlayer] = current;
                }
            }
        }

        public static void SyncVector(Vector2 v)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncVector, v);

                VectorStorage = v;
            }
        }

        internal static void HandlePacket(BinaryReader reader, int fromwho, EEMessageType msg)
        {
            if (msg == EEMessageType.MouseCheck)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    EENet.SendPacketTo(EEMessageType.MouseCheck, -1, fromwho, reader.Read<Vector2>(), (ushort)fromwho);
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Mouses[reader.Read<ushort>()] = reader.Read<Vector2>();
                }
            }

            if (msg == EEMessageType.SyncVector)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    EENet.SendPacketTo(EEMessageType.SyncVector, -1, fromwho, reader.Read<Vector2>());
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    VectorStorage = reader.Read<Vector2>();
                }
            }
        }
    }
}
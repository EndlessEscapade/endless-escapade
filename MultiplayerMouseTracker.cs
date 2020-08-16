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
    public static class MultiplayerMouseTracker
    {
        public static Vector2[] Mouses = new Vector2[300];

        public static Vector2 velHolder;
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

        internal static void HandlePacket(BinaryReader reader, int fromwho)
        {
            if (Main.netMode == NetmodeID.Server)
                EENet.SendPacketTo(EEMessageType.MouseCheck, -1, fromwho, reader.Read<Vector2>(), (ushort)fromwho);
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Vector2 v = reader.Read<Vector2>();
                Mouses[reader.Read<ushort>()] = v;
            }
        }
    }
}

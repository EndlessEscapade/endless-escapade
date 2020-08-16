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

        internal static void HandlePacket(BinaryReader reader, int fromwho, EEMessageType msg)
        {
            if (msg == EEMessageType.MouseCheck)
            {
                if (Main.netMode == NetmodeID.Server)
                    EENet.SendPacketTo(EEMessageType.MouseCheck, -1, fromwho, reader.ReadVector2(), (ushort)fromwho);
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Vector2 v = reader.ReadVector2();
                    Mouses[reader.ReadUInt16()] = v;
                }
            }
            if (msg == EEMessageType.VelCheck)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    EENet.SendPacketTo(EEMessageType.VelCheck, -1, fromwho, reader.ReadVector2(), reader.ReadSingle());
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Vector2 v = reader.ReadVector2();
                    float vel = reader.ReadSingle();
                    Main.NewText(v * vel);
                    if (Main.dedServ)
                        Console.WriteLine($"{v * vel}");
                    velHolder = v * vel;
                }
                
            }
        }
    }
}

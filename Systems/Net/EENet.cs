using EEMod.Net.Serializers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EEMod.Net
{
    public static class EENet
    {
        public static void SendPacket(EEMessageType msg, params object[] data) => _SendPacket(msg, data: data);

        public static void SendPacketTo(EEMessageType msg, int toclient = -1, int ignoreclient = -1, params object[] data) => _SendPacket(msg, toclient, ignoreclient, data);

        private static void _SendPacket(EEMessageType msg, int toclient = -1, int ignoreclient = -1, params object[] data)
        {
            WriteToPacket(msg, data).Send(toclient, ignoreclient);
        }

        public static void SendMessageMultiPlayer(string yes, int exclude)
        {
            //NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(yes), new Color(0, 0, 0), exclude);
        }

        public static ModPacket WriteToPacket(EEMessageType msg, params object[] param)
        {
            ModPacket packet = ModContent.GetInstance<EEMod>().GetPacket();
            packet.Write((byte)msg);

            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                WriteObject(packet, obj);
            }
            return packet;
        }

        public static void ReceievePacket(BinaryReader reader, int fromwho)
        {
            EEMessageType msgtype = (EEMessageType)reader.ReadByte();
            switch (msgtype)
            {
                case EEMessageType.None: break;
                case EEMessageType.MouseCheck:
                    MultiplayerMouseTracker.HandlePacket(reader, fromwho, EEMessageType.MouseCheck);
                    break;

                case EEMessageType.SyncVector:
                case EEMessageType.SyncPosition:
                case EEMessageType.SyncBoatPos:
                    EEServerVariableCache.HandlePacket(reader, fromwho, msgtype);
                    break;

                case EEMessageType.SyncBrightness:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        SendPacketTo(EEMessageType.SyncBrightness, -1, fromwho, reader.Read<float>());
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        float v = reader.Read<float>();
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().brightness = v;
                    }
                    break;
            }
        }

        private static void WriteObject(ModPacket packet, object obj)
        {
            var serializer = SerializersManager.GetTypeSerializer(obj.GetType());
            if (serializer is null)
            {
                throw new ArgumentException($"The type of {nameof(obj)}, {obj.GetType().Name} could not be found");
            }

            serializer.WriteObj(packet, obj);
            //switch (obj)
            //{
            //    case byte n: packet.Write(n); break;
            //    case sbyte n: packet.Write(n); break;
            //    case short n: packet.Write(n); break;
            //    case ushort n: packet.Write(n); break;
            //    case int n: packet.Write(n); break;
            //    case uint n: packet.Write(n); break;
            //    case long n: packet.Write(n); break;
            //    case ulong n: packet.Write(n); break;
            //    case float n: packet.Write(n); break;
            //    case double n: packet.Write(n); break;
            //    case decimal n: packet.Write(n); break;
            //    case string n: packet.Write(n); break;
            //    case Vector2 n: packet.WriteVector2(n); break;
            //    case Enum n:
            //        switch (Type.GetTypeCode(Enum.GetUnderlyingType(n.GetType())))
            //        {
            //            case TypeCode.SByte: packet.Write(((IConvertible)n).ToSByte(null)); break;
            //            case TypeCode.Byte: packet.Write(((IConvertible)n).ToByte(null)); break;
            //            case TypeCode.Int16: packet.Write(((IConvertible)n).ToInt16(null)); break;
            //            case TypeCode.UInt16: packet.Write(((IConvertible)n).ToUInt16(null)); break;
            //            case TypeCode.Int32: packet.Write(((IConvertible)n).ToInt32(null)); break;
            //            case TypeCode.UInt32: packet.Write(((IConvertible)n).ToUInt32(null)); break;
            //            case TypeCode.Int64: packet.Write(((IConvertible)n).ToInt64(null)); break;
            //            case TypeCode.UInt64: packet.Write(((IConvertible)n).ToUInt64(null)); break;
            //        }
            //        break;
            //    case Array array:
            //        packet.Write(array.Length);
            //        foreach (object o in array)
            //            WriteObject(packet, o);
            //        break;
            //}
        }
    }
}
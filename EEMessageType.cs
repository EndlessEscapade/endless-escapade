

using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;

namespace EEMod
{
    public class EENet
    {
        public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);
            
            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                if (obj is bool) packet.Write((bool)obj);
                else
                if (obj is byte) packet.Write((byte)obj);
                else
                if (obj is int) packet.Write((int)obj);
                else
                if (obj is float) packet.Write((float)obj);
            }
            return packet;
        }
    }
}

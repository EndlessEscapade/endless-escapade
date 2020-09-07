using EEMod.Extensions;
using Terraria;
using Terraria.ID;

namespace EEMod
{
    public static partial class Helpers
    {
        public static bool AnyPlayerAlive
        {
            get
            {
                Player p;
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    p = Main.LocalPlayer;
                    return p.IsAlive();
                }
                for (int i = 0; i < Main.player.Length; i++)
                {
                    p = Main.player[i];
                    if (p.IsAlive())
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Looks for the first NPC in the <seealso cref="Main.npc"/> array that's alive and it's type is the one specified <paramref name="type"/>, starting from <paramref name="start"/>. <br />
        /// If no NPC is found, -1 is returned.
        /// </summary>
        /// <param name="type">The type of the NPC</param>
        /// <param name="start">From where the loop starts</param>
        /// <returns></returns>
        public static int FirstNPCIndex(int type, int start = 0)
        {
            int maxnpc = Main.npc.Length - 1; // Last element is broken
            for (int i = start; i < maxnpc; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.type == type)
                    return i;
            }
            return -1;
        }
    }
}
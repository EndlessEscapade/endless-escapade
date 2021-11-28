using EEMod.Seamap.SeamapAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEMod;
using Terraria.Audio;
using System.Diagnostics;

namespace EEMod.Seamap.SeamapContent
{
    public partial class Seamap
    {
        public static void UpdateSeamap()
        {
            for (int i = 0; i < SeamapObjects.SeamapEntities.Length; i++)
            {
                if (SeamapObjects.SeamapEntities[i] != null)
                {
                    SeamapObjects.SeamapEntities[i].Update();
                }
            }
        }
    }
}


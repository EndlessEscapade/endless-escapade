using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using EEMod.Autoloading;
using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using System.Diagnostics;

namespace EEMod.Seamap.SeamapContent
{
    internal static class SeamapObjects
    {
        public static SeamapObject[] SeamapEntities = new SeamapObject[2000];

        public static EEPlayerShip localship;

        public static void InitObjects(Vector2 shipPos)
        {
            SeamapEntities = new SeamapObject[2000];
            localship = new EEPlayerShip(shipPos, Vector2.Zero, Main.LocalPlayer);

            NewSeamapObject(localship);
        }

        public static void NewSeamapObject(SeamapObject obj)
        {
            for(int i = 0; i < SeamapEntities.Length; i++)
            {
                if(SeamapEntities[i] == null || !SeamapEntities[i].active)
                {
                    SeamapEntities[i] = obj;
                    SeamapEntities[i].whoAmI = i;
                    SeamapEntities[i].active = true;
                    SeamapEntities[i].OnSpawn();

                    break;
                }
            }
        }
    }
}

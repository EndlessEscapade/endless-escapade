using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using EEMod.Autoloading;
using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Seamap.SeamapContent
{
    internal static class SeamapObjects
    {
        [FieldInit] public static List<Island> IslandEntities = new List<Island>();

        [FieldInit] internal static List<ISeamapEntity> OceanMapElements = new List<ISeamapEntity>();

        [FieldInit] internal static SeamapObject[] SeamapEntities = new SeamapObject[400];

        public static EEPlayerShip localship;

        public static void InitObjects(Vector2 shipPos)
        {
            localship = new EEPlayerShip(shipPos, Vector2.Zero);

            SeamapEntities[0] = localship;
            SeamapEntities[0].whoAmI = 0;

            localship.OnSpawn();
        }

        public static void NewSeamapObject(SeamapObject obj)
        {
            for(int i = 0; i < SeamapEntities.Length; i++)
            {
                if(SeamapEntities[i] == null || !SeamapEntities[i].active)
                {
                    SeamapEntities[i] = obj;
                    SeamapEntities[i].whoAmI = i;
                    obj.OnSpawn();
                    break;
                }
            }
        }
    }
}

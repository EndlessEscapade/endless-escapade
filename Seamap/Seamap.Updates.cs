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

        public static void InitializeSeamap()
        {
            SeamapObjects.InitObjects(new Vector2(seamapWidth - 450, seamapWidth - 100));

            SeamapObjects.NewSeamapObject(new MainIsland(new Vector2(seamapWidth - 402, seamapHeight - 118)));

            for (int i = 0; i < 15; i++)
            {
                switch (Main.rand.Next(0, 6))
                {
                    case 0:
                        SeamapObjects.NewSeamapObject(new Rock1(new Vector2(seamapWidth - (i * 67), seamapHeight - 800)));
                        break;
                    case 1:
                        SeamapObjects.NewSeamapObject(new Rock2(new Vector2(seamapWidth - (i * 67), seamapHeight - 800)));
                        break;
                    case 2:
                        SeamapObjects.NewSeamapObject(new Rock3(new Vector2(seamapWidth - (i * 67), seamapHeight - 800)));
                        break;
                    case 3:
                        SeamapObjects.NewSeamapObject(new Rock4(new Vector2(seamapWidth - (i * 67), seamapHeight - 800)));
                        break;
                    case 4:
                        SeamapObjects.NewSeamapObject(new Rock5(new Vector2(seamapWidth - (i * 67), seamapHeight - 800)));
                        break;
                    case 5:
                        SeamapObjects.NewSeamapObject(new Rock6(new Vector2(seamapWidth - (i * 67), seamapHeight - 800)));
                        break;
                }
            }

            for (int i = 0; i < 15; i++)
            {
                switch (Main.rand.Next(0, 6))
                {
                    case 0:
                        SeamapObjects.NewSeamapObject(new Rock1(new Vector2(seamapWidth - 1000, seamapHeight - (i * 53))));
                        break;
                    case 1:
                        SeamapObjects.NewSeamapObject(new Rock2(new Vector2(seamapWidth - 1000, seamapHeight - (i * 53))));
                        break;
                    case 2:
                        SeamapObjects.NewSeamapObject(new Rock3(new Vector2(seamapWidth - 1000, seamapHeight - (i * 53))));
                        break;
                    case 3:
                        SeamapObjects.NewSeamapObject(new Rock4(new Vector2(seamapWidth - 1000, seamapHeight - (i * 53))));
                        break;
                    case 4:
                        SeamapObjects.NewSeamapObject(new Rock5(new Vector2(seamapWidth - 1000, seamapHeight - (i * 53))));
                        break;
                    case 5:
                        SeamapObjects.NewSeamapObject(new Rock6(new Vector2(seamapWidth - 1000, seamapHeight - (i * 53))));
                        break;
                }
            }
        }
    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.VerletIntegration
{
    public static class VerletHelpers
    {
        struct VineInfo
        {
            public Vector2 position;
            public int numberOfChains;
            public float lengthOfChains;
            public string glowmaskPath;
            public string texturePath;
            public Texture2D glowmask => ModContent.GetInstance<EEMod>().GetTexture(glowmaskPath);
            public Texture2D texture => ModContent.GetInstance<EEMod>().GetTexture(texturePath);
            public VineInfo(Vector2 position, int numberOfChains, float lengthOfChains, string texturePath, string glowmaskPath)
            {
                this.position = position;
                this.numberOfChains = numberOfChains;
                this.lengthOfChains = lengthOfChains;
                this.glowmaskPath = glowmaskPath;
                this.texturePath = texturePath;
            }
        }
        public static HashSet<int> EndPointChains = new HashSet<int>();
        public static IList<Vector2> SwingableVines = new List<Vector2>();

        public static void AddStickChain(ref Verlet verlet, Vector2 position, int numberOfChains, float lengthOfChains)
        {
            for (int i = 0; i < numberOfChains; i++)
            {
                EEMod eemood = ModContent.GetInstance<EEMod>();
                int a = verlet.CreateVerletPoint(position + new Vector2(0, lengthOfChains * i), i == 0 ? true : false);
                if (i == 0)
                {
                    SwingableVines.Add(position);
                }
                if (i > 1)
                {
                    int vineRand = Main.rand.Next(0, 7);
                    if (vineRand != 0 && vineRand != 3)
                        verlet.BindPoints(a, a - 1, true, default, eemood.GetTexture("Projectiles/Vines/Vine" + vineRand), eemood.GetTexture("Projectiles/Vines/Vine" + vineRand + "Glow"));
                    else
                        verlet.BindPoints(a, a - 1, true, default, eemood.GetTexture("Projectiles/Vines/Vine" + vineRand));
                }
                if (i == 1)
                {
                    int vineRand = Main.rand.Next(0, 3);
                    if (vineRand != 0)
                        verlet.BindPoints(a, a - 1, true, default, eemood.GetTexture("Projectiles/Vines/VineBase" + vineRand), eemood.GetTexture("Projectiles/Vines/VineBase" + vineRand + "Glow"));
                    else
                        verlet.BindPoints(a, a - 1, true, default, eemood.GetTexture("Projectiles/Vines/VineBase" + vineRand));
                }

                if (i == numberOfChains - 1)
                {
                    verlet.BindPoints(a, a - 1, true, default, eemood.GetTexture("Projectiles/Vines/Vine2"), eemood.GetTexture("Projectiles/Vines/Vine2Glow"));
                    EndPointChains.Add(a);
                }
            }
        }
        public static void LoadVines()
        {

        }

    }
}

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
            public Texture2D glowmask;
            public Texture2D texture;
            public VineInfo(Vector2 position, int numberOfChains, float lengthOfChains, Texture2D texture, Texture2D glowmask = null)
            {
                this.position = position;
                this.numberOfChains = numberOfChains;
                this.lengthOfChains = lengthOfChains;
                this.glowmask = glowmask;
                this.texture = texture;
            }
        }
        public static HashSet<int> EndPointChains = new HashSet<int>();
        public static List<Vector2> SwingableVines = new List<Vector2>();

        public static void AddStickChain(ref Verlet verlet,Vector2 position,int numberOfChains, float lengthOfChains)
        {
            for (int i = 0; i < numberOfChains; i++)
            {
                EEMod eemood = ModContent.GetInstance<EEMod>();
                int a = verlet.CreateVerletPoint(position + new Vector2(0, lengthOfChains * i), i == 0 ? true : false);
                if (i != numberOfChains - 1)
                {
                    if (i > 1)
                    {
                        int vineRand = Main.rand.Next(0, 7);
                        while (vineRand == 2)
                            vineRand = Main.rand.Next(0, 7);
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
                }
                else
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
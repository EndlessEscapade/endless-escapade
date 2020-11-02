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
            public VineInfo(Vector2 position, int numberOfChains, float lengthOfChains)
            {
                this.position = position;
                this.numberOfChains = numberOfChains;
                this.lengthOfChains = lengthOfChains;
            }
        }
        public static HashSet<int> EndPointChains = new HashSet<int>();
        public static List<Vector2> SwingableVines = new List<Vector2>();
        public static void AddStickChain(ref Verlet verlet,Vector2 position,int numberOfChains, float lengthOfChains)
        {
            for (int i = 0; i < numberOfChains; i++)
            { 
                int a = verlet.CreateVerletPoint(position + new Vector2(0, lengthOfChains * i), i == 0 ? true : false);
                if (i > 0)
                    verlet.BindPoints(a, a - 1,true,default, ModContent.GetInstance<EEMod>().GetTexture("Projectiles/Vine"));

                if(i == numberOfChains - 1)
                {
                    EndPointChains.Add(a);
                }
            }
        }
        public static void LoadVines()
        {

        }
         
    }
}
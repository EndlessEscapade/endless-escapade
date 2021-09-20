using EEMod.Extensions;
using EEMod.ID;
using EEMod.Systems;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class EmptyTileEntities : ModSystem
    {
        public static EmptyTileEntities Instance;
        internal Dictionary<Vector2, Vector2> EmptyTilePairsCache = new Dictionary<Vector2, Vector2>();
        internal Dictionary<Vector2, EmptyTileEntity> EmptyTileEntityPairsCache = new Dictionary<Vector2, EmptyTileEntity>();
        //Hash set cause I wanna be a douchebag :v
        internal HashSet<EmptyTileEntity> ETES = new HashSet<EmptyTileEntity>();

        public void AddPair(EmptyTileEntity ETE, Vector2 position, byte[,,] array)
        {
            ETES.Add(ETE);
            if (!EmptyTileEntityPairsCache.ContainsKey(position))
                EmptyTileEntityPairsCache.Add(position, ETE);

            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i, 0] == 1)
                    {
                        if (!EmptyTilePairsCache.ContainsKey(position + new Vector2(i, j)))
                            EmptyTilePairsCache.Add(position + new Vector2(i, j), position);
                    }
                }
            }

            EEWorld.EEWorld.CreateInvisibleTiles(array, position);
        }

        public void Remove(Vector2 position) => EmptyTileEntityPairsCache[Convert(position)].Destroy();

        public Vector2 Convert(Vector2 position) => EmptyTilePairsCache.TryGetValue(position, out var val) ? val : Vector2.Zero;
        
        public override void PostUpdateEverything()
        {
            if (Main.worldName == KeyID.CoralReefs)
            {
                foreach (EmptyTileEntity ETE in ETES) // List because if the collection is modified an exception will be thrown
                {
                    if (ETE != null)
                        ETE.Update();
                }
            }
        }

        public override void PostDrawTiles()
        {
            if (Main.worldName == KeyID.CoralReefs)
            {
                foreach (EmptyTileEntity ETE in ETES)
                {
                    if (ETE != null)
                        ETE.Draw();
                }
            }
        }

        public void Invoke(Vector2 position)
        {
            EmptyTileEntityPairsCache[Convert(position)].Activiate();
        }

        public override void Load()
        {
            ETES.Clear();
            List<EmptyTileEntity> emptyTileEntities = EmptyTileEntityPairsCache.Values.ToList();
            foreach(EmptyTileEntity ETEnt in emptyTileEntities)
            {
                ETES.Add(ETEnt);
            }
            Instance = this;
        }
    }
}
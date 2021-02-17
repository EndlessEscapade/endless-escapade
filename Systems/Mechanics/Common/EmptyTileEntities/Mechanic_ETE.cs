using EEMod.Extensions;
using EEMod.ID;
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
    public class EmptyTileEntities : Mechanic
    {
        public static EmptyTileEntities Instance;
        internal Dictionary<Vector2, Vector2> EmptyTilePairs = new Dictionary<Vector2, Vector2>();
        internal Dictionary<Vector2, EmptyTileDrawEntity> EmptyTileEntityPairs = new Dictionary<Vector2, EmptyTileDrawEntity>();

        public void AddPair(EmptyTileDrawEntity ETE, Vector2 position, byte[,,] array)
        {
            if (!EmptyTileEntityPairs.ContainsKey(position))
                EmptyTileEntityPairs.Add(position, ETE);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i, 0] == 1)
                    {
                        if (!EmptyTilePairs.ContainsKey(position + new Vector2(i, j)))
                            EmptyTilePairs.Add(position + new Vector2(i, j), position);
                    }
                }
            }
            EEWorld.EEWorld.CreateInvisibleTiles(array, position);
        }
        public void Remove(Vector2 position) =>
            EmptyTileEntityPairs[Convert(position)].Destroy();

        public Vector2 Convert(Vector2 position) => EmptyTilePairs.TryGetValue(position, out var val) ? val : Vector2.Zero;
        public override void OnUpdate()
        {
            if(Main.worldName == KeyID.CoralReefs)
            foreach (EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values.ToList()) // List because if the collection is modified an exception will be thrown
            {
                if (ETE != null)
                    ETE.Update();
            }
        }
        public override void OnDraw()
        {
            if(Main.worldName == KeyID.CoralReefs)
            foreach (EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values.ToList())
            {
                if (ETE != null)
                    ETE.Draw();
            }
        }
        public void Invoke(Vector2 position)
        {
            EmptyTileEntityPairs[Convert(position)].Activiate();
        }
        public override void OnLoad()
        {
            Instance = this;
        }
        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}
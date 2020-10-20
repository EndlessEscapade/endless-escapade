

using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using EEMod.Extensions;

namespace EEMod.Tiles.EmptyTileArrays
{
    public abstract class EmptyTileDrawEntity
    {
        public Vector2 position;
        public int activeTime;
        public float alpha = 1;
        public virtual int activityTime { get; set; }
        public virtual Texture2D tex { get; set; }
        public EmptyTileDrawEntity(Vector2 position)
        {
            this.position = position;
        }
        public void Activiate()
        {
            activeTime = activityTime;
        }
        public virtual void DuringActivation()
        {

        }
        public virtual void DuringNonActivation()
        {

        }

        public virtual void OnUpdate()
        {

        }
        public void Update()
        {
            OnUpdate();
            if (activeTime > 0)
            {
                DuringActivation();
                activeTime--; 
            }
            else
            {
                DuringNonActivation();
            }
        }
        public void Draw()
        {
            Main.spriteBatch.Draw(tex, (position*16).ForDraw(), Color.White * alpha);
        }
    }
    public static class EmptyTileEntityCache
    {
        static internal readonly Dictionary<Vector2, Vector2> EmptyTilePairs = new Dictionary<Vector2, Vector2>();
        static internal readonly Dictionary<Vector2, EmptyTileDrawEntity> EmptyTileEntityPairs = new Dictionary<Vector2, EmptyTileDrawEntity>();

        public static void AddPair(EmptyTileDrawEntity ETE, Vector2 position, byte[,] array)
        {
            EmptyTileEntityPairs.Add(position, ETE);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i] == 1)
                    {
                        EmptyTilePairs.Add(position + new Vector2(i, j), position);
                    }
                }
            }
            EEWorld.EEWorld.CreateInvisibleTiles(array, position);
        }
        public static Vector2 Convert(Vector2 position)
        {
            return EmptyTilePairs[position];
        }

        public static void Update()
        {
            foreach(EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values)
            {
                if(ETE != null)
                ETE.Update();
            }
        }
        public static void Draw()
        {
            foreach (EmptyTileDrawEntity ETE in EmptyTileEntityPairs.Values)
            {
                if (ETE != null)
                    ETE.Draw();
            }
        }
        public static void Invoke(Vector2 position)
        {
            EmptyTileEntityPairs[Convert(position)].Activiate();
        }
    }
    public class Crystal : EmptyTileDrawEntity
    {
        public Crystal(Vector2 position) : base(position)
        {
            this.position = position;
        }
        public override Texture2D tex => EEMod.instance.GetTexture("ShaderAssets/BulbousBall");
        public override int activityTime => 60;
        public override void DuringActivation()
        {
            alpha = 0;
        }
        public override void DuringNonActivation()
        {
            alpha = 1;
        }
    }
}

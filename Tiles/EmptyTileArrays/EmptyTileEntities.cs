

using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EEMod.Tiles.EmptyTileArrays
{
    public abstract class EmptyTileDrawEntity
    {
        public Vector2 position;
        public int activeTime;
        public float alpha = 1;
        public Color colour = Color.White;
        public int RENDERDISTANCE => 2000;
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
            if ((position * 16 - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                Main.spriteBatch.Draw(tex, (position*16).ForDraw(), colour * alpha);
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
        public static void Remove(Vector2 position)
        {
            EmptyTileEntityPairs.Remove(Convert(position));
            foreach (var item in EmptyTilePairs.Where(kvp => kvp.Value == Convert(position)).ToList())
            {
                WorldGen.KillTile((int)item.Key.X,(int)item.Key.Y);
                EmptyTilePairs.Remove(item.Key);
            }
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
        public override Texture2D tex => EEMod.instance.GetTexture("Tiles/EmptyTileArrays/CoralCrystal");
        public override int activityTime => 120;
        public override void DuringActivation()
        {
            colour = Color.Lerp(Color.White, Color.LightBlue, (float)Math.Sin((Math.PI / (float)activityTime) * activeTime)*5);
        }
        public override void DuringNonActivation()
        {
            colour = Color.White;
        }
    }
}

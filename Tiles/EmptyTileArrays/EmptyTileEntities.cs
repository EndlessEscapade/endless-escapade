

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
        public float rotation;
        public Vector2 ScreenPosition => position * 16;
        public int RENDERDISTANCE => 2000;
        public virtual int activityTime { get; set; }
        public virtual Texture2D tex { get; set; }

        public bool CanActivate { get; set; }
        public EmptyTileDrawEntity(Vector2 position, Texture2D texture)
        {
            this.position = position;
            tex = texture;
        }
        public void Activiate()
        {
            if(CanActivate)
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
                CanActivate = false;
                DuringActivation();
                activeTime--; 
            }
            else
            {
                CanActivate = true;
                DuringNonActivation();
            }
        }
        public void Draw()
        {
            if ((position * 16 - Main.LocalPlayer.Center).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                Main.spriteBatch.Draw(tex, (position*16).ForDraw() + new Vector2(0,tex.Height),new Rectangle(0,0,tex.Width,tex.Height), colour * alpha, rotation, new Vector2(0,tex.Height),1f,SpriteEffects.None,0f);
        }
    }
    public static class EmptyTileEntityCache
    {
        static internal readonly Dictionary<Vector2, Vector2> EmptyTilePairs = new Dictionary<Vector2, Vector2>();
        static internal readonly Dictionary<Vector2, EmptyTileDrawEntity> EmptyTileEntityPairs = new Dictionary<Vector2, EmptyTileDrawEntity>();

        public static void AddPair(EmptyTileDrawEntity ETE, Vector2 position, byte[,,] array)
        {
            if(!EmptyTileEntityPairs.ContainsKey(position))
            EmptyTileEntityPairs.Add(position, ETE);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i,0] == 1)
                    {
                        if (!EmptyTilePairs.ContainsKey(position + new Vector2(i, j)))
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
                //WorldGen.KillTile((int)item.Key.X,(int)item.Key.Y);
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
        public Crystal(Vector2 position, Texture2D texture) : base(position, texture)
        {
            this.position = position;
            tex = texture;
        }
        public override int activityTime => 20;
        public override void DuringActivation()
        {
            colour = Color.Lerp(Lighting.GetColor((int)position.X, (int)position.Y), Color.LightBlue, (float)Math.Sin((Math.PI / (float)activityTime) * activeTime));
            rotation = Main.rand.NextFloat(-.01f, .01f);
        }
        public override void DuringNonActivation()
        {
            Vector2 rand = new Vector2(Main.rand.NextFloat(ScreenPosition.X, ScreenPosition.X + tex.Width), Main.rand.NextFloat(ScreenPosition.Y, ScreenPosition.Y + tex.Height));
            EEMod.Particles.Get("Main").AddModule(new SlowDown(0.92f));
            EEMod.Particles.Get("Main").AddModule(new RotateVelocity(Main.rand.NextFloat(-0.1f, 0.1f)));
            EEMod.Particles.Get("Main").AddSpawningModule(new SpawnRandomly(0.01f));
            EEMod.Particles.Get("Main").SpawnParticles(rand,new Vector2(Main.rand.NextFloat(-1f,1f), Main.rand.NextFloat(-1f,1f)), 2,Color.LightBlue);
            rotation = 0;
            colour = Lighting.GetColor((int)position.X, (int)position.Y);
        }
    }
}

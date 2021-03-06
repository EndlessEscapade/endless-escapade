using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class AdditiveCalls : Mechanic
    {
        public List<IDrawAdditive> Additives = new List<IDrawAdditive>();

        public static AdditiveCalls Instance;

        internal void LoadObject(IDrawAdditive IDA)
        =>
            Additives.Add(IDA);
        internal void DisposeObject(IDrawAdditive IDA)
        =>
            Additives.Remove(IDA);

        public override void OnDraw()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                BlendState.Additive, null, null, null, null, 
                Main.GameViewMatrix.TransformationMatrix);
            foreach (IDrawAdditive IDA in Additives)
            {
                IDA.AdditiveCall(Main.spriteBatch);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                null, null, null, null, null, 
                Main.GameViewMatrix.TransformationMatrix);

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLoad()
        {
            Instance = this;
            base.OnLoad();
        }
        protected override Layer DrawLayering => Layer.AboveTiles;
    }
}
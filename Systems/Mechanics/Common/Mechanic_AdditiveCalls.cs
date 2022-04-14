using EEMod.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class AdditiveCalls : ModSystem
    {
        public List<IDrawAdditive> Additives = new List<IDrawAdditive>();

        public static AdditiveCalls Instance;

        internal void LoadObject(IDrawAdditive IDA)
        =>
            Additives.Add(IDA);
        internal void DisposeObject(IDrawAdditive IDA)

        =>
            Additives.Remove(IDA);

        public override void PostDrawTiles()
        {
            foreach (IDrawAdditive IDA in Additives)
            {
                IDA.AdditiveCall(Main.spriteBatch);
            }
        }

        public override void Load()
        {
            Instance = this;
            base.Load();
        }
    }
}
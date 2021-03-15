using EEMod.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class MechanicStructure : Mechanic
    {
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }
        protected override Layer DrawLayering => base.DrawLayering;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.ID;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ID;
using EEMod.Seamap.SeamapAssets;
using System.Diagnostics;

namespace EEMod.Seamap.SeamapContent
{
    public class Seagull : SeamapObject
    {
        public Seagull(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 24;
            height = 22;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Seagull").Value;
        }

        public override void Update()
        {
            velocity = new Vector2(0, 2);

            base.Update();
        }
    }
}

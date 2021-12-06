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
    public class FriendlyCannonball : SeamapObject
    {
        public FriendlyCannonball(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 12;
            height = 12;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/FriendlyCannonball").Value;
        }

        public int ticks;
        public override void Update()
        {
            if(++ticks > 120)
            {
                Kill();
            }

            base.Update();
        }
    }
}

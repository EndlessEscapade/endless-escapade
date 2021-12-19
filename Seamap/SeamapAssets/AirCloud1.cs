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
using EEMod.Extensions;

namespace EEMod.Seamap.SeamapContent
{
    public class AirCloud1 : SeamapObject
    {
        public AirCloud1(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 100;
            height = 48;

            alpha = 0.5f;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/AirCloud1").Value;
        }

        public override void Update()
        {
            velocity = new Vector2(-0.25f, 0);

            base.Update();
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            return true;
        }
    }
}

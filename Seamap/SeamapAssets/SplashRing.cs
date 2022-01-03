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
using ReLogic.Content;

namespace EEMod.Seamap.SeamapContent
{
    public class SplashRing : SeamapObject
    {
        public SplashRing(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 0;
            height = 0;

            alpha = 0.75f;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/SplashRing", AssetRequestMode.ImmediateLoad).Value;
        }

        public override void Update()
        {
            base.Update();
        }

        public int age;

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            scale = Math.Sin(((age * 3.14f) / 20)).PositiveSin();

            if (age >= 20) alpha -= 0.075f;

            if (age >= 30) Kill();

            Main.spriteBatch.Draw(texture, position.ForDraw(), null, new Color(78, 168, 236) * alpha, 0, new Vector2(19 / 2f, 9 / 2f), scale, SpriteEffects.None, 0f);

            age++;

            return false;
        }
    }
}

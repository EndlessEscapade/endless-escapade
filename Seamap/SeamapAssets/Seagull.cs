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
    public class Seagull : SeamapObject
    {
        public Seagull(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 24;
            height = 22;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/Seagull", AssetRequestMode.ImmediateLoad).Value;
        }

        public override void Update()
        {
            velocity = new Vector2(0, 0.5f);

            base.Update();
        }

        public int frameCounter = 0;
        public int frame = 0;
        public int age;

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch)
        {
            if (frameCounter++ >= 4)
            {
                frame++;
                if (frame > 8) frame = 0;

                frameCounter = 0;
            }

            age++;

            if (position.Y >= 4450) alpha -= 0.05f;

            Rectangle rect = new Rectangle(0, frame * height, 24, 22);
            Color drawColour2 = Color.White * alpha;

            //drawColour2.A = 255;
            Main.spriteBatch.Draw(texture, position.ForDraw(), rect, drawColour2.LightSeamap(), 0, rect.Size() / 2, scale, SpriteEffects.None, 0f);

            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            Color drawColour = Color.Black * alpha * 0.4f;

            Main.spriteBatch.Draw(texture, position.ForDraw() + new Vector2(0, 40), rect, drawColour, 0, rect.Size() / 2, scale, SpriteEffects.None, 0f);

            base.PostDraw(spriteBatch);
        }
    }
}

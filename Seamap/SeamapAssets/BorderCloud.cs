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
    public class BorderCloud : SeamapObject
    {
        public BorderCloud(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 148;
            height = 62;

            alpha = 1f;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/BorderCloud", AssetRequestMode.ImmediateLoad).Value;
        }

        public float lerpToBlack;
        public Vector2 origPos;

        public override void Update()
        {
            if (ai[0] == 0)
            {
                origPos = Center;
                ai[0] = 1;
                scale = Main.rand.NextFloat(0.75f, 1.1f);
            }

            velocity = new Vector2((float)Math.Sin((Main.GameUpdateCount / 90f) + origPos.X - origPos.Y) * 0.1f, 0);

            base.Update();
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            //Texture2D cloudSprite = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/BorderCloudSlate").Value;

            //lerpToBlack = (4500 - Center.Y) / 120f;
        
            /*for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                spriteBatch.Draw(cloudSprite, Center - Main.screenPosition + offsetPositon, null, Color.Lerp(new Color(49, 92, 140) * 0.1f, new Color(49, 92, 140), lerpToBlack), 0, new Rectangle(0, 0, width, height).Size() / 2, 1, SpriteEffects.None, 0);
            }*/

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch)
        {
            Texture2D cloudSprite = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/BorderCloud").Value;

            spriteBatch.Draw(cloudSprite, Center - Main.screenPosition, null, Color.Lerp(Color.White.LightSeamap(), Color.LightGray.LightSeamap() * 0.1f, lerpToBlack), 0, new Rectangle(0, 0, width, height).Size() / 2, 1, SpriteEffects.None, 0);
        }

        //public override bool PreDraw(SpriteBatch spriteBatch)
        //{
        //Main.NewText(position);

        //Main.spriteBatch.Draw(texture, position.ForDraw(), null, Color.White * alpha, 0, texture.TextureCenter(), scale, SpriteEffects.None, 0f);

        //return true;
        //}
    }
}

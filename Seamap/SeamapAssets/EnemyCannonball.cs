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
    public class EnemyCannonball : SeamapObject
    {
        public EnemyCannonball(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 12;
            height = 12;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/EnemyCannonball").Value;
        }

        public int ticks;
        public override void Update()
        {
            ticks++;

            if(sinkLevel >= 12)
            {
                SeamapObjects.NewSeamapObject(new SplashRing(Center + new Vector2(0, 4), Vector2.Zero));

                Kill();
            }

            base.Update();
        }

        public float sinkLevel;
        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            if(ticks >= 108)
            {
                sinkLevel += 1f;

                velocity = Vector2.Zero;

                Main.spriteBatch.Draw(texture, position.ForDraw() + new Vector2(0, sinkLevel), new Rectangle(0, 0, width, (int)(height - sinkLevel)), color * alpha, rotation, texture.Bounds.Size() / 2, scale, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

                return false;
            } 
            else
            {
                return true;
            }
        }
    }
}

using System;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Seamap.SeamapContent
{
    public class MovingCloud : ISeamapEntity
    {
        public Vector2 pos;
        public float flash;

        public int posXToScreen
        {
            get => (int)(pos.X + Main.screenPosition.X);
        }

        public int posYToScreen
        {
            get => (int)(pos.Y + Main.screenPosition.Y);
        }

        public Texture2D texture;
        public float scale, alpha;
        private readonly EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

        public MovingCloud(Vector2 pos, Texture2D tex, float scale, float alpha)
        {
            this.pos = pos;
            texture = tex;
            this.scale = scale;
            this.alpha = alpha;
            flash = Main.rand.NextFloat(0, 4);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            flash += 0.003f;
            Vector2 p = new Vector2(pos.X + (float)Math.Sin(flash) * 10, pos.Y - 1000).ForDraw();
            Color drawcolor = Lighting.GetColor(posXToScreen / 16, (posYToScreen - 1000) / 16) * modPlayer.brightness * (modPlayer.isStorming ? 2 / 3f : 1);
            drawcolor.A = (byte)alpha;
            spriteBatch.Draw(texture, p, null, drawcolor * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, default, scale, SpriteEffects.None, 0f);
        }

        public void Update()
        {

        }
    }
}

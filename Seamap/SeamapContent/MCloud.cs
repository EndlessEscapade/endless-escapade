using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Seamap.SeamapContent
{
    public class MCloud : ISeamapEntity
    {
        private Vector2 position;
        private readonly int width, height;
        private readonly float alpha, scale;
        private readonly Texture2D texture;
        private readonly EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
        private Vector2 Center => new Vector2(position.X + width / 2f, position.Y + height / 2f);

        public MCloud(Texture2D texture, Vector2 position, int width, int height, float scale, float alpha)
        {
            //scale = projectile.ai[0];
            //alpha = (int)projectile.ai[1];
            this.scale = scale;
            this.alpha = alpha;
            this.texture = texture;
            this.position = position;
            this.width = width;
            this.height = height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 newPos = Center;
            Rectangle rect = new Rectangle(0, 0, width, height);
            Color lightColour = Lighting.GetColor((int)newPos.X / 16, (int)newPos.Y / 16) * modPlayer.brightness * (modPlayer.isStorming ? 2 / 3f : 1);
            spriteBatch.Draw(texture, Center.ForDraw(), rect, lightColour * (alpha / 255f) * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, rect.Size() / 2, scale, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            position.X -= 0.3f;
        }
    }
}

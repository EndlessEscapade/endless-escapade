using System;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Seamap.SeamapContent
{
    //public struct SeaEntity
    //{
    //    EEPlayer player => Main.LocalPlayer.GetModPlayer<EEPlayer>();
    //    public SeaEntity(Vector2 pos, Texture2D tex, string NameOfIsland, int frameCount = 1, int frameSpid = 2, bool canCollide = false, int startingFrame = 0)
    //    {
    //        posX = (int)pos.X;
    //        posY = (int)pos.Y;
    //        texture = tex;
    //        frames = frameCount;
    //        frameSpeed = frameSpid;
    //        currentFrame = startingFrame;
    //        this.canCollide = canCollide;
    //        if (NameOfIsland != null)
    //        {
    //            if (!SeamapObjects.Islands.ContainsKey(NameOfIsland))
    //            {
    //                SeamapObjects.Islands.Add(NameOfIsland, this);
    //            }
    //        }
    //    }

    //    private readonly int posX;
    //    private readonly int posY;
    //    public int frames;
    //    public int currentFrame;
    //    public int frameSpeed;
    //    public bool canCollide;

    //    public int posXToScreen
    //    {
    //        get => posX + (int)Main.screenPosition.X + Main.screenWidth;
    //    }

    //    public int posYToScreen
    //    {
    //        get => posY + (int)Main.screenPosition.Y + Main.screenHeight;
    //    }

    //    public Texture2D texture;
    //    public Vector2 posToScreen => new Vector2(posXToScreen - texture.Width / 2, posYToScreen - texture.Height / (2 * frames));
    //    public Rectangle hitBox => new Rectangle((int)posToScreen.X, (int)posToScreen.Y - texture.Height / (frames * 2), texture.Width, texture.Height / (frames));
    //    private Rectangle ShipHitBox => new Rectangle((int)Main.screenPosition.X + (int)SeamapPlayerShip.localship.position.X - 30, (int)Main.screenPosition.Y + (int)SeamapPlayerShip.localship.position.Y - 30, 30, 30);
    //    public bool isColliding => hitBox.Intersects(ShipHitBox) && canCollide;

    //    public void Update()
    //    {

    //    }
    //}

    public class DarkCloud : ISeamapEntity
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

        public DarkCloud(Vector2 pos, Texture2D tex, float scale, float alpha)
        {
            flash += 0.01f;
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
            Color drawcolor = Lighting.GetColor(posXToScreen / 16, (posYToScreen - 1000) / 16) * modPlayer.seamapLightColor;
            drawcolor.A = (byte)alpha;
            if (modPlayer.quickOpeningFloat > 0.01f)
            {
                float lerp = 1 - (modPlayer.quickOpeningFloat / 10f);
                spriteBatch.Draw(texture, p, null, drawcolor * lerp, 0f, default, scale, SpriteEffects.None, 0f);
                return;
            }
            spriteBatch.Draw(texture, p, null, drawcolor * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, default, scale, SpriteEffects.None, 0f);
        }

        public void Update()
        {

        }
    }
}

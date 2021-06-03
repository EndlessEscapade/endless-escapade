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

namespace EEMod.Seamap.SeamapContent
{
    public class SeamapPlayerShip
    {
        public static SeamapPlayerShip localship = new SeamapPlayerShip();

        public static readonly Vector2 start = new Vector2(1700, 900);
        public float[] anchorLerp = new float[12];
        public Texture2D texture;
        public Rectangle frame;
        public int frames;
        public static float ShipHelthMax = 7;
        public float shipHelth = 7;
        public Vector2 position;
        public int width = 44;
        public int height = 52;
        public Vector2 velocity;
        public int cannonDelay = 60;
        public Vector2 otherBoatPos;
        public Vector2 currentLightningPos;
        public float intenstityLightning;

        public float flash = 0;
        public float markerPlacer = 0;

        public Rectangle rect => new Rectangle((int)position.X, (int)position.Y, width, height);

        public Vector2 Center => new Vector2((int)position.X + (width / 2), (int)position.Y + (height / 2));

        public void ModifyScreenPosition(ref Vector2 position)
        {
            position = this.position - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
        }

        #region Drawing "Disembark" text
        internal void DrawSubText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.subTextAlpha;
            Color color = Color.White;
            if (Main.worldName == KeyID.Sea)
            {
                string text = "Disembark?";
                color *= alpha;
                Vector2 textSize = Main.fontMouseText.MeasureString(text);
                float textPositionLeft = position.X - textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(textPositionLeft, position.Y + 20) - Main.screenPosition, color * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
        #endregion
    }
}

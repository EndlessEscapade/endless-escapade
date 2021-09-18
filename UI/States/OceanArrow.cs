using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.EEWorld;
using EEMod.Tiles.Furniture;

namespace EEMod.UI.States
{
    public class ArrowsUIState : UIState
    {
        public static bool OceanArrowVisible { get; set; }
        public static bool DesertArrowVisible { get; set; }
        Texture2D arrowUp;
        Texture2D arrowLeft;
        Arrow oceanArrow;
        Arrow desArrow;
        public ArrowsUIState()
        {

        }
        public override void OnInitialize()
        {
            base.OnInitialize();
            arrowLeft = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("UI/ArrowLeft").Value;
            arrowUp = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("UI/ArrowUp").Value;
            oceanArrow.width = 36;
            oceanArrow.height = 46;
            oceanArrow.alpha = 255;
            desArrow.width = 22;
            desArrow.height = 34;
            desArrow.alpha = 255;
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.gameMenu)
                return;
            base.Update(gameTime);
            if (Main.gamePaused || Main.gameInactive)
                return;
            UpdateOceanArrow();
            UpdateDesertArrow();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            base.DrawSelf(spriteBatch);
            DrawOceanArrow(spriteBatch);
            DrawDesertArrow(spriteBatch);
        }
        private void UpdateOceanArrow()
        {
            oceanArrow.time += 0.1f;
            Player p = Main.LocalPlayer;
            oceanArrow.position.X = p.Center.X - oceanArrow.width / 2 + (float)Math.Sin(oceanArrow.time) * 10 - 75;
            oceanArrow.position.Y = p.Center.Y - oceanArrow.height / 3;
            if (!OceanArrowVisible)
            {
                oceanArrow.alpha += 5;
            }
            else
            {
                oceanArrow.alpha -= 5;
            }

            Helpers.Clamp(ref oceanArrow.alpha, 0, 255);
        }
        private void DrawOceanArrow(SpriteBatch spriteBatch)
        {
            Vector2 center = oceanArrow.position;
            float colorlerp = (float)Helpers.SmoothStepValueAsLerp(Helpers.SmoothStepValueAsLerp((255f - oceanArrow.alpha) / 255f));
            spriteBatch.Draw(arrowLeft, center - Main.screenPosition, null, Color.Cyan * colorlerp, 0, default, 1f, SpriteEffects.None, 0f);
        }
        private void UpdateDesertArrow()
        {
            desArrow.time += 0.1f;
            Player ownerplayer = Main.LocalPlayer;
            desArrow.width = 22;
            desArrow.height = 34;
            desArrow.position.X = ownerplayer.position.X - desArrow.width / 3;
            desArrow.position.Y = ownerplayer.Center.Y - 100 + (float)Math.Sin(desArrow.time) * 10;
            if (!DesertArrowVisible)
            {
                desArrow.alpha += 5;
            }
            else
            {
                desArrow.alpha -= 5;
            }
            Helpers.Clamp(ref desArrow.alpha, 0, 255);
        }
        private void DrawDesertArrow(SpriteBatch spriteBatch)
        {
            Vector2 center = desArrow.position;
            float lerp = (255f - desArrow.alpha) / 255f;
            spriteBatch.Draw(arrowUp, center - Main.screenPosition, null, Color.Yellow * lerp, 0, default, 1f, SpriteEffects.None, 0f);
        }
        struct Arrow
        {
            public Vector2 position;
            public float time;
            public int width;
            public int height;
            public int alpha;
            //public bool visible;
        }
    }
}

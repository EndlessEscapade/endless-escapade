using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using EEMod.Items.Armor.Kelpweaver;

namespace EEMod.UI.States
{
    public class IndicatorsUI : UIState
    {
        public Arrow KelpArmorAmmoArrow;
        public bool MovingToRight;
        public int MoveTimer = 0;
        public bool ShouldFadeOut;
        public override void OnInitialize()
        {
            KelpArmorAmmoArrow = new Arrow();
            Append(KelpArmorAmmoArrow);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KelpArmorAmmoArrow.Top.Set(117.5f, 0f);
            if (MoveTimer < 50)
            {
                KelpArmorAmmoArrow.Left.Set(MathHelper.SmoothStep(MovingToRight ? 620f : 645f, MovingToRight ? 645f : 620f, MoveTimer / 49f), 0f);
                MoveTimer++;
            }
            else
            {
                MovingToRight = !MovingToRight;
                MoveTimer = 0;
            }
            if (KelpArmorAmmoArrow.Transparency < 0.85f && !ShouldFadeOut)
            {
                KelpArmorAmmoArrow.Transparency += 0.05f;
            }
            if (ShouldFadeOut)
            {
                if (KelpArmorAmmoArrow.Transparency <= 0f)
                {
                    Main.LocalPlayer.GetModPlayer<KelpweaverPlayer>().HasInteractedWithSlotBefore = true;
                    EEMod.UI.RemoveState("IndicatorsInterface");
                }
                KelpArmorAmmoArrow.Transparency -= 0.05f;
            }
            if (KelpArmorAmmoUI.Slot.IsMouseHovering)
            {
                ShouldFadeOut = true;
            }
        }
    }
    public class Arrow : UIElement
    {
        public Texture2D Texture;
        public float Transparency;
        public Arrow() : base() 
        {
            Texture = ModContent.Request<Texture2D>("EEMod/UI/ArrowLeft").Value;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            int x = (int)(dimensions.X + (Texture.Size().X / 2));
            int y = (int)(dimensions.Y + (Texture.Size().Y / 2));
            spriteBatch.Draw(Texture, new Vector2(x, y), null, Color.LawnGreen * Transparency, 0f, Texture.Size(), 0.75f, SpriteEffects.None, 0f);
        }
    }
}
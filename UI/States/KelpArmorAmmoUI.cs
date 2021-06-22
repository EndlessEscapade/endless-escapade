using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;
using CustomSlot;
using Terraria;
using Terraria.ID;

namespace EEMod.UI.States
{
	public class KelpArmorAmmoUI : UIState
	{
		public static CustomItemSlot Slot;
        public override void OnInitialize()
		{
            CroppedTexture2D emptyTexture = new CroppedTexture2D(ModContent.GetTexture("EEMod/icon")); //placeholder

            Slot = new CustomItemSlot(0, 0.6f)
            {
                IsValidItem = item => item.ammo == AmmoID.Bullet,
                EmptyTexture = emptyTexture,
                ActualColor = Color.LawnGreen * 0.85f,
                HoverText = "Kelp Revolvers' Ammo"
            };
            Append(Slot);
        }
		public override void Update(GameTime gameTime)
		{
            base.Update(gameTime);
            Slot.Left.Set(571.5f, 0f);
            Slot.Top.Set(105f, 0f);
            Recalculate();
        }
    }
}
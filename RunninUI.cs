using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace EEMod
{
	internal class RunninUI : UIState
	{
		/*
		public new float HAlign;
		public new float VAlign;
		UIText text = new UIText("");
		public override void OnInitialize()
		{
			UIPanel panel = new UIPanel();
			panel.Height.Set(50, 0);
			panel.Width.Set(100, 0);
			panel.HAlign = 0.5f;
			panel.VAlign = 0.01f;

			panel.Top.Set(0f, 0f);
			//panel.BackgroundColor = new Color(255, 255, 255, 0);

			text.HAlign = 0.5f;
			text.VAlign = 0.5f;
			text.Height.Set(400, 0);
			text.Width.Set(900, 0);
			panel.Append(text);
			Append(panel);
		}
		public override void Update(GameTime gameTime)
		{
			EEPlayer playerr = Main.LocalPlayer.GetModPlayer<EEPlayer>();
			var other = playerr.Hours;
			var other1 = playerr.Minutes;
			var other2 = playerr.Seconds;
			var other3 = playerr.Milliseconds;
			text.SetText(playerr.Hours + ":" + playerr.Minutes + ":" + playerr.Seconds + "." + playerr.Milliseconds) ;
		}
		*/
	}
}
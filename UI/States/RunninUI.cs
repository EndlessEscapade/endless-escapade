using EEMod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace EEMod.UI.States
{
    internal class RunninUI : UIState
    {
        private readonly UIText text = new UIText("");

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
            var modplayer = Main.LocalPlayer.GetModPlayer<SpeedrunTimerPlayer>();
            text.SetText(modplayer.hours + ":" + modplayer.minutes + ":" + modplayer.seconds + "." + modplayer.milliseconds);
        }
    }
}
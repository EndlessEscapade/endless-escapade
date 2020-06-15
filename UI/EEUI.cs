using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;

namespace EEMod.UI
{
    internal class EEUI : UIState
    {
        public override void OnActivate()
        {

        }
        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();
            panel.Width.Set(300, 0);
            panel.Height.Set(300, 0);
            Append(panel);

            UIText text = new UIText("Hello world!");
            panel.Append(text);
        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
        }


        public override void Update(GameTime gameTime)
        {

        }
    }
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace Prophecy.UI
{
     internal class EEUI : UIState
     {
       /* public static bool visible;
        public DragableUIPanel panel;
        public float oldScale;
        UIText text2 = new UIText("Click me!")*/

        public override void OnActivate()
        {
            base.OnActivate();
            UIPanel b = new DragableUIPanel();
            b.Width.Set(300, 0);
            b.Height.Set(300, 0);
            Append(b);
        }
        /*public override void OnInitialize()
          {
            // if you set this to true, it will show up in game
           visible = true;

           panel = new DragableUIPanel(); //initialize the panel
           // ignore these extra 0s
           panel.Left.Set(800, 0); //this makes the distance between the left of the screen and the left of the panel 500 pixels (somewhere by the middle)
           panel.Top.Set(100, 0); //this is the distance between the top of the screen and the top of the panel
           panel.Width.Set(100, 0);
           panel.Height.Set(100, 0);

           Append(panel); //appends the panel to the UIState

            UIText text = new UIText("Hello world!"); // 1
            panel.Append(text);

            UIPanel button = new UIPanel(); // 1
            button.Width.Set(100, 0);
            button.Height.Set(50, 0);
            button.HAlign = 0.5f;
            button.Top.Set(25, 0); // 2
            button.OnClick += OnButtonClick; // 3
            panel.Append(button);


            text2.HAlign = text2.VAlign = 0.5f; // 4
            button.Append(text2); // 5
        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (oldScale != Main.inventoryScale)
            {
                oldScale = Main.inventoryScale;
                Recalculate();
            }
        }*/
    }
}

using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.Graphics.Effects;

namespace EEMod.UI
{
    internal class EEUI : UIState
    {
        public override void OnActivate()
        {

        }

        bool[] isPulsating = new bool[5];
        DragableUIPanel panel = new DragableUIPanel();
        DragableUIPanel panel2 = new DragableUIPanel();
        DragableUIPanel panel3 = new DragableUIPanel();
        DragableUIPanel panel4 = new DragableUIPanel();
        DragableUIPanel panel5 = new DragableUIPanel();
        DragableUIPanel panel6 = new DragableUIPanel();
        DragableUIPanel panel7 = new DragableUIPanel();
        private UIText text;
        private UIText text2;
        private UIText text3;
        private UIText text4;
        private UIText text5;
        private UIText firstRune;
        private UIText secondRune;
        public float pivot;
        public int pauseTimer;

        public override void OnInitialize()
        {
            
            panel.Width.Set(100, 0);
            panel.Height.Set(100, 0);
            panel.Left.Set((Main.screenWidth/2) + (float)(Math.Sin(pivot/10))*500, 0);
            panel.Top.Set((Main.screenHeight / 2) + (float)(Math.Sin(pivot / 10)) * 500, 0);
            panel.OnClick += OnButtonClick;
            Append(panel);
            text = new UIText("Desert Rune")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel.Append(text);
            panel2.Width.Set(100, 0);
            panel2.Height.Set(100, 0);
            panel2.Left.Set(Main.screenWidth * 0.2f - 50, 0);
            panel2.Top.Set(100, 0);
            panel2.OnClick += OnButtonClick2;
            Append(panel2);
            text2 = new UIText("Ignus Rune")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel2.Append(text2);
            panel3.Width.Set(100, 0);
            panel3.Height.Set(100, 0);
            panel3.Left.Set(Main.screenWidth * 0.4f - 50, 0);
            panel3.Top.Set(100, 0);
            panel3.OnClick += OnButtonClick3;
            Append(panel3);
            text3 = new UIText("Leaf Rune")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel3.Append(text3);
            panel4.Width.Set(100, 0);
            panel4.Height.Set(100, 0);
            panel4.Left.Set(Main.screenWidth * 0.6f - 50, 0);
            panel4.Top.Set(100, 0);
            panel4.OnClick += OnButtonClick4;
            Append(panel4);
            text4 = new UIText("Bubble Rune")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel4.Append(text4);
            panel5.Width.Set(100, 0);
            panel5.Height.Set(100, 0);
            panel5.Left.Set(Main.screenWidth * 0.8f - 50, 0);
            panel5.Top.Set(100, 0);
            panel5.OnClick += OnButtonClick5;
            Append(panel5);
            text5 = new UIText("I dont know the names Rune")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel5.Append(text5);

            panel6.Width.Set(100, 0);
            panel6.Height.Set(100, 0);
            panel6.Left.Set(Main.screenWidth * .5f - 150, 0);
            panel6.Top.Set(450, 0);
            panel6.OnClick += ChooseFirstRune;
            Append(panel6);
            firstRune = new UIText("Choose a Rune!")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel6.Append(firstRune);
            panel7.Width.Set(100, 0);
            panel7.Height.Set(100, 0);
            panel7.Left.Set(Main.screenWidth * .5f + 50, 0);
            panel7.Top.Set(450, 0);
            panel7.OnClick += ChooseSecondRune;
            Append(panel7);
            secondRune = new UIText("Choose a Rune!")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            panel7.Append(secondRune);

        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < 5; i++)
                isPulsating[i] = false;
            isPulsating[0] = true;
        }
        private void OnButtonClick2(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < 5; i++)
                isPulsating[i] = false;
            isPulsating[1] = true;
        }
        private void OnButtonClick3(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < 5; i++)
                isPulsating[i] = false;
            isPulsating[2] = true;
        }
        private void OnButtonClick4(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < 5; i++)
                isPulsating[i] = false;
            isPulsating[3] = true;
        }
        private void OnButtonClick5(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < 5; i++)
                isPulsating[i] = false;
            isPulsating[4] = true;
        }
        private void ChooseFirstRune(UIMouseEvent evt, UIElement listeningElement)
        {
            int choose;
            string textBeforeChange = firstRune.Text;
            for (int i = 0; i<5; i++)
            {
                if(isPulsating[i])
                {
                    choose = i + 1;
                    switch (choose)
                    {
                        case 1:
                            {
                                firstRune.SetText("Desert Rune");
                                break;
                            }
                        case 2:
                            {
                                firstRune.SetText("Ignus Rune");
                                break;
                            }
                        case 3:
                            {
                                firstRune.SetText("Leaf Rune");
                                break;
                            }
                        case 4:
                            {
                                firstRune.SetText("Bubble Rune");
                                break;
                            }
                        case 5:
                            {
                                firstRune.SetText("I dont know the names Rune");
                                break;
                            }
                    }
                    break;
                }
            }
            if (secondRune.Text == firstRune.Text)
            {
                firstRune.SetText(textBeforeChange);
            }
        }
        private void ChooseSecondRune(UIMouseEvent evt, UIElement listeningElement)
        {
            int choose;
            string textBeforeChange = secondRune.Text;
            for (int i = 0; i < 5; i++)
            {
                if (isPulsating[i])
                {
                    
                    choose = i + 1;
                    switch (choose)
                    {
                        case 1:
                            {
                                secondRune.SetText("Desert Rune");
                                break;
                            }
                        case 2:
                            {
                                secondRune.SetText("Ignus Rune");
                                break;
                            }
                        case 3:
                            {
                                secondRune.SetText("Leaf Rune");
                                break;
                            }
                        case 4:
                            {
                                secondRune.SetText("Bubble Rune");
                                break;
                            }
                        case 5:
                            {
                                secondRune.SetText("I dont know the names Rune");
                                break;
                            }
                    }
                }
            }
            if(secondRune.Text == firstRune.Text)
            {
                secondRune.SetText(textBeforeChange);
            }
        }

        float pulsatingControl;
        public override void Update(GameTime gameTime)
        {
            pivot = 0.05f;
            panel.Left.Set((Main.screenWidth / 2) - 50 + (float)(Math.Sin(pivot / 10)) * 300, 0);
            panel.Top.Set((Main.screenHeight / 2) - 50 + (float)(Math.Cos(pivot / 10)) * 300, 0);
            panel2.Left.Set((Main.screenWidth / 2)- 50 + (float)Math.Sin((pivot / 10) + (Math.PI * 2) / 5) * 300, 0);
            panel2.Top.Set((Main.screenHeight / 2) - 50 + (float)Math.Cos((pivot / 10) + (Math.PI * 2) / 5) * 300, 0);
            panel3.Left.Set((Main.screenWidth / 2) - 50 + (float)Math.Sin((pivot / 10) + (Math.PI*4) / 5) * 300, 0);
            panel3.Top.Set((Main.screenHeight / 2) - 50 + (float)Math.Cos((pivot / 10) + (Math.PI * 4) / 5) * 300, 0);
            panel4.Left.Set((Main.screenWidth / 2) - 50 + (float)Math.Sin((pivot / 10) + (Math.PI * 6) / 5) * 300, 0);
            panel4.Top.Set((Main.screenHeight / 2) - 50 + (float)Math.Cos((pivot / 10) + (Math.PI * 6) / 5) * 300, 0);
            panel5.Left.Set((Main.screenWidth / 2) - 50 + (float)Math.Sin((pivot / 10) + (Math.PI * 8) / 5) * 300, 0);
            panel5.Top.Set((Main.screenHeight / 2) - 50 + (float)Math.Cos((pivot / 10) + (Math.PI * 8) / 5) * 300, 0);
            pulsatingControl += 0.2f;
            if (isPulsating[0])
            {
                panel.Width.Set(105 + (float)Math.Sin(pulsatingControl)*10, 0);
                panel.Height.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
            }
            else
            {
                panel.Width.Set(100,0);
                panel.Height.Set(100, 0);
            }
            if (isPulsating[1])
            {
                panel2.Width.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
                panel2.Height.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
            }
            else
            {
                panel2.Width.Set(100, 0);
                panel2.Height.Set(100, 0);
            }
            if (isPulsating[2])
            {
                panel3.Width.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
                panel3.Height.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
            }
            else
            {
                panel3.Width.Set(100, 0);
                panel3.Height.Set(100, 0);
            }
            if (isPulsating[3])
            {
                panel4.Width.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
                panel4.Height.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
            }
            else
            {
                panel4.Width.Set(100, 0);
                panel4.Height.Set(100, 0);
            }
            if (isPulsating[4])
            {
                panel5.Width.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
                panel5.Height.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
            }
            else
            {
                panel5.Width.Set(100, 0);
                panel5.Height.Set(100, 0);
            }

            
        }
    }
}

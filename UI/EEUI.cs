using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace EEMod.UI
{
    internal class EEUI : UIState
    {
        public override void OnActivate()
        {

        }

        public override void OnDeactivate()
        {
            for (int i = 0; i < 5; i++)
                isPulsating[i] = false;
        }
        public static string[] StringOfTextures = new string[7];
        bool[] isPulsating = new bool[5];
        DragableUIPanel[] panels = new DragableUIPanel[7];
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
            StringOfTextures[0] = "EEMod/NPCs/Sphinx";
            StringOfTextures[1] = "EEMod/NPCs/Bosses/Stagrel/Stagrel";
            StringOfTextures[2] = "EEMod/NPCs/CoralReefs/ToxicPuffer";
            StringOfTextures[3] = "EEMod/NPCs/CoralReefs/Dreadmine";
            StringOfTextures[4] = "EEMod/NPCs/CoralReefs/ToxicPufferSmall";
            StringOfTextures[5] = "EEMod/NPCs/CoralReefs/Dreadmine";
            StringOfTextures[6] = "EEMod/NPCs/CoralReefs/Dreadmine";
            for(int i = 0; i<7; i++)
            {
                panels[i] = new DragableUIPanel(StringOfTextures[i]);
                if (i < 5)
                {
                    panels[i].Width.Set(100, 0);
                    panels[i].Height.Set(100, 0);
                    panels[i].Left.Set((Main.screenWidth / 2) + (float)(Math.Sin(pivot / 10)) * 500, 0);
                    panels[i].Top.Set((Main.screenHeight / 2) + (float)(Math.Sin(pivot / 10)) * 500, 0);
                    Append(panels[i]);
                }
            }
            panels[0].OnClick += OnButtonClick;
            panels[1].OnClick += OnButtonClick2;
            panels[2].OnClick += OnButtonClick3;
            panels[3].OnClick += OnButtonClick4;
            panels[4].OnClick += OnButtonClick5;

            panels[5].Width.Set(100, 0);
            panels[5].Height.Set(100, 0);
            panels[5].Left.Set(Main.screenWidth * .5f - 150, 0);
            panels[5].Top.Set(450, 0);
            panels[5].OnClick += ChooseFirstRune;
            Append(panels[5]);

            panels[6].Width.Set(100, 0);
            panels[6].Height.Set(100, 0);
            panels[6].Left.Set(Main.screenWidth * .5f + 50, 0);
            panels[6].Top.Set(450, 0);
            panels[6].OnClick += ChooseSecondRune;
            Append(panels[6]);

        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[0] == 1)
            {
                for (int i = 0; i < 5; i++)
                    isPulsating[i] = false;
                isPulsating[0] = true;
            }
            else
            {
                isPulsating[0] = false;
            }
        }
        private void OnButtonClick2(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[1] == 1)
            {
                for (int i = 0; i < 5; i++)
                    isPulsating[i] = false;
                isPulsating[1] = true;
            }
            else
            {
                isPulsating[1] = false;
            }
        }
        private void OnButtonClick3(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[2] == 1)
            {
                for (int i = 0; i < 5; i++)
                    isPulsating[i] = false;
                isPulsating[2] = true;
            }
            else
            {
                isPulsating[2] = false;
            }
        }
        private void OnButtonClick4(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[3] == 1)
            {
                for (int i = 0; i < 5; i++)
                    isPulsating[i] = false;
                isPulsating[3] = true;
            }
            else
            {
                isPulsating[3] = false;
            }
        }
        private void OnButtonClick5(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[4] == 1)
            {
                for (int i = 0; i < 5; i++)
                    isPulsating[i] = false;
                isPulsating[4] = true;
            }
            else
            {
                isPulsating[4] = false;
            }
        }
        private void ChooseFirstRune(UIMouseEvent evt, UIElement listeningElement)
        {
            Texture2D textBeforeChange = panels[5]._backgroundTexture;
            for (int i = 0; i<5; i++)
            {
                if(isPulsating[i])
                {
                    panels[5]._backgroundTexture = ModContent.GetTexture(StringOfTextures[i]);
                    break;
                }
            }
            if (panels[5]._backgroundTexture == panels[6]._backgroundTexture)
            {
                panels[5]._backgroundTexture = textBeforeChange;
            }
        }
        private void ChooseSecondRune(UIMouseEvent evt, UIElement listeningElement)
        {
            Texture2D textBeforeChange = panels[6]._backgroundTexture;
            for (int i = 0; i < 5; i++)
            {
                if (isPulsating[i])
                {
                    panels[6]._backgroundTexture = ModContent.GetTexture(StringOfTextures[i]);
                    break;
                }
            }
            if (panels[5]._backgroundTexture == panels[6]._backgroundTexture)
            {
                panels[6]._backgroundTexture = textBeforeChange;
            }
        }

        float pulsatingControl;
        public override void Update(GameTime gameTime)
        {
            pivot = 0.05f;
            for(int i = 0; i<5; i++)
            {
               panels[i].Left.Set((Main.screenWidth / 2) - 50 + (float)(Math.Sin((pivot / 10) + (Math.PI * (i*2)) / 5)) * 300, 0);
               panels[i].Top.Set((Main.screenHeight / 2) - 50 + (float)(Math.Cos((pivot / 10) + (Math.PI * (i * 2)) / 5)) * 300, 0);
            }
            pulsatingControl += 0.2f;
            for(int i = 0; i<5; i++)
            {
                if (isPulsating[i])
                {
                    panels[i].Width.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
                    panels[i].Height.Set(105 + (float)Math.Sin(pulsatingControl) * 10, 0);
                }
                else
                {
                    panels[i].Width.Set(100, 0);
                    panels[i].Height.Set(100, 0);
                }
            }
        }
    }
}

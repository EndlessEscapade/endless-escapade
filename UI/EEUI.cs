using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace EEMod.UI
{
    internal class EEUI : UIState
    {
        public override void OnActivate()
        {
        }

        public override void OnDeactivate()
        {
            for (int i = 0; i < 7; i++)
            {
                isPulsating[i] = false;
            }
        }

        public string[] StringOfTextures = {
            "EEMod/Projectiles/Runes/DesertRune",
            "EEMod/Projectiles/Runes/DepocaditaRune",
            "EEMod/Projectiles/Runes/BubblingWatersRune",
            "EEMod/Projectiles/Runes/FeralWrathRune",
            "EEMod/Projectiles/Runes/IgnisRune",
            "EEMod/Projectiles/Runes/PermafrostRune",
            "EEMod/Projectiles/Runes/CycloneStormRune",
            "EEMod/Projectiles/Runes/RunePlacement",
            "EEMod/Projectiles/Runes/RunePlacement"
            };

        private readonly bool[] isPulsating = new bool[7];
        private readonly Vector2[] sizes = new Vector2[7];
        private readonly DragableUIPanelBackgroundTexture[] panels = new DragableUIPanelBackgroundTexture[9];
        public float pivot;
        public int pauseTimer;

        public override void OnInitialize()
        {
            for (int i = 0; i < 9; i++)
            {
                panels[i] = new DragableUIPanelBackgroundTexture(StringOfTextures[i]);
                if (i < 7)
                {
                    panels[i].Width.Set(ModContent.GetTexture(StringOfTextures[i]).Width, 0);
                    panels[i].Height.Set(ModContent.GetTexture(StringOfTextures[i]).Height, 0);
                    panels[i].Left.Set((Main.screenWidth / 2) + (float)Math.Sin(pivot / 10) * 500, 0);
                    panels[i].Top.Set((Main.screenHeight / 2) + (float)Math.Sin(pivot / 10) * 500, 0);
                    Append(panels[i]);
                }
            }
            panels[0].OnClick += OnButtonClick;
            panels[1].OnClick += OnButtonClick2;
            panels[2].OnClick += OnButtonClick3;
            panels[3].OnClick += OnButtonClick4;
            panels[4].OnClick += OnButtonClick5;
            panels[5].OnClick += OnButtonClick6;
            panels[6].OnClick += OnButtonClick7;

            panels[7].Width.Set(100, 0);
            panels[7].Height.Set(100, 0);
            panels[7].Left.Set(Main.screenWidth * .5f - 150, 0);
            panels[7].Top.Set(450, 0);
            panels[7].OnClick += ChooseFirstRune;
            Append(panels[7]);
            panels[8].Width.Set(100, 0);
            panels[8].Height.Set(100, 0);
            panels[8].Left.Set(Main.screenWidth * .5f + 50, 0);
            panels[8].Top.Set(450, 0);
            panels[8].OnClick += ChooseSecondRune;
            Append(panels[8]);
        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[0] == 1)
            {
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

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
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

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
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

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
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

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
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

                isPulsating[4] = true;
            }
            else
            {
                isPulsating[4] = false;
            }
        }

        private void OnButtonClick6(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[5] == 1)
            {
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

                isPulsating[5] = true;
            }
            else
            {
                isPulsating[5] = false;
            }
        }

        private void OnButtonClick7(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[6] == 1)
            {
                for (int i = 0; i < 7; i++)
                {
                    isPulsating[i] = false;
                }

                isPulsating[6] = true;
            }
            else
            {
                isPulsating[6] = false;
            }
        }

        private void ChooseFirstRune(UIMouseEvent evt, UIElement listeningElement)
        {
            Texture2D textBeforeChange = panels[7]._backgroundTexture;
            for (int i = 0; i < 7; i++)
            {
                if (isPulsating[i])
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().inPossesion[i] = 1;
                    panels[7]._backgroundTexture = ModContent.GetTexture(StringOfTextures[i]);
                    panels[7].Width.Set(ModContent.GetTexture(StringOfTextures[i]).Width, 0);
                    panels[7].Height.Set(ModContent.GetTexture(StringOfTextures[i]).Height, 0);
                    panels[7].Left.Set(Main.screenWidth * .5f - ModContent.GetTexture(StringOfTextures[i]).Width / 2 - 100, 0);
                    panels[7].Top.Set(500 - ModContent.GetTexture(StringOfTextures[i]).Height / 2, 0);
                    break;
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().inPossesion[i] = 0;
                }
            }
            if (panels[7]._backgroundTexture == panels[8]._backgroundTexture)
            {
                panels[7]._backgroundTexture = textBeforeChange;
            }
        }

        private void ChooseSecondRune(UIMouseEvent evt, UIElement listeningElement)
        {
            Texture2D textBeforeChange = panels[8]._backgroundTexture;
            for (int i = 0; i < 7; i++)
            {
                if (isPulsating[i])
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().inPossesion[i] = 1;
                    panels[8]._backgroundTexture = ModContent.GetTexture(StringOfTextures[i]);
                    panels[8].Width.Set(ModContent.GetTexture(StringOfTextures[i]).Width, 0);
                    panels[8].Height.Set(ModContent.GetTexture(StringOfTextures[i]).Height, 0);
                    panels[8].Left.Set(Main.screenWidth * .5f - ModContent.GetTexture(StringOfTextures[i]).Width / 2 + 100, 0);
                    panels[8].Top.Set(500 - +ModContent.GetTexture(StringOfTextures[i]).Height / 2, 0);
                    break;
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().inPossesion[i] = 0;
                }
            }
            if (panels[7]._backgroundTexture == panels[8]._backgroundTexture)
            {
                panels[8]._backgroundTexture = textBeforeChange;
            }
        }

        private float pulsatingControl;

        public override void Update(GameTime gameTime)
        {
            pivot = 0.05f;
            for (int i = 0; i < 7; i++)
            {
                panels[i].Left.Set((Main.screenWidth / 2) - ModContent.GetTexture(StringOfTextures[i]).Width / 2 + (float)Math.Sin((pivot / 10) + Math.PI * (i * 2) / 7) * 200, 0);
                panels[i].Top.Set((Main.screenHeight / 2) - ModContent.GetTexture(StringOfTextures[i]).Height / 2 + (float)Math.Cos((pivot / 10) + Math.PI * (i * 2) / 7) * 200, 0);
            }
            pulsatingControl += 0.2f;
            for (int i = 0; i < 7; i++)
            {
                if (isPulsating[i])
                {
                    panels[i].Width.Set(ModContent.GetTexture(StringOfTextures[i]).Width + (float)Math.Sin(pulsatingControl) * 2, 0);
                    panels[i].Height.Set(ModContent.GetTexture(StringOfTextures[i]).Height + (float)Math.Sin(pulsatingControl) * 2, 0);
                }
                else
                {
                    panels[i].Width.Set(ModContent.GetTexture(StringOfTextures[i]).Width, 0);
                    panels[i].Height.Set(ModContent.GetTexture(StringOfTextures[i]).Height, 0);
                }
            }
        }
    }
}
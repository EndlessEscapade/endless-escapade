using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;

namespace EEMod.UI.States
{
    public class FishermansLogUI : UIState
    {
        public UIImage Background = new UIImage(ModContent.GetTexture("EEMod/UI/FishermansLogUI"));
        public UIPanel FishPanel = new UIPanel();
        public UIGrid FishGrid = new UIGrid();
        FixedUIScrollbar ScrollBar = new FixedUIScrollbar();
        public override void OnInitialize()
        {
            Background.HAlign = 0.5f;
            Background.VAlign = 0.5f;

            FishPanel.Width.Set(344, 0f);
            FishPanel.Height.Set(356, 0f);
            FishPanel.HAlign = 0.05f;
            FishPanel.VAlign = 0.80f;
            FishPanel.BackgroundColor = new Color();
            Background.Append(FishPanel);

            ScrollBar.SetView(100f, 1000f);
            ScrollBar.Top.Pixels = 32f + 8f;
            ScrollBar.Height.Set(-50f - 8f, 1f);
            ScrollBar.HAlign = 1f;
            FishPanel.Append(ScrollBar);

            FishGrid.Width.Set(344, 0f);
            FishGrid.Height.Set(356, 0f);
            FishGrid.HAlign = 0.5f;
            FishGrid.VAlign = 0.80f;
            FishGrid.ListPadding = 9f;
            FishGrid.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
            FishGrid.SetScrollbar(ScrollBar);
            FishPanel.Append(FishGrid);

            Append(Background);
            LoadAllFish();
        }
        internal static void OnScrollWheel_FixHotbarScroll(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.ScrollHotbar(Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120);
        }
        public UIPanel LoadFish(int itemType)
        {
            UIPanel panel = new UIPanel();
            panel.Width.Set(50, 0f);
            panel.Height.Set(58, 0f);
            panel.BackgroundColor = new Color();

            FishElement fish = new FishElement(itemType);
            fish.HAlign = -0.25f;
            panel.Append(fish);

            return panel;
        }
        public void LoadAllFish()
        {
            FishGrid.Add(LoadFish(ItemID.Bass));
            FishGrid.Add(LoadFish(ItemID.AtlanticCod));
            FishGrid.Add(LoadFish(ItemID.Ebonkoi));
        }
    }
    public class FishElement : UIElement 
    {
        public int itemType;
        public int npcType;
        public string habitat;
        public string description;
        public FishElement(int itemType)
        {
            this.itemType = itemType;
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture = Main.itemTexture[itemType];
            int x = (int)(dimensions.X + texture.Size().X);
            int y = (int)(dimensions.Y + texture.Size().Y);
            spriteBatch.Draw(texture, new Vector2(x, y), null, Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths.ContainsKey(itemType) ? Color.White : Color.Black, 0f, texture.Size(), 1f, SpriteEffects.None, 0f);
        }
    }
    public class FixedUIScrollbar : UIScrollbar
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            UserInterface temp = UserInterface.ActiveInstance;
            UserInterface.ActiveInstance = EEMod.UI.GetInterface("EEInterfacee");
            base.DrawSelf(spriteBatch);
            UserInterface.ActiveInstance = temp;
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            UserInterface temp = UserInterface.ActiveInstance;
            UserInterface.ActiveInstance = EEMod.UI.GetInterface("EEInterfacee");
            base.MouseDown(evt);
            UserInterface.ActiveInstance = temp;
        }
    }
}
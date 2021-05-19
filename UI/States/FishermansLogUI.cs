using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.ModLoader.IO;
using EEMod.Items;
using EEMod.Extensions;

namespace EEMod.UI.States
{
    public class FishermansLogUI : UIState
    {
        public UIText Name;
        public UIText ExtraInfo;
        public UIText Description;
        public UIImage Background = new UIImage(ModContent.GetTexture("EEMod/UI/FishermansLogUI"));
        public UIElement RightStuff = new UIElement();
        public UIPanel FishPanel = new UIPanel();
        public UIGrid FishGrid = new UIGrid();
        public FixedUIScrollbar ScrollBar = new FixedUIScrollbar();
        public List<UIElement> FullList = new List<UIElement>();
        public UIElement SelectedFish;
        public bool ClosingUI;
        public int SlideTimer = 0;
        public override void OnInitialize()
        {
            Background.HAlign = 0.5f;
            Background.VAlign = 2f;

            FishPanel.Width.Set(344, 0f);
            FishPanel.Height.Set(416, 0f);
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
            FishGrid.Height.Set(416, 0f);
            FishGrid.HAlign = 0.5f;
            FishGrid.VAlign = 0.80f;
            FishGrid.ListPadding = 9f;
            FishGrid.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
            FishGrid.SetScrollbar(ScrollBar);
            FishPanel.Append(FishGrid);

            RightStuff.Width.Set(376, 0f);
            RightStuff.Height.Set(496, 0f);
            RightStuff.HAlign = 1f;
            Background.Append(RightStuff);

            Name = new UIText("");
            Name.HAlign = 0.5f;
            Name.VAlign = 0.475f;
            RightStuff.Append(Name);

            ExtraInfo = new UIText("");
            ExtraInfo.HAlign = 0.5f;
            ExtraInfo.VAlign = 0.575f;
            RightStuff.Append(ExtraInfo);

            Description = new UIText("");
            Description.HAlign = 0.5f;
            Description.VAlign = 0.8f;
            RightStuff.Append(Description);

            Append(Background);
            LoadAllFish();
        }
        public override void OnActivate()
        {
            base.OnActivate();
            ClosingUI = false;
            Background.VAlign = 2f;
            SlideTimer = 0;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Background.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (SlideTimer < 10)
            {
                Background.VAlign = MathHelper.Lerp(ClosingUI ? 0.5f : 2f, ClosingUI ? 2f : 0.5f, SlideTimer / 9f);
                SlideTimer++;
                if (SlideTimer == 10 && ClosingUI)
                {
                    EEMod.UI.RemoveState("EEInterfacee");
                }
            }
        }
        internal static void OnScrollWheel_FixHotbarScroll(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.ScrollHotbar(Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120);
        }
        public void LoadFilters(string[] filters)
        {
            FishGrid._items = FullList.FindAll(e => filters.Any((e as FishElement).habitat.Contains));
        }
        public void LoadAllFish()
        {
            FishGrid.Add(new FishElement(ItemID.Bass, "Yea cat echhh hhhhhhhhhhhdhs  uidfhafuiafaf a    a fh h monkeymonkeymonkeymonkey e", "purity|underground|snow|ug ice|a lot"));
            FishGrid.Add(new FishElement(ItemID.AtlanticCod, "Na cat", "snow|ug ice"));
            FishGrid.Add(new FishElement(ItemID.Ebonkoi, "Mayb cat", "corruption|ug corruption"));
            FullList = FishGrid._items;
        }
    }
    public class FishElement : UIImageButton 
    {
        public FishermansLogUI LogUI;
        public Texture2D borderTexture = ModContent.GetTexture("EEMod/UI/FishBorder");
        public bool caught;
        public int maxSize;
        public enum maxSizes { Small = 16, Medium = 32, Big = 44 }
        public int itemType;
        public string description;
        public string habitat;
        public Texture2D swimmingAnimation;
        public int frameHeight;
        public int frameWidth;

        /// <param name="itemType">The type of the item that'll be used as the selection sprite.</param>
        /// <param name="habitat">Will determine what background and water to use on the display, if multiple, put a "|" between each and they'll cycle.</param>
        /// <param name="swimmingAnimation">The sprite sheet used to make the fish swim in the display, if left null, the item sprite will be used instead.</param>
        public FishElement(int itemType, string description, string habitat, Texture2D swimmingAnimation = null, int frameHeight = 0, int frameWidth = 0) : base(ModContent.GetTexture("EEMod/UI/FishBorder"))
        {
            this.itemType = itemType;
            this.description = description;
            this.habitat = habitat;
            this.swimmingAnimation = swimmingAnimation ?? swimmingAnimation;
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
            EEGlobalItem eeGlobalItem = new EEGlobalItem();
            if (eeGlobalItem.smallSizeFish.Contains(itemType))
            {
                maxSize = 16;
            }
            else if (eeGlobalItem.averageSizeFish.Contains(itemType))
            {
                maxSize = 32;
            }
            else if (eeGlobalItem.bigSizeFish.Contains(itemType))
            {
                maxSize = 44;
            }
        }
        public override void OnInitialize()
        {
            LogUI = (Parent.Parent.Parent.Parent.Parent as FishermansLogUI);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            caught = Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths.ContainsKey(itemType);
            if (caught)
            {
                SetImage(Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths[itemType] == maxSize ? ModContent.GetTexture("EEMod/UI/FishBorderGold") : ModContent.GetTexture("EEMod/UI/FishBorder"));
            }
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (caught)
            {
                LogUI.Name.SetText(Lang.GetItemNameValue(itemType));
                LogUI.ExtraInfo.SetText($"Habitat: {"habitat"}\nSize: {Enum.GetName(typeof(maxSizes), maxSize)}\nBiggest Catch: {Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths[itemType]} cm");
                LogUI.Description.SetText(description.FormatString(32));
            }
            else
            {
                LogUI.Name.SetText("???");
                LogUI.ExtraInfo.SetText("Habitat: ???\nSize: ???\nBiggest Catch: ???");
                LogUI.Description.SetText("???");
            }
            LogUI.SelectedFish = this;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture = Main.itemTexture[itemType];
            int x = (int)(dimensions.X + (texture.Size().X + borderTexture.Size().Y) / 2);
            int y = (int)(dimensions.Y + (texture.Size().Y + borderTexture.Size().Y) / 2);
            float transparency = IsMouseHovering || LogUI.SelectedFish == this ? 1f : 0.4f;
            SetVisibility(1f, transparency);
            spriteBatch.Draw(texture, new Vector2(x, y), null, (caught ? Color.White : Color.Black) * transparency, 0f, texture.Size(), 1f, SpriteEffects.None, 0f);
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
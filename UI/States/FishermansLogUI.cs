using System;
using System.Linq;
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
        public static UIText Description = new UIText("");
        public UIImage Background = new UIImage(ModContent.GetTexture("EEMod/UI/FishermansLogUI"));
        public UIElement RightStuff = new UIElement();
        public UIPanel FishPanel = new UIPanel();
        public UIGrid FishGrid = new UIGrid();
        public FixedUIScrollbar ScrollBar = new FixedUIScrollbar();
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

            RightStuff.Width.Set(376, 0f);
            RightStuff.Height.Set(436, 0f);
            RightStuff.HAlign = 1f;
            Background.Append(RightStuff);

            Description.HAlign = 0.5f;
            Description.VAlign = 0.65f;
            RightStuff.Append(Description);

            Append(Background);
            LoadAllFish();
        }
        internal static void OnScrollWheel_FixHotbarScroll(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.ScrollHotbar(Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120);
        }
        public void LoadAllFish()
        {
            FishGrid.Add(new FishElement(ItemID.Bass, "Yea cat echhh hhhhhhhhhhhdhs  uidfhafuiafaf a    a fh h monkeymonkeymonkeymonkey e", "purity|underground|snow|ug ice|a lot"));
            FishGrid.Add(new FishElement(ItemID.AtlanticCod, "Na cat", "snow|ug ice"));
            FishGrid.Add(new FishElement(ItemID.Ebonkoi, "Mayb cat", "corruption|ug corruption"));
        }
    }
    public class FishElement : UIImageButton 
    {
        public Texture2D borderTexture = ModContent.GetTexture("EEMod/UI/FishBorder");
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
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            string formattedDescription = "";
            int lineBreakPoint = 0;
            for (int i = 0; i < description.Length; i++)
            {
                //TODO: include a check for if the amount of chars until a space is higher than the amount left for a line break
                if (lineBreakPoint >= 32)
                {
                    lineBreakPoint = 0;
                    formattedDescription += "\n";
                }
                formattedDescription += description[i];
                lineBreakPoint++;
            }
            FishermansLogUI.Description.SetText(formattedDescription);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture = Main.itemTexture[itemType];
            int x = (int)(dimensions.X + (texture.Size().X + borderTexture.Size().Y) / 2);
            int y = (int)(dimensions.Y + (texture.Size().Y + borderTexture.Size().Y) / 2);
            float transparency = IsMouseHovering ? 1f : 0.4f;
            spriteBatch.Draw(texture, new Vector2(x, y), null, (Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths.ContainsKey(itemType) ? Color.White : Color.Black) * transparency, 0f, texture.Size(), 1f, SpriteEffects.None, 0f);
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
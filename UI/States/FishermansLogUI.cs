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
using EEMod.Items;
using EEMod.Extensions;

namespace EEMod.UI.States
{
    //TODO: Increase width of ExtraInfo border
    public class FishermansLogUI : UIState
    {
        public UIText Name;
        public UIText ExtraInfo;
        public UIText Description;
        public UIImage Background;
        public UIElement RightStuff;
        public FishDisplay Display;
        public UIElement LeftStuff;
        public UIImageButton FiltersMenuButton;
        public ProgressBar CaughtBar;
        public UIGrid FishGrid;
        public FixedUIScrollbar ScrollBar;
        public List<UIElement> FullList = new List<UIElement>();
        public UIElement SelectedFish;
        public bool ClosingUI;
        public int SlideTimer = 0;
        public override void OnInitialize()
        {
            Background = new UIImage(ModContent.GetTexture("EEMod/UI/FishermansLogUI"));
            Background.HAlign = 0.5f;
            Background.VAlign = 2f;

            LeftStuff = new UIElement();
            LeftStuff.Width.Set(424, 0f);
            LeftStuff.Height.Set(474, 0f);
            LeftStuff.HAlign = 0f;
            LeftStuff.VAlign = 0.825f;
            Background.Append(LeftStuff);

            ScrollBar = new FixedUIScrollbar(); 
            ScrollBar.SetView(100f, 1000f);
            ScrollBar.Top.Pixels = 32f + 8f;
            ScrollBar.Height.Set(-50f - 8f, 1f);
            ScrollBar.HAlign = 0.95f;
            LeftStuff.Append(ScrollBar);

            FishGrid = new UIGrid();
            FishGrid.Width.Set(392, 0f);
            FishGrid.Height.Set(420, 0f);
            FishGrid.HAlign = 0.85f;
            FishGrid.VAlign = 1f;
            FishGrid.ListPadding = 10f;
            FishGrid.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
            FishGrid.SetScrollbar(ScrollBar);
            LeftStuff.Append(FishGrid);

            //FiltersMenuButton = new UIImageButton(ModContent.GetTexture(""));

            CaughtBar = new ProgressBar();

            RightStuff = new UIElement();
            RightStuff.Width.Set(424, 0f);
            RightStuff.Height.Set(554, 0f);
            RightStuff.HAlign = 1f;
            Background.Append(RightStuff);

            Display = new FishDisplay();
            Display.HAlign = 0.5f;
            Display.VAlign = 0.125f;
            RightStuff.Append(Display);

            Name = new UIText("");
            Name.HAlign = 0.5f;
            Name.VAlign = 0.42775f;
            RightStuff.Append(Name);

            ExtraInfo = new UIText("");
            ExtraInfo.HAlign = 0.5f;
            ExtraInfo.VAlign = 0.52125f;
            RightStuff.Append(ExtraInfo);

            Description = new UIText("");
            Description.HAlign = 0.5f;
            Description.VAlign = 0.725f;
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
            FishGrid._items = FullList.Where(e => (e as FishElement).Habitats.Intersect(filters).Any()).ToList();
            CaughtBar.MaxCaught = FishGrid._items.Count;
            CaughtBar.CurrentCaught = FishGrid._items.Where(e => (e as FishElement).Caught).Count();
        }
        public void LoadAllFish()
        {
            LoadFish(new FishElement(ItemID.Bass, "Plentiful", "Bass is a name shared by many species of fish, encompassing both freshwater and marine species, all belonging to the large order Perciformes.", 
                "Anywhere")); //All except Desert in 1.4.
            LoadFish(new FishElement(ItemID.ArmoredCavefish, "Uncommon", "WIP",
                "Underground|Caverns|UG Jungle|UG Beehive|UG Corruption|UG Crimson|UG Hallow|Ice|Corrupt Ice|Crimson Ice|Hallow Ice|UG Glowing Mush|Underworld"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            LoadFish(new FishElement(ItemID.Ebonkoi, "Uncommon", "Mayb cat", "corruption|ug corruption"));
            //This is done so the fish are actually in the order they were loaded in since UIGrid is kinda wack.
            FishGrid._items = FullList;
        }
        internal void LoadFish(FishElement fishElement)
        {
            FullList.Add(fishElement);
            FishGrid.Add(fishElement);
        }
    }
    public class FishElement : UIImageButton 
    {
        public FishermansLogUI LogUI;
        public Texture2D BorderTexture = ModContent.GetTexture("EEMod/UI/FishBorder");
        public bool Caught;
        public int MaxSize;
        public int ItemType;
        public string Rarity;
        public string Description;
        public List<string> Habitats = new List<string>();
        public int SwimSpeed;
        public int AnimSpeed;
        public bool IsSpriteFacingRight;
        public Texture2D SwimmingAnimation;
        public int FrameCount;

        /// <param name="itemType">The type of the item that'll be used as the selection sprite.</param>
        /// <param name="rarity">Check "Catch Quality" on the wiki.</param>
        /// <param name="habitat">Will determine what background and water to use on the display, if multiple, put a "|" between each and they'll cycle.</param>
        /// <param name="swimSpeed">How many frames the fish takes to swim from one end to the other).</param>
        /// <param name="animSpeed">How many frames each frame of the animation lasts.</param>
        /// <param name="swimmingAnimation">The sprite sheet used to make the fish swim in the display, if left null, the item sprite will be used instead.</param>
        public FishElement(int itemType, string rarity, string description, string habitat, int swimSpeed = 70, int animSpeed = 30, bool isSpriteFacingRight = false, Texture2D swimmingAnimation = null, int frameCount = 1) : base(ModContent.GetTexture("EEMod/UI/FishBorder"))
        {
            ItemType = itemType;
            Rarity = rarity;
            Description = description;
            Habitats = habitat.Split('|').ToList();
            SwimSpeed = swimSpeed;
            AnimSpeed = animSpeed;
            IsSpriteFacingRight = isSpriteFacingRight;
            SwimmingAnimation = swimmingAnimation ?? swimmingAnimation;
            FrameCount = frameCount;
            EEGlobalItem eeGlobalItem = new EEGlobalItem();
            if (eeGlobalItem.smallSizeFish.Contains(itemType))
            {
                MaxSize = 16;
            }
            else if (eeGlobalItem.averageSizeFish.Contains(itemType))
            {
                MaxSize = 32;
            }
            else if (eeGlobalItem.bigSizeFish.Contains(itemType))
            {
                MaxSize = 44;
            }
        }
        public override void OnInitialize()
        {
            LogUI = (Parent.Parent.Parent.Parent.Parent as FishermansLogUI);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Caught = Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths.ContainsKey(ItemType);
            if (Caught)
            {
                SetImage(Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths[ItemType] == MaxSize ? ModContent.GetTexture("EEMod/UI/FishBorderGold") : ModContent.GetTexture("EEMod/UI/FishBorder"));
            }
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (Caught)
            {
                LogUI.Name.SetText(Lang.GetItemNameValue(ItemType));
                LogUI.ExtraInfo.SetText($"Habitat: {Habitats[0]}\nRarity: {Rarity}\nBiggest Catch: {Main.LocalPlayer.GetModPlayer<EEPlayer>().fishLengths[ItemType]} cm");
                LogUI.Description.SetText(Description.FormatString(32));
                (LogUI.Display as FishDisplay).UpdateDisplay(ItemType, IsSpriteFacingRight, Habitats, SwimSpeed, AnimSpeed, SwimmingAnimation, FrameCount);
            }
            else
            {
                LogUI.Name.SetText("???");
                LogUI.ExtraInfo.SetText("Habitat: ???\nRarity: ???\nBiggest Catch: ???");
                LogUI.Description.SetText("???");
                (LogUI.Display as FishDisplay).ShouldDraw = false;
            }
            LogUI.SelectedFish = this;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture = Main.itemTexture[ItemType];
            int x = (int)(dimensions.X + (texture.Size().X + BorderTexture.Size().X) / 2);
            int y = (int)(dimensions.Y + (texture.Size().Y + BorderTexture.Size().Y) / 2);
            float transparency = IsMouseHovering || LogUI.SelectedFish == this ? 1f : 0.4f;
            SetVisibility(1f, transparency);
            spriteBatch.Draw(texture, new Vector2(x, y), null, (Caught ? Color.White : Color.Black) * transparency, 0f, texture.Size(), 1f, SpriteEffects.None, 0f);
        }
    }
    public class FishDisplay : UIImage
    {
        public Texture2D OutlineTexture = ModContent.GetTexture("EEMod/UI/DisplayBorder");
        public bool ShouldDraw;
        public bool IsSpriteFacingRight;
        public string CurrentHabitat;
        public List<string> Habitats = new List<string>(); 
        public float SwimSpeed;
        public int AnimSpeed;
        public Texture2D SwimmingAnimation;
        public int FrameCount;
        public Texture2D BackgroundTexture;
        public bool FacingLeft = true;
        public int HabitatTimer;
        public int SwimTimer;
        public bool IsUsingItemTexture;
        public Rectangle Frame;
        public int FrameCounter;
        public FishDisplay() : base(ModContent.GetTexture("EEMod/UI/DisplayBorder")) { }
        public void UpdateDisplay(int itemType, bool isSpriteFacingRight, List<string> habitats, int swimSpeed, int animSpeed, Texture2D swimmingAnimation, int frameCount)
        {
            ShouldDraw = true;
            Habitats = habitats;
            CurrentHabitat = Habitats[0];
            SwimSpeed = swimSpeed;
            AnimSpeed = animSpeed;
            if (swimmingAnimation == null)
            {
                IsUsingItemTexture = true;
                SwimmingAnimation = Main.itemTexture[itemType];
                FrameCount = 1;
                IsSpriteFacingRight = true;
            }
            else
            {
                IsSpriteFacingRight = isSpriteFacingRight;
                SwimmingAnimation = swimmingAnimation;
                FrameCount = frameCount;
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (ShouldDraw)
            {
                //This should be in Update() but for some reason it is VERY stubborn in being consistent. ~Exitium
                if (++HabitatTimer >= 60)
                {
                    HabitatTimer = 0;
                    var oldIndex = Habitats.IndexOf(CurrentHabitat);
                    var index = oldIndex + 1;
                    if (index >= Habitats.Count) index = 0;
                    CurrentHabitat = Habitats[index];
                    (Parent.Parent.Parent as FishermansLogUI).ExtraInfo.SetText((Parent.Parent.Parent as FishermansLogUI).ExtraInfo.Text.Replace(Habitats[oldIndex], CurrentHabitat));
                }

                var facingLeft = IsSpriteFacingRight ? !FacingLeft : FacingLeft;
                var spriteEffects = facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                BackgroundTexture = ModContent.GetTexture("EEMod/UI/LogDisplayBGs/" + CurrentHabitat.Replace(" ", ""));
                CalculatedStyle dimensions = GetDimensions();
                if (++SwimTimer >= SwimSpeed)
                {
                    FacingLeft = !FacingLeft;
                    SwimTimer = 0;
                }
                int x = (int)(dimensions.X + (SwimmingAnimation.Size().X + OutlineTexture.Size().X) / 2 + MathHelper.Lerp(FacingLeft ? 50 : -75, FacingLeft ? -75 : 50, SwimTimer / SwimSpeed));
                int y = (int)(dimensions.Y + (SwimmingAnimation.Size().Y + OutlineTexture.Size().Y) / 2) + 34 - (IsUsingItemTexture && !facingLeft ? SwimmingAnimation.Height / 2 : 0);
                int xB = (int)(dimensions.X + (BackgroundTexture.Size().X + OutlineTexture.Size().X) / 2);
                int yB = (int)(dimensions.Y + (BackgroundTexture.Size().Y + OutlineTexture.Size().Y) / 2);
                if (FrameCounter >= AnimSpeed)
                {
                    FrameCounter = 0;
                    Frame.Y += SwimmingAnimation.Height / FrameCount;
                }
                if (Frame.Y >= SwimmingAnimation.Height / FrameCount * (FrameCount - 1))
                {
                    Frame.Y = 0;
                }
                FrameCounter++;
                spriteBatch.Draw(BackgroundTexture, new Vector2(xB, yB), null, Color.White, 0f, BackgroundTexture.Size(), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(SwimmingAnimation, new Vector2(x, y), null, Color.White, IsUsingItemTexture ? facingLeft ? 0.6f : -0.6f : 0f, SwimmingAnimation.Size(), 1f, spriteEffects, 0f);
                //spriteBatch.Draw(SwimmingAnimation, new Vector2(x, y), new Rectangle(0, Frame.Y, SwimmingAnimation.Width, SwimmingAnimation.Height / FrameCount), new Color(0, 0, 0), 0, new Rectangle(0, Frame.Y, SwimmingAnimation.Width, SwimmingAnimation.Height / FrameCounter).Size() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
    }
    public class FiltersElement : UIElement 
    {
        public FiltersElement()
        {
        }
    }
    internal class FilterButton : UIImageButton
    {
        public FilterButton() : base(ModContent.GetTexture("EEMod/UI/Filter")) { }
    }
    public class ProgressBar : UIElement
    {
        public int MaxCaught;
        public int CurrentCaught;
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
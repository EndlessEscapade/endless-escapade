using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ID;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using Microsoft.Xna.Framework.Input;

namespace EndlessEscapade.Common.FishermansLogUI;

public class FishermansLogUIState : UIState
{
    public static List<List<int>> SplitList(List<int> locations, int size) {
        var list = new List<List<int>>();

        for (int i = 0; i < locations.Count; i += size) {
            list.Add(locations.GetRange(i, Math.Min(size, locations.Count - i)));
        }

        return list;
    }

    public static bool Visible => Main.playerInventory;
    readonly string texturePath = "EndlessEscapade/Assets/UI";

    readonly List<int> catches = new List<int>() { ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.BlueJellyfish, ItemID.ChaosFish, ItemID.CrimsonTigerfish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.Ebonkoi, ItemID.FlarefinKoi, ItemID.Flounder, ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.GreenJellyfish, ItemID.Hemopiranha, ItemID.Honeyfin, ItemID.NeonTetra, ItemID.Obsidifish, ItemID.PinkJellyfish, ItemID.PrincessFish, ItemID.Prismite, ItemID.RedSnapper, ItemID.RockLobster, ItemID.Salmon, ItemID.Shrimp, ItemID.SpecularFish, ItemID.Stinkfish, ItemID.Trout, ItemID.Tuna, ItemID.VariegatedLardfish };

    readonly List<int> questCatches = new List<int>() { ItemID.AmanitaFungifin, ItemID.Angelfish, ItemID.Batfish, ItemID.BloodyManowar, ItemID.Bonefish, ItemID.BumblebeeTuna, ItemID.Bunnyfish, ItemID.CapnTunabeard, ItemID.Catfish, ItemID.Cloudfish, ItemID.Clownfish, ItemID.Cursedfish, ItemID.DemonicHellfish, ItemID.Derpfish, ItemID.Dirtfish, ItemID.DynamiteFish, ItemID.EaterofPlankton, ItemID.FallenStarfish, ItemID.TheFishofCthulu, ItemID.Fishotron, ItemID.Fishron, ItemID.GuideVoodooFish, ItemID.Harpyfish, ItemID.Hungerfish, ItemID.Ichorfish, ItemID.InfectedScabbardfish, ItemID.Jewelfish, ItemID.MirageFish, ItemID.Mudfish, ItemID.MutantFlinxfin, ItemID.Pengfish, ItemID.Pixiefish, ItemID.ScarabFish, ItemID.ScorpioFish, ItemID.Slimefish, ItemID.Spiderfish, ItemID.TropicalBarracuda, ItemID.TundraTrout, ItemID.UnicornFish, ItemID.Wyverntail, ItemID.ZombieFish };

    List<int> allCatches;

    int currentPage = 0;
    int pageCount = 0;
    List<List<int>> pages;
    List<FishermansLogGrid> grids = new();
    List<UIElement> gridElements = new();

    float padding = 15f;
    int elementsPerRow = 5;
    int elementsPerColumn;

    UISearchBar searchBar;
    Color searchBarBorderColor = Color.Black * 0.25f;

    string searchString;

    List<string> sortMethods = new() { "Name", "Item ID", "Unlocks (WIP)", "Length (WIP)" };
    int sortMethodIndex = 0;
    UIText sortButtonText;
    bool reverseOrder = false;
    bool numberedOrder = false;

    public override void OnInitialize() {
        SortElements();

        // ELEMENT ATTRIBUTES ARE SORTED BY TYPE IN THIS ORDER:
        // - POSITIONING (Top, HAlign, etc.)
        // - DISPLAY & SIZING (Width, PaddingLeft, etc.)
        // - COLORS (BackgroundColor, BorderColor, etc.)
        // - EVERYTHING ELSE

        UIImage MainPanel = this.AddElement(new UIImage(ModContent.Request<Texture2D>($"{texturePath}/FishermansLogUIPanel", ReLogic.Content.AssetRequestMode.ImmediateLoad)).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;

            e.Width = StyleDimension.FromPixels(800f);
            e.Height = StyleDimension.FromPixels(560f);
            e.SetPadding(12f);
        }));

        for (int i = 0; i < 2; i++) {
            UIElement PageContentContainer = MainPanel.AddElement(new UIElement().With(e => {
                e.HAlign = 1f * i;
                e.VAlign = 0.5f;

                e.Width = StyleDimension.FromPixelsAndPercent(-6f, 0.5f);
                e.Height = StyleDimension.Fill;
                e.SetPadding(padding);
            }));

            UIPanel topBarPanel = PageContentContainer.AddElement(new UIPanel().With(e => {
                e.Width = StyleDimension.Fill;
                e.Height = StyleDimension.FromPixels(28f);

                if (i == 0) {
                    e.BackgroundColor = new Color(191, 197, 201);
                    e.BorderColor = searchBarBorderColor;
                }
                else {
                    e.BackgroundColor = Color.Transparent;
                    e.BorderColor = Color.Transparent;
                }

                e.SetPadding(0f);
            }));

            if (i == 0) {
                searchBar = topBarPanel.AddElement(new UISearchBar(Language.GetText("Search"), 0.8f).With(e => {
                    e.VAlign = 0.5f;

                    e.Width = StyleDimension.Fill;
                    e.Height = StyleDimension.Fill;

                    e.OnClick += click_SearchArea;
                    e.OnContentsChanged += onSearchContentsChanged;

                    e.SetContents(null, true);
                }));

                UIImageButton searchCancelButton = topBarPanel.AddElement(new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", ReLogic.Content.AssetRequestMode.ImmediateLoad)).With(e => {
                    e.HAlign = 1f;
                    e.VAlign = 0.5f;
                    e.Left = StyleDimension.FromPixels(-2f);

                    e.OnMouseOver += searchCancelButton_OnMouseOver;
                    e.OnClick += searchCancelButton_OnClick;
                }));
            }
            else {
                UIHoverImageButton sortButton = topBarPanel.AddElement(new UIHoverImageButton(Main.Assets.Request<Texture2D>($"Images/UI/Bestiary/Button_Sorting", ReLogic.Content.AssetRequestMode.ImmediateLoad), "Sort by").With(e => {
                    e.SetVisibility(1f, 1f);
                    e.OnMouseOver += button_OnMouseOver;
                    e.OnClick += sortButton_OnClick;
                    e.OnRightClick += sortButton_OnRightClick;
                }));

                UIElement sortButtonTextPanel = sortButton.AddElement(new UIElement().With(e => {
                    e.HAlign = 1f;

                    e.Width = StyleDimension.FromPixels(86f);
                    e.Height = StyleDimension.Fill;
                    e.SetPadding(2f);
                }));

                sortButtonText = sortButtonTextPanel.AddElement(new UIText(sortMethods[sortMethodIndex], 0.8f).With(e => {
                    e.HAlign = 0.5f;
                    e.VAlign = 0.5f;
                }));

                UICheckbox sortReverseButton = topBarPanel.AddElement(new UICheckbox("Reverse", "").With(e => {
                    e.Left = StyleDimension.FromPixels(sortButton.GetOuterDimensions().Width + padding);
                    e.VAlign = 0.5f;
                    e.Top = StyleDimension.FromPixels(-2f);

                    e.OnClick += sortReverseButton_OnClick;
                }));
            }

            UIElement GridContainer = PageContentContainer.AddElement(new UIElement().With(e => {
                e.Top = StyleDimension.FromPixels(28f + padding * 0.8f);

                e.Width = StyleDimension.Fill;
                e.Height = StyleDimension.Fill;
            }));

            float elementSize = (GridContainer.GetOuterDimensions().Width - ((elementsPerRow - 1) * padding)) / elementsPerRow;
            elementsPerColumn = (int)((GridContainer.GetOuterDimensions().Height - padding) / (elementSize + padding));

            GridContainer.Height = StyleDimension.FromPixels((elementSize * elementsPerColumn) + (padding * (elementsPerColumn - 1)));
            GridContainer.Recalculate();

            pages = SplitList(allCatches, elementsPerColumn * elementsPerRow);
            if (pages.Count % 2 != 0) pages.Add(new());

            FishermansLogGrid Grid = GridContainer.AddElement(new FishermansLogGrid(padding, elementSize, pages[currentPage + i]));
            grids.Add(Grid);
            gridElements.AddRange(Grid._items);

            UIHoverImageButton paginateButton = PageContentContainer.AddElement(new UIHoverImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_" + (i == 0 ? "Back" : "Forward"), ReLogic.Content.AssetRequestMode.ImmediateLoad), (i == 0 ? "Previous" : "Next") + " Page").With(e => {
                if (i == 1) e.HAlign = 1f;
                e.VAlign = 1f;

                e.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border"));
                e.SetVisibility(1f, 1f);
                e.OnMouseOver += button_OnMouseOver;
                e.OnClick += paginateButton_OnClick;
            }));
        }

        pageCount = pages.Count;
    }

    private void sortReverseButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        UICheckbox target = (UICheckbox)listeningElement;

        reverseOrder = target.Selected;
        SortElements();
        sortButtonFunction();
    }

    private void SortElements() {
        switch (sortMethodIndex) {
            case 0:
                if (reverseOrder) allCatches = catches.Concat(questCatches).OrderByDescending(i => new Item(i).Name).ToList();
                else allCatches = catches.Concat(questCatches).OrderBy(i => new Item(i).Name).ToList();
                break;
            case 1:
                if (reverseOrder) allCatches = catches.Concat(questCatches).OrderByDescending(i => i).ToList();
                else allCatches = catches.Concat(questCatches).OrderBy(i => i).ToList();
                break;
        }
    }

    private void UpdateGrids() {
        SortElements();
        if (searchString != null) allCatches = allCatches.Where(e => new Item(e).Name.ToLower().Contains(searchString)).ToList();

        if (allCatches.Count == 0) pages = new() { new(), new() };
        else {
            pages = SplitList(allCatches, elementsPerColumn * elementsPerRow);
            if (pages.Count % 2 != 0) pages.Add(new());
        }

        currentPage = 0;
        pageCount = pages.Count;

        for (int i = 0; i < grids.Count; i++) {
            grids[i].SwitchElements(pages[currentPage + i], reverseOrder, numberedOrder);
        }
    }

    private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement) {
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        if (searchBar.HasContents) {
            searchBar.SetContents(null, forced: true);
            UpdateGrids();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
        else SoundEngine.PlaySound(SoundID.MenuTick);

        if (searchBar.IsWritingText) searchBar.ToggleTakingText();
    }

    private void button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement) {
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    private void sortButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        sortMethodIndex = sortMethodIndex >= sortMethods.Count - 1 ? 0 : sortMethodIndex + 1;
        sortButtonFunction();
    }

    private void sortButton_OnRightClick(UIMouseEvent evt, UIElement listeningElement) {
        sortMethodIndex = sortMethodIndex <= 0 ? sortMethods.Count - 1 : sortMethodIndex - 1;
        sortButtonFunction();
    }

    private void sortButtonFunction() {
        sortButtonText.SetText(sortMethods[sortMethodIndex]);

        gridElements.ForEach(e => {
            FishermansLogGridElement element = (FishermansLogGridElement)e;

            numberedOrder = false;
            switch (sortMethodIndex) {
                case 0:
                    element.Reorder(element.Name);
                    break;
                case 1:
                    element.Reorder(element.ItemID);
                    numberedOrder = true;
                    break;
            }
        });

        UpdateGrids();
    }

    private void paginateButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        SoundEngine.PlaySound(SoundID.MenuTick);
        UIHoverImageButton target = (UIHoverImageButton)listeningElement;

        if ((target.HoverText.StartsWith("Previous") && currentPage == 0) || (target.HoverText.StartsWith("Next") && currentPage == pageCount - 2)) return;

        currentPage += 2 * (target.HoverText.StartsWith("Previous") ? -1 : 1);
        for (int i = 0; i < grids.Count; i++) {
            grids[i].SwitchElements(pages[currentPage + i], reverseOrder, numberedOrder);
        }
    }

    private void click_SearchArea(UIMouseEvent evt, UIElement listeningElement) {
        if (searchBar == null) return;
        if (listeningElement == searchBar) searchBar.ToggleTakingText();
    }

    private void onSearchContentsChanged(string contents) {
        searchString = contents;

        if (contents == null) return;
        UpdateGrids();
    }

    public override void Update(GameTime gameTime) {
        if (Main.keyState.IsKeyDown(Keys.Escape) && !Main.oldKeyState.IsKeyDown(Keys.Escape)) {
            if (searchBar.IsWritingText) searchBar.ToggleTakingText();
        }
    }
}
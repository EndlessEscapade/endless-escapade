using EndlessEscapade.Common.FishermansLogUI;
using EndlessEscapade.Utilities.Extensions;
using System.Collections.Generic;
using Terraria.UI;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.Localization;
using Microsoft.Xna.Framework.Input;

namespace EndlessEscapade.Common.UIElements;

internal class FishermansLogGridScreen : UIElement
{
    public static List<List<int>> SplitList(List<int> inputList, int size) {
        var outputList = new List<List<int>>();

        foreach (var group in inputList.Select((value, index) => new { value, index }).GroupBy(x => x.index / size, x => x.value)) {
            outputList.Add(group.ToList());
        }

        return outputList;
    }

    public FishermansLogUIState.PageGrid grid;

    public List<List<int>> pages;
    public int currentScreen = 0;
    public string searchQuery = "";

    private List<FishermansLogGridPage> gridPages = new();
    private List<UIElement> bottomBarPanels = new();
    private List<UIHoverImageButton> paginateButtons = new();
    private List<UIText> pageNumbers = new();

    private UISearchBar searchBar { get; set; }

    public FishermansLogGridScreen(FishermansLogUIState.PageGrid grid, List<int> elements, UISearchBar searchBar) : base() {
        Width = StyleDimension.Fill;
        Height = StyleDimension.Fill;

        this.grid = grid;

        pages = SplitList(FishermansLogUIState.Fish, grid.Columns * grid.Rows);

        for (int i = 0; i < 1 + (pages.Count % 2); i++) {
            var ContentContainer = this.AddElement(new UIElement().With(e => {
                e.HAlign = 1f * i;
                e.VAlign = 0.5f;

                e.Width = StyleDimension.FromPixelsAndPercent(-6f, 0.5f);
                e.Height = StyleDimension.Fill;
                e.SetPadding(grid.Padding);
            }));

            var topBarPanel = ContentContainer.AddElement(new UIElement().With(e => {
                e.Width = StyleDimension.Fill;
                e.Height = StyleDimension.FromPixels(28f);
            }));

            if (i == 0) {
                var searchBarPanel = topBarPanel.AddElement(new UIPanel().With(e => {
                    e.Width = StyleDimension.Fill;
                    e.Height = StyleDimension.Fill;
                    e.SetPadding(0f);

                    e.BackgroundColor = new Color(191, 197, 201);
                    e.BorderColor = Color.Black * 0.25f;
                }));

                searchBar.VAlign = 0.5f;

                searchBar.Width = StyleDimension.Fill;
                searchBar.Height = StyleDimension.Fill;

                searchBar.OnClick += searchBar_OnClick;
                searchBar.OnRightClick += resetSearchBarText;
                searchBar.OnContentsChanged += searchBar_OnContentsChanged;

                searchBar.SetContents(null, true);
                this.searchBar = searchBar;

                topBarPanel.Append(searchBar);

                UIImageButton searchCancelButton = topBarPanel.AddElement(new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", ReLogic.Content.AssetRequestMode.ImmediateLoad)).With(e => {
                    e.HAlign = 1f;
                    e.VAlign = 0.5f;
                    e.Left = StyleDimension.FromPixels(-2f);

                    e.OnClick += resetSearchBarText;
                }));
            }

            var GridPageContainer = ContentContainer.AddElement(new UIElement().With(e => {
                e.HAlign = i * 1f;
                e.Top = StyleDimension.FromPixels(grid.Padding + 28f);

                e.Width = StyleDimension.Fill;
                e.Height = StyleDimension.FromPixelsAndPercent(-(grid.Padding * 2 + 56f), 1f);
            }));

            var GridPage = GridPageContainer.AddElement(new FishermansLogGridPage(grid, pages[i + currentScreen]).With(e => {
                e.HAlign = 0.5f;
                e.VAlign = 0.5f;

                gridPages.Add(e);
            }));

            var bottomBarPanel = ContentContainer.AddElement(new UIElement().With(e => {
                e.VAlign = 1f;

                e.Width = StyleDimension.Fill;
                e.Height = StyleDimension.FromPixels(28f);

                bottomBarPanels.Add(e);
            }));

            var paginateButton = bottomBarPanel.AddElement(new UIHoverImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_" + (i == 0 ? "Back" : "Forward"), ReLogic.Content.AssetRequestMode.ImmediateLoad), (i == 0 ? "Previous" : "Next") + " Page").With(e => {
                if (i == 1) e.HAlign = 1f;

                e.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border"));
                e.SetVisibility(1f, 1f);
                e.OnClick += paginateButton_OnClick;

                paginateButtons.Add(e);
            }));

            if (currentScreen == 0) paginateButtons[0].Remove();

            var pageNumberText = bottomBarPanel.AddElement(new UIText($"{currentScreen + i + 1}").With(e => {
                if (i % 2 == 0) e.HAlign = 1f;
                e.VAlign = 0.5f;

                pageNumbers.Add(e);
            }));
        }
    }

    private void UpdateElements(Func<int, bool> filter = null) {
        if (filter == null) {
            pages = SplitList(FishermansLogUIState.Fish, grid.Columns * grid.Rows);

            for (int i = 0; i < gridPages.Count; i++) {
                if (pages.Count % 2 != 0 && currentScreen * 2 == pages.Count - 1 && i == gridPages.Count - 1) {
                    gridPages[i].RemoveAllChildren();
                    paginateButtons[i].Remove();
                    pageNumbers[i].SetText("");
                    continue;
                }

                gridPages[i].UpdateElements(pages[i + currentScreen * gridPages.Count]);
                pageNumbers[i].SetText($"{currentScreen * 2 + i + 1}");

                if (currentScreen == 0) paginateButtons[0].Remove();
                else bottomBarPanels[0].Append(paginateButtons[0]);

                if (currentScreen * 2 == pages.Count - 1) paginateButtons[1].Remove();
                else bottomBarPanels[1].Append(paginateButtons[1]);
            }

            return;
        }

        currentScreen = 0;

        var filteredList = FishermansLogUIState.Fish.Where(filter).ToList();
        pages = SplitList(filteredList, grid.Columns * grid.Rows);

        for (int i = 0; i < gridPages.Count; i++) {
            if (pages.Count == 0 || (pages.Count % 2 != 0 && currentScreen * 2 == pages.Count - 1 && i == gridPages.Count - 1)) {
                gridPages[i].RemoveAllChildren();
                paginateButtons[i].Remove();
                pageNumbers[i].SetText("");
                continue;
            }

            pageNumbers[i].SetText($"{currentScreen * 2 + i + 1}");
            paginateButtons[0].Remove();
            if (pages.Count < 3) paginateButtons[1].Remove();

            gridPages[i].UpdateElements(pages[i + currentScreen * gridPages.Count]);
        }
    }

    private void searchBar_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        var target = (UISearchBar)listeningElement;
        target.ToggleTakingText();
    }

    private void searchBar_OnContentsChanged(string obj) {
        searchQuery = obj ?? "";
        if (searchQuery == "") UpdateElements();
        else UpdateElements(e => new Item(e).Name.ToLower().Contains(searchQuery));
    }

    private void resetSearchBarText(UIMouseEvent evt, UIElement listeningElement) {
        if (searchBar.HasContents) {
            SoundEngine.PlaySound(SoundID.MenuClose);
            searchBar.SetContents(null, forced: true);
        }
        else SoundEngine.PlaySound(SoundID.MenuTick);

        if (searchBar.IsWritingText) searchBar.ToggleTakingText();
    }

    private void paginateButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        SoundEngine.PlaySound(new SoundStyle("EndlessEscapade/Assets/Sounds/UI/FishermansLogOpen"));

        var target = (UIHoverImageButton)listeningElement;
        if (target.HoverText.Contains("Previous") && currentScreen > 0) currentScreen--;
        if (target.HoverText.Contains("Next") && currentScreen < Math.Floor((float)(pages.Count / 2))) currentScreen++;

        UpdateElements();
    }
}

internal class FishermansLogGridPage : UIElement
{
    FishermansLogUIState.PageGrid grid;

    public FishermansLogGridPage(FishermansLogUIState.PageGrid grid, List<int> elements) : base() {
        this.grid = grid;

        Width = StyleDimension.FromPixels((grid.Padding + grid.ElementSize) * grid.Columns + grid.Padding);
        Height = StyleDimension.FromPixels((grid.Padding + grid.ElementSize) * grid.Rows + grid.Padding);

        for (int i = 0; i < elements.Count; i++) {
            var GridElement = this.AddElement(new FishermansLogGridElement(StyleDimension.FromPixels(grid.ElementSize), elements[i]).With(e => {
                e.Left = StyleDimension.FromPixels((i % grid.Columns) * grid.ElementSize + (i % grid.Columns) * grid.Padding);
                e.Top = StyleDimension.FromPixels(((i / grid.Columns) % grid.Rows) * grid.ElementSize + ((i / grid.Columns) % grid.Rows) * grid.Padding);

                e.BorderColor = Color.Black * 0.25f;
            }));
        }
    }

    public void UpdateElements(List<int> elements) {
        RemoveAllChildren();

        for (int i = 0; i < elements.Count; i++) {
            var GridElement = this.AddElement(new FishermansLogGridElement(StyleDimension.FromPixels(grid.ElementSize), elements[i]).With(e => {
                e.Left = StyleDimension.FromPixels((i % grid.Columns) * grid.ElementSize + (i % grid.Columns) * grid.Padding);
                e.Top = StyleDimension.FromPixels(((i / grid.Columns) % grid.Rows) * grid.ElementSize + ((i / grid.Columns) % grid.Rows) * grid.Padding);

                e.BorderColor = Color.Black * 0.25f;
            }));
        }
    }
}
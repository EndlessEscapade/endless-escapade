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
using EndlessEscapade.Common.UIElements;

namespace EndlessEscapade.Common.FishermansLogUI;

public class FishermansLogUIState : UIState
{
    public static bool Visible { get; set; }

    private static List<int> GetFishItems() {
        List<int> items = new List<int>();

        for (int i = 0; i < 5125; i++) {
            Item item = new Item(i);

            if (ContentSamples.CreativeHelper.GetItemGroup(item, out int x) == ContentSamples.CreativeHelper.ItemGroup.FishingQuestFish) {
                items.Add(i);
            }
        }

        return items;
    }

    public static readonly List<int> Fish = GetFishItems();

    private enum ScreenType
    {
        Grid = 0,
        Information = 1
    }

    public struct PageGrid
    {
        public PageGrid(List<int> elements, float padding, int columns) {
            Elements = elements;
            Padding = padding;
            Columns = columns;
        }

        public List<int> Elements { get; set; }
        public float Padding { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public float ElementSize { get; set; }
    }

    private ScreenType currentScreen = ScreenType.Grid;
    private PageGrid grid = new(Fish, 15f, 5);

    public UISearchBar searchBar { get; set; }

    public override void OnInitialize() {
        // ELEMENT ATTRIBUTES SHOULD BE SORTED BY TYPE IN THIS ORDER:
        // - POSITIONING (Top, HAlign, etc.)
        // - DISPLAY & SIZING (Width, PaddingLeft, etc.)
        // - COLORS (BackgroundColor, BorderColor, etc.)
        // - EVERYTHING ELSE

        var MainPanel = this.AddElement(new UIImage(ModContent.Request<Texture2D>("EndlessEscapade/Assets/UI/FishermansLog/BackgroundPanel", ReLogic.Content.AssetRequestMode.ImmediateLoad)).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;

            e.Width = StyleDimension.FromPixels(800f);
            e.Height = StyleDimension.FromPixels(560f);
            e.SetPadding(12f);
        }));

        grid.ElementSize = (float)Math.Floor((((MainPanel.GetOuterDimensions().Width - 24f) / 2 - 6f) - ((grid.Columns + 1) * grid.Padding)) / grid.Columns);
        grid.Rows = (int)((MainPanel.GetOuterDimensions().Height - 80f - grid.Padding) / (grid.ElementSize + grid.Padding));

        searchBar = new(Language.GetText("Search"), 0.8f);

        var GridScreen = MainPanel.AddElement(new FishermansLogGridScreen(grid, Fish, searchBar));
    }

    public override void Update(GameTime gameTime) {
        Main.playerInventory = false;

        if (Main.keyState.IsKeyDown(Keys.Escape) && !Main.oldKeyState.IsKeyDown(Keys.Escape)) {
            if (searchBar.IsWritingText) searchBar.ToggleTakingText();
            else {
                SoundEngine.PlaySound(new SoundStyle("EndlessEscapade/Assets/Sounds/UI/FishermansLogClose"));
                Visible = false;
            }

            if (Main.keyState.IsKeyDown(Keys.Enter) && !Main.oldKeyState.IsKeyDown(Keys.Enter) && searchBar.IsWritingText) searchBar.ToggleTakingText();
        }
    }
}
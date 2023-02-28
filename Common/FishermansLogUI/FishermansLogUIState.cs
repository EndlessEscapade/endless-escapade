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

    public static bool Visible => !Main.playerInventory;
    private readonly string texturePath = "EndlessEscapade/Assets/UI";

    readonly List<int> catches = new List<int>() { ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.BlueJellyfish, ItemID.ChaosFish, ItemID.CrimsonTigerfish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.Ebonkoi, ItemID.FlarefinKoi, ItemID.Flounder, ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.GreenJellyfish, ItemID.Hemopiranha, ItemID.Honeyfin, ItemID.NeonTetra, ItemID.Obsidifish, ItemID.PinkJellyfish, ItemID.PrincessFish, ItemID.Prismite, ItemID.RedSnapper, ItemID.RockLobster, ItemID.Salmon, ItemID.Shrimp, ItemID.SpecularFish, ItemID.Stinkfish, ItemID.Trout, ItemID.Tuna, ItemID.VariegatedLardfish };

    readonly List<int> questCatches = new List<int>() { ItemID.AmanitaFungifin, ItemID.Angelfish, ItemID.Batfish, ItemID.BloodyManowar, ItemID.Bonefish, ItemID.BumblebeeTuna, ItemID.Bunnyfish, ItemID.CapnTunabeard, ItemID.Catfish, ItemID.Cloudfish, ItemID.Clownfish, ItemID.Cursedfish, ItemID.DemonicHellfish, ItemID.Derpfish, ItemID.Dirtfish, ItemID.DynamiteFish, ItemID.EaterofPlankton, ItemID.FallenStarfish, ItemID.TheFishofCthulu, ItemID.Fishotron, ItemID.Fishron, ItemID.GuideVoodooFish, ItemID.Harpyfish, ItemID.Hungerfish, ItemID.Ichorfish, ItemID.InfectedScabbardfish, ItemID.Jewelfish, ItemID.MirageFish, ItemID.Mudfish, ItemID.MutantFlinxfin, ItemID.Pengfish, ItemID.Pixiefish, ItemID.ScarabFish, ItemID.ScorpioFish, ItemID.Slimefish, ItemID.Spiderfish, ItemID.TropicalBarracuda, ItemID.TundraTrout, ItemID.UnicornFish, ItemID.Wyverntail, ItemID.ZombieFish };

    int currentPage = 0;
    List<List<int>> pages;
    List<FishermansLogGrid> grids = new();

    public override void OnInitialize() {
        List<int> allCatches = catches.Concat(questCatches).OrderBy(i => new Item(i).Name).ToList();

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
            UIPanel GridContainer = MainPanel.AddElement(new UIPanel().With(e => {
                e.HAlign = 1f * i;
                e.VAlign = 0.5f;

                e.Width = StyleDimension.FromPixelsAndPercent(-6f, 0.5f);
                e.SetPadding(0f);

                e.BackgroundColor = Color.Transparent;
                e.BorderColor = Color.Transparent;
            }));

            float padding = 15f;
            int elementsPerRow = 5;
            float elementSize = (GridContainer.GetOuterDimensions().Width - ((elementsPerRow + 1) * padding)) / elementsPerRow;

            GridContainer.Height = StyleDimension.FromPixelsAndPercent(-(elementSize + padding), 1f);
            GridContainer.Recalculate();

            int elementsPerColumn = (int)((GridContainer.GetOuterDimensions().Height - padding) / (elementSize + padding));

            pages = SplitList(allCatches, elementsPerColumn * elementsPerRow);
            pages.Add(new());

            FishermansLogGrid Grid = GridContainer.AddElement(new FishermansLogGrid(padding, elementSize, elementsPerRow, elementsPerColumn, pages[currentPage + i]));
            grids.Add(Grid);

            UIHoverImageButton paginateButton = MainPanel.AddElement(new UIHoverImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_" + (i == 0 ? "Back" : "Forward"), ReLogic.Content.AssetRequestMode.ImmediateLoad), (i == 0 ? "Previous" : "Next") + " Page").With(e => {
                e.Left = StyleDimension.FromPixels(padding * (i == 0 ? 1 : -1));
                e.Top = StyleDimension.FromPixelsAndPercent(-(elementSize + padding) / 2, 1f);

                if (i == 1) e.HAlign = 1f;

                e.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border"));
                e.SetVisibility(1f, 1f);
                e.OnMouseOver += paginateButton_OnMouseOver;
                e.OnClick += paginateButton_OnClick;
            }));
        }
    }

    private void paginateButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement) {
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    private void paginateButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        SoundEngine.PlaySound(SoundID.MenuTick);
        UIHoverImageButton target = (UIHoverImageButton)listeningElement;

        if ((target.HoverText.StartsWith("Previous") && currentPage == 0) || (target.HoverText.StartsWith("Next") && currentPage == pages.Count / 2)) return;

        currentPage += 2 * (target.HoverText.StartsWith("Previous") ? -1 : 1);
        for (int i = 0; i < grids.Count; i++) {
            grids[i].SwitchElements(pages[currentPage + i]);
        }
    }
}
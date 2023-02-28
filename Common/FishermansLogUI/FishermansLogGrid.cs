using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace EndlessEscapade.Common.FishermansLogUI;

internal class FishermansLogGrid : UIGrid
{
    private float ElementSize;

    public FishermansLogGrid(float padding, float elementSize, int elementsPerRow, int elementsPerColumn, List<int> elements) : base() {
        HAlign = 0.5f;
        VAlign = 0.5f;

        Width = StyleDimension.FromPixels(elementSize * elementsPerRow + padding * (elementsPerRow + 1));
        Height = StyleDimension.FromPixels((elementSize + padding) * elementsPerColumn + padding);
        ListPadding = padding;
        SetPadding(padding);

        ElementSize = elementSize;

        PopulateGrid(elements);
    }

    private void PopulateGrid(List<int> elements) {
        elements.ForEach(i => {
            FishermansLogGridElement Button = new FishermansLogGridElement(StyleDimension.FromPixels(ElementSize), i).With(e => {
                e.BorderColor = Color.Black * 0.25f;
            });

            Add(Button);
        });
    }

    public void SwitchElements(List<int> newElements) {
        Clear();

        PopulateGrid(newElements);
    }
}
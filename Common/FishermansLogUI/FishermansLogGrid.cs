using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace EndlessEscapade.Common.FishermansLogUI;

internal class FishermansLogGrid : UIGrid
{
    public FishermansLogGrid(float padding, float elementSize, int elementsPerRow, int elementsPerColumn, List<int> elements) : base() {
        HAlign = 0.5f;
        VAlign = 0.5f;

        Width = StyleDimension.FromPixels(elementSize * elementsPerRow + padding * (elementsPerRow + 1));
        Height = StyleDimension.FromPixels((elementSize + padding) * elementsPerColumn + padding);
        ListPadding = padding;
        SetPadding(padding);

        elements.ForEach(i => {
            FishermansLogGridElement Button = new FishermansLogGridElement(StyleDimension.FromPixels(elementSize), i).With(e => {
                e.BorderColor = Color.Black * 0.25f;
            });

            Add(Button);
        });
    }
}
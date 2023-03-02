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
    internal float ElementSize;

    public FishermansLogGrid(float padding, float elementSize, List<int> elements) : base() {
        HAlign = 0.5f;
        VAlign = 0.5f;

        Width = StyleDimension.Fill;
        Height = StyleDimension.Fill;
        ListPadding = padding;

        ElementSize = elementSize;

        PopulateGrid(elements, false, false);
    }

    public void PopulateGrid(List<int> elements, bool reverseOrder, bool numberedOrder) {
        elements.ForEach(i => {
            FishermansLogGridElement Button = new FishermansLogGridElement(StyleDimension.FromPixels(ElementSize), i, reverseOrder, numberedOrder).With(e => {
                e.BorderColor = Color.Black * 0.25f;
            });

            Add(Button);
        });
    }

    public void SwitchElements(List<int> newElements, bool reverseOrder, bool numberedOrder) {
        Clear();
        PopulateGrid(newElements, reverseOrder, numberedOrder);
    }
}
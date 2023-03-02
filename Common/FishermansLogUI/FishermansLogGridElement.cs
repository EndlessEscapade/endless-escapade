using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using System;

namespace EndlessEscapade.Common.FishermansLogUI;
internal class FishermansLogGridElement : UIPanel
{
    public readonly string Name;

    public string ComparisonString;
    public int ComparisonInt;

    public bool ReverseOrder;
    public bool NumberedOrder;

    public int ItemID;

    public FishermansLogGridElement(StyleDimension size, int itemID, bool reverseOrder, bool numberedOrder) : base() {
        Name = new Item(itemID).Name;
        ComparisonString = Name;
        ComparisonInt = itemID;
        ReverseOrder = reverseOrder;
        NumberedOrder = numberedOrder;
        ItemID = itemID;

        Width = size;
        Height = size;

        BackgroundColor = Color.Transparent;

        UIHoverItemIcon ButtonIcon = this.AddElement(new UIHoverItemIcon(itemID).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }

    public void Reorder(string comparison) {
        ComparisonString = comparison;
    }

    public void Reorder(int comparison) {
        ComparisonInt = comparison;
    }

    public override int CompareTo(object obj) {
        FishermansLogGridElement other = obj as FishermansLogGridElement;

        if (!NumberedOrder) {
            if (ReverseOrder) return other.ComparisonString.CompareTo(ComparisonString);
            return ComparisonString.CompareTo(other.ComparisonString);
        }

        if (ReverseOrder) return other.ComparisonInt.CompareTo(ComparisonInt);
        return ComparisonInt.CompareTo(other.ComparisonInt);
    }
}

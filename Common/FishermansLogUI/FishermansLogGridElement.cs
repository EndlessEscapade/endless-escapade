using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using EndlessEscapade.Utilities.Extensions;
using Terraria;

namespace EndlessEscapade.Common.FishermansLogUI;
internal class FishermansLogGridElement : UIPanel
{
    public readonly string Name;

    public FishermansLogGridElement(StyleDimension size, int itemID) : base() {
        Name = new Item(itemID).Name;

        Width = size;
        Height = size;

        BackgroundColor = Color.Transparent;

        UIHoverItemIcon ButtonIcon = this.AddElement(new UIHoverItemIcon(itemID).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }

    public override int CompareTo(object obj) {
        FishermansLogGridElement other = obj as FishermansLogGridElement;
        return Name.CompareTo(other.Name);
    }
}

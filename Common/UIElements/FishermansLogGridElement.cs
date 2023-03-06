using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using EndlessEscapade.Utilities.Extensions;
using Terraria;

namespace EndlessEscapade.Common.UIElements;
internal class FishermansLogGridElement : UIPanel
{
    public FishermansLogGridElement(StyleDimension size, int itemID) : base() {
        Width = size;
        Height = size;

        BackgroundColor = Color.Transparent;

        var ButtonIcon = this.AddElement(new UIHoverItemIcon(itemID).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }

    public override int CompareTo(object obj) {
        var other = obj as FishermansLogGridElement;
        return CompareTo(other);
    }
}

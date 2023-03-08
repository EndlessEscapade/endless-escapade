using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace EndlessEscapade.Common.UIElements;
internal class FishermansLogGridElement : UIPanel
{
    public FishermansLogGridElement(StyleDimension size, int itemID) : base() {
        Width = size;
        Height = size;

        BackgroundColor = Color.Transparent;
        BorderColor = Color.Black * 0.25f;

        var ButtonIcon = this.AddElement(new UIHoverItemIcon(itemID).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (IsMouseHovering) {
            BackgroundColor = Color.Gold * 0.25f;
            BorderColor = Color.Gold;
        }
        else {
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Black * 0.25f;
        }
    }
}
